using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Npgsql;
using System.Data;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Windows;

namespace ContactBook.DAL
{
    public class SqlDataAccess
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;
        public SqlDataAccess()
        {
            var config = new ConfigurationBuilder()
                                .AddUserSecrets<DBRepository>()
                                .Build();
            _connectionString = config.GetConnectionString("DefaultConnection");

            string youtube = "https://www.youtube.com/watch?v=n7mGEh4_06c";
        }



        //public async Task<List<TResult>> LoadDataAsync<T, TResult>(string sql, T parameters)
        //{
        //    try
        //    {
        //        NpgsqlConnection connection = new(_connectionString);

        //        connection.Open();
        //        var trsult = new List<TResult>();    
        //        var @params = SQLParameters(parameters);

        //        using (connection)
        //        {
        //            var rows = await connection.QueryAsync<ExpandoObject>(sql, @params);

        //            return trsult;
        //        }
        //    }
        //    catch (PostgresException ex) 
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<int> SavaDataAsync<T, TResult>(string sql, T parameters)
        //{
        //    using (IDbConnection connection = await GetConnectionAsync())
        //    {
        //        int rowsAffected = await connection.ExecuteAsync(sql, parameters);
        //        return rowsAffected;
        //    }
        //}

        public async Task<List<T>> SelectDataAsync<T,U>(string sql, U parameters)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using (connection)
                {
                    var result = await ReadDataAsync<T>(GetConnectedCommand(sql, connection));
                    return result.ToList();
                } 
            }
            catch (PostgresException ex)
            { 
                throw ex;
            }

        }

        public NpgsqlCommand GetConnectedCommand(string command, NpgsqlConnection connection) =>
                                new NpgsqlCommand() { CommandText = command, Connection = connection};
        



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

        private async Task<IEnumerable<T>> ReadDataAsync<T>(NpgsqlCommand command)
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
            catch (NpgsqlException npgEx){throw;}
            
        }

        private async Task<TResult> InteractWithDBAsync<TResult>(Func<Task<TResult>> interactionTask)
        {
            try
            {
                _connection = await GetConnectionAsync();
                var result = await interactionTask();

                _connection.Close();
                _connection.Dispose();
                return result;
            }
            catch (PostgresException ex)
            {
                throw ex;
            }
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
                var command = new NpgsqlCommand($"insert into {objectType} ({sqlValue}) values({csValue})");
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

        private void AddWithValues(NpgsqlCommand command, Dictionary<string, object> properties)
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
