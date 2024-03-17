using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Reflection.PortableExecutable;
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
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        using var command = new MySqlCommand(sql, connection);
        foreach (var param in parameters)
        {
            command.Parameters.AddWithValue(param.Key, param.Value);
        }
        using var reader = await command.ExecuteReaderAsync();
        var results = new List<T>();
        while (await reader.ReadAsync())
        {
            results.Add(map(reader));
        }
        return results;
    }


    // GET: api/Events
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessEvent>>> GetAccessEvents(int pageNumber = 1, int pageSize = 10)
    {
        var offset = (pageNumber - 1) * pageSize; // кількість записів, які треба пропустити
                                                  // Наприклад, якщо pageNumber = 2 і pageSize = 10, offset буде 10.
                                                  // Це означає, що перші 10 рядків будуть пропущені і дані почнуть вибиратись з 11-го рядка.
        var sql = "SELECT * FROM AccessEvents LIMIT @offset, @pageSize;";
        var parameters = new Dictionary<string, object>
    {
        {"@offset", offset},
        {"@pageSize", pageSize}
    };

        try
        {
            var events = await QueryAsync(sql, parameters, reader => new AccessEvent
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    // GET: api/Events/count
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetAccessEventsCount() // отримати загальну кількість записів
    {
        try
        {
            var sql = "SELECT COUNT(*) FROM AccessEvents;";
            var count = await QueryAsync<int>(sql, new Dictionary<string, object>(), reader => reader.GetInt32(0));
            return Ok(count.FirstOrDefault());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}



