using ContactBook.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Data.Common;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Text.Json;
using ContactBook.Models.Base;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace ContactBook.DAL
{
    public class SqliteDataAccess
    {
        private string _connectionString;
        private SQLiteConnection _connection;
        public SqliteDataAccess()
        {
            var config = new ConfigurationBuilder()
                                .AddUserSecrets<DBRepository>()
                                .Build();
            _connectionString = config.GetConnectionString("SQLiteConnection");
        }

        /// <summary>
        /// Retrieves all data from database according to sql command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<T>> SelectDataAsync<T>(string sql) where T : class 
        {
            var result = await InteractWithDBAsync<IEnumerable<T>>(() => ReadDataAsync<T>(new SQLiteCommand(sql, _connection)));
            return result.ToList();
        }

        public async Task<int> GetCountOf<T>(string sql)
        {
            var result = await InteractWithDBAsync<int>(() => GetCountOf<int>(new SQLiteCommand(sql, _connection)));
            return result;
        }

        /// <summary>
        /// Saves entity to database while returning the entity with its id retrieved from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> SaveDataAsync<T>(T entity) where T : BaseEntity
        {
            var result = await InteractWithDBAsync<Result<T>>(() => InsertEntity<T>(entity));
            return result.Value;
        }

        /// <summary>
        /// Connects an SQL command string to a connection.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private SQLiteCommand GetConnectedCommand(string command, SQLiteConnection connection) =>
                                new SQLiteCommand() { CommandText = command, Connection = connection };


        /// <summary>
        /// Formats a string to its SQL equivalent format.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ToSQLFormat(string str)
        {
            string pattern = @"(?<!^)(?=[A-Z])";

            string[] split = Regex.Split(str, pattern, RegexOptions.IgnorePatternWhitespace);

            return String.Join('_', split).ToLower();
        }

        /// <summary>
        /// Creates an expando object of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ExpandoObject SQLParameters<T>(T entity)
        {
            var sqlParameters = new ExpandoObject();
            var parameters = entity.GetType().GetProperties();
            foreach (var parameter in parameters)
            {
                sqlParameters.TryAdd(ToSQLFormat(parameter.Name), parameter.GetValue(entity));
            }
            return sqlParameters;
        }

        /// <summary>
        /// Reads rows and copies down all entities of type T from a database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns>IEnumerable of all read entities found with the command.</returns>
        private async Task<IEnumerable<T>> ReadDataAsync<T>(SQLiteCommand command) where T : class 
        {
            try
            {
                T entity;
                var dictionary = GetPropertiesOf<T>();
                List<ExpandoObject> rawEntities = new();
                List<T> entities = new List<T>();

                command.Connection = _connection;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        ExpandoObject dynamicObject = new();
                        foreach (var property in dictionary.Keys)
                        {
                            var parsedProperty = ToSQLFormat(property);
                            var value = ConvertFromDBVal<object>(reader[parsedProperty]);
                            dynamicObject.TryAdd(property, value);
                        }

                        rawEntities.Add(dynamicObject);
                        
                        
                    }
                }
                rawEntities.ForEach(rawEntity =>
                {
                    if (SerializeTo(out entity, rawEntity))
                    {
                        entities.Add(entity);
                    }
                });

                return entities;

            }
            catch (NpgsqlException npgEx) { throw; }
            catch (SQLiteException sqliteEx) { throw; }
        }

        private async Task<int> GetCountOf<T>(SQLiteCommand command)
        {
            int count = 0;
            command.Connection = _connection;
           
            count = Convert.ToInt32(command.ExecuteScalar());                
            return count;
        }

        /// <summary>
        /// Opens and closes a database connection while allowing for interaction with the database to be made.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="interactionTask"></param>
        /// <returns></returns>
        private async Task<TResult> InteractWithDBAsync<TResult>(Func<Task<TResult>> interactionTask)
        {
            try
            {
                _connection = new SQLiteConnection(_connectionString);
                await _connection.OpenAsync();
                var result = await interactionTask();

                _connection.Close();
                _connection.Dispose();
                return result;
            }
            catch (PostgresException ex){throw ex;}
            catch (SQLiteException sqliteEx) { throw sqliteEx; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private (string, string) FormatToValues(Dictionary<string, object> properties)
        {

            string value1 = EntryToInsertValue(properties.Keys.ToList<object>());

            var atProperties = new List<object>();  
            properties.Keys
                .ToList()
                .ForEach(x => 
                { 
                    atProperties.Add(new string($"{x}"));}
                );

            string value2 = EntryToInsertValue(atProperties, isParameter: true);

            return (value1, value2);
        }

        /// <summary>
        /// Converts a list of objects into insert values.
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        private string EntryToInsertValue(List<object> entries, bool isParameter = false)
        {
            if (entries.Count == 0) 
                return string.Empty;

            string values = string.Empty;
            string prepend = (isParameter) ? "@" : string.Empty;
            entries.ForEach(key => { values += $"{prepend}{ToSQLFormat(key.ToString())}, ";});

            return values.Substring(0, values.Length - 2);
        }

        private async Task<Result<T>> InsertEntity<T>(T entity) where T : BaseEntity
        {
            var result = new Result<T>();
            try
            {
                using var transaction = await _connection.BeginTransactionAsync();

                var dictionary = GetPropertiesOf<T>(entity, toDatabase: true);
                (string name, string value) values = FormatToValues(dictionary);
                string tableName = entity.GetType().Name.ToLower();

                var command = new SQLiteCommand($"INSERT INTO {tableName} ({values.name}) VALUES ({values.value}) RETURNING id;");

                AddWithValues(command, dictionary);
                command.Connection = _connection;

                //AddWithValues(command, dictionary);

                var id = await command.ExecuteScalarAsync();

                entity.Id = Convert.ToInt32(id);
                result.Value = entity;
                result.Success = true;

                await transaction.CommitAsync();
                return result;

            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
                result.Success = false;
                result.Value = default;
                result.Exception = ex;
                return result;

            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.Value = default;
                result.Success = false;
                return result;
            }
        }

        
        /// <summary>
        /// Might not need to be a string,object dictionary, could potentially be a list since the values are not used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>A string, object dictionary containing all public properties and their values.</returns>
        private Dictionary<string, object> GetPropertiesOf<T>(T entity = default, bool toDatabase = false)
        {
            var properties = new Dictionary<string, object>();

            //Gets all properties if entity is null, else gets all properties except Id.
            var entityProperties = (entity == null) ? typeof(T).GetProperties() :
                entity.GetType().GetProperties().Where(property => property.Name != "Id");

            //Populates properties dictionary with either values of each property type if entity is null
            //else converts all property values to DBValues.
            for (int i = 0; i < entityProperties.Count(); i++)
            {
                if (!properties.ContainsKey(entityProperties.ElementAt(i).Name))
                {
                    object value = (entity == null) ? entityProperties.ElementAt(i).PropertyType
                                                                        :
                                                     ConvertToDBVal<object>(entity
                                                     .GetType()
                                                    .GetProperty(entityProperties.ElementAt(i).Name)
                                                    .GetValue(entity, null));

                    properties[entityProperties.ElementAt(i).Name] = value;
                }
            }

            //Removes all properties without values for when an entry is being added to the database.
            if (toDatabase)
            {
                properties = properties
                    .TakeWhile(x => ConvertFromDBVal<object>(x.Value) != default)
                    .ToDictionary();
            }

            

            return properties;
        }

        /// <summary>
        /// Adds each property with its name in properties to the command parameters.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="properties"></param>
        private void AddWithKeys(SQLiteCommand command, Dictionary<string, object> properties)
        {
            var dictionary = new Dictionary<string, object>();
            properties.ToList().ForEach(entry =>
            {
                dictionary[entry.Key] = entry.Key;
            });

            AddWithValues(command, dictionary);
        }

        /// <summary>
        /// Adds each property with its value in properties to the command parameters.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="properties"></param>
        private void AddWithValues(SQLiteCommand command, Dictionary<string, object> properties)
        {
            foreach (var property in properties)
            {
                command.Parameters.AddWithValue($"{ToSQLFormat(property.Key)}", property.Value);
            }
        }

        /// <summary>
        /// Converts an expando object to the type of T given that the expando object already mimicks T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expandoObject"></param>
        /// <returns></returns>
        private bool SerializeTo<T>(out T entity, ExpandoObject expandoObject)
        {
            entity = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(expandoObject));
            return entity != null;
        }

        private static T? ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default; // returns the default value for the type
            }
            return (T)obj;
        }

        private static object ConvertToDBVal<T>(object obj)
        {
            if (obj == null || obj == string.Empty)
            {
                return DBNull.Value;
            }
            return (T)obj;
        }
        
    }
}
