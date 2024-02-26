using MySqlConnector;

namespace test.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<T>> QueryAsync<T>(string sql, Func<MySqlDataReader, T> map)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            var results = new List<T>();
            while (await reader.ReadAsync())
            {
                results.Add(map(reader));
            }

            return results;
        }

        // Insert
        public async Task<int> InsertAsync(string sql, Dictionary<string, object> parameters)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(sql, connection);
            AddParameters(command, parameters);

            return await command.ExecuteNonQueryAsync();
        }

        // Update
        public async Task<int> UpdateAsync(string sql, Dictionary<string, object> parameters)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(sql, connection);
            AddParameters(command, parameters);

            return await command.ExecuteNonQueryAsync();
        }

        //Delete
        public async Task<int> DeleteAsync(string sql, Dictionary<string, object> parameters)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(sql, connection);
            AddParameters(command, parameters);

            return await command.ExecuteNonQueryAsync();
        }

        // =====================================
        private void AddParameters(MySqlCommand command, Dictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }
        }
    }
}
