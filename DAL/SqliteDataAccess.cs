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

        public List<Contact> LoadContacts()
        {
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                var output = connection.Query<Contact>("select * from contact", new DynamicParameters());
                return output.ToList();
            }
        }

        public void SaveContact(Contact contact)
        {
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Execute("insert into contact (first_name, last_name) values (@FirstName, @LastName)", contact);
            }
        }

        public async Task<List<T>> SelectDataAsync<T, U>(string sql, U parameters)
        {
            try
            {
                SQLiteConnection connection = new SQLiteConnection(_connectionString);
                connection.Open();
                using (connection)
                {
                    var result = await ReadDataAsync<T>(GetConnectedCommand(sql, connection));
                    return result.ToList();
                }
            }
            catch (SQLiteException ex)
            {
                var cpde = ex.ErrorCode;
                throw ex;
            }

        }

        public SQLiteCommand GetConnectedCommand(string command, SQLiteConnection connection) =>
                                new SQLiteCommand() { CommandText = command, Connection = connection };

        ///// <summary>
        ///// Opens an asynchronous connection using the global connection string.
        ///// </summary>
        ///// <returns></returns>
        //private async Task<SQLiteConnection> GetConnectionAsync()
        //{
            
        //    var connection = new SQLiteConnection(_connectionString);

        //    connection.OpenAsync();

        //    var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        //    var dataSource = dataSourceBuilder.Build();

        //    return await dataSource.OpenConnectionAsync();
        //}

        public string ToSQLFormat(string str)
        {
            string pattern = @"(?<!^)(?=[A-Z])";

            string[] split = Regex.Split(str, pattern, RegexOptions.IgnorePatternWhitespace);

            return String.Join('_', split).ToLower();
        }

        public ExpandoObject SQLParameters<T>(T entity)
        {
            var sqlParameters = new ExpandoObject();
            var parameters = entity.GetType().GetProperties();
            foreach (var parameter in parameters)
            {
                sqlParameters.TryAdd(ToSQLFormat(parameter.Name), parameter.GetValue(entity));
            }
            return sqlParameters;
        }

        private async Task<IEnumerable<T>> ReadDataAsync<T>(DbCommand command)
        {
            try
            {


                T entity;
                var dictionary = GetPropertiesOf<T>();
                List<ExpandoObject> rawEntities = new();
                List<T> entities = new List<T>();

                //command.Connection = _connection;
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

        public async Task<bool> InsertEntity<T>(T entity)
        {
            try
            {
                using var transaction = await _connection.BeginTransactionAsync();

                var dictionary = GetPropertiesOf<T>(entity);

                dictionary.Any();

                string objectType = entity.GetType().Name;

                string sqlValue = "";
                string csValue = "";
                var command = new SQLiteCommand($"insert into {objectType} ({sqlValue}) values({csValue})");
                command.Connection = _connection;

                AddWithValues(command, dictionary);

                await command.ExecuteScalarAsync();
                await transaction.CommitAsync();
                return true;

            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Might not need to be a string,object dictionary, could potentially be a list since the values are not used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>A string, object dictionary containing all public properties and their values.</returns>
        private Dictionary<string, object> GetPropertiesOf<T>(T entity = default)
        {

            var properties = new Dictionary<string, object>();



            var entityProperties = (entity == null) ? typeof(T).GetProperties() :
                entity.GetType().GetProperties().Where(property => property.Name != "Id");

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

            return properties;
        }

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
