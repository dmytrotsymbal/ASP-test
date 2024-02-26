using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;
using test.Models;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly string _connectionString;

    public EventsController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private async Task<List<T>> QueryAsync<T>(string sql, Dictionary<string, object> parameters, Func<MySqlDataReader, T> map)
    {
        var results = new List<T>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand(sql, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(map(reader));
                    }
                }
            }
        }
        return results;
    }

    // GET: api/AccessEvents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessEvent>>> GetAccessEvents()
    {
        var sql = "SELECT * FROM AccessEvents";
        var events = await QueryAsync(sql, new Dictionary<string, object>(), reader => new AccessEvent
        {
            EventId = reader.GetInt32("EventId"),
            EmployeeId = reader.GetInt32("EmployeeId"),
            RoomId = reader.GetInt32("RoomId"),
            EventType = reader.GetString("EventType"),
            EventTimestamp = reader.GetDateTime("EventTimestamp"),
            AccessDenied = reader.GetBoolean("AccessDenied")
        });
        return Ok(events);
    }

}
