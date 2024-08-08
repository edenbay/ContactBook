using ContactBook.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace ContactBook.DAL
{
    internal class DBRepository
    {
        private readonly string _connectionString;
       
        public DBRepository()
        {
            var config = new ConfigurationBuilder()
                                 .AddUserSecrets<DBRepository>()
                                 .Build();
            _connectionString = config.GetConnectionString("DefaultConnection");
        }


        /// <summary>
        /// Opens an asynchronous connection using the global connection string.
        /// </summary>
        /// <returns></returns>
        private async Task<NpgsqlConnection> GetConnectionAsync()
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
            var dataSource = dataSourceBuilder.Build();

            return await dataSource.OpenConnectionAsync();
        }


        /// <summary>
        /// Might not need to be a string,object dictionary, could potentially be a list since the values are not used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>A string, object dictionary containing all public properties and their values.</returns>
        private Dictionary<string, object> GetPropertiesOf<T>() {
            Dictionary<string, object> properties
                = new Dictionary<string, object>();

            foreach (var property in typeof(T).GetProperties())
            {
                if (!properties.ContainsKey(property.Name))
                {
                    properties[property.Name] = property.PropertyType;
                }

            }

            return properties;
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

        private async Task<IEnumerable<T>> ReadEntitiesAsync<T>(NpgsqlCommand command) where T : BaseEntity
        {
            T entity;
              
            var dictionary = GetPropertiesOf<T>();
            List<ExpandoObject> rawEntities = new();
            List<T> entities = new List<T>();
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

        public string ToSQLFormat(string str)
        {
            string pattern = @"(?<!^)(?=[A-Z])";

            string[] split = Regex.Split(str, pattern, RegexOptions.IgnorePatternWhitespace);

            return String.Join('_',split).ToLower();
        }

        private async Task<IEnumerable<TResult>> InteractWithDatabaseAsync<TResult>(Func<NpgsqlCommand, TResult> func) where TResult : IEnumerable<TResult>
        {
            var result = new List<TResult>();
            TResult entity;
            ExpandoObject dynamicObject = new();
            var dictionary = GetPropertiesOf<TResult>();
            try
            {
                var conn = await GetConnectionAsync();

                result = func as List<TResult>;
                

                conn.Close();
                return result;
            }
            catch (PostgresException ex)
            {
                throw ex;
            }
        }

        public async Task InsertEntityAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                var conn = await GetConnectionAsync();

                using var command = new NpgsqlCommand($@"INSERT INTO {ToSQLFormat(typeof(T).Name)}s VALUES()");
            }
            catch (PostgresException ex)
            {
                throw ex;
            }
        }

        public async Task<T> SelectEntityAsync<T>(int id) where T : BaseEntity
        {
            var conn = await GetConnectionAsync();
            var command = new NpgsqlCommand($@"SELECT * FROM contacts WHERE id=@id");
            command.Parameters.AddWithValue("id", id);
            command.Connection = conn;
            var commandstring = $@"SELECT * FROM contacts WHERE contact_id=@id";

            var result = await ReadEntitiesAsync<T>(command);
             
            //Task<IEnumerable<T>> result = await InteractWithDatabaseAsync<T>(ReadEntitiesAsync<T>(command));

            return result.First();
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
