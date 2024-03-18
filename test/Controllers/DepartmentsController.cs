using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using test.Models;


[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly string _connectionString;

    public DepartmentsController(IConfiguration configuration)
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



    // GET: api/Departments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        try
        {
            var sql = "SELECT * FROM Departments;";
            var departments = await QueryAsync<Department>(sql, new Dictionary<string, object>(), reader => new Department
            {
                DepartmentId = reader.GetInt32("DepartmentId"),
                Name = reader.GetString("Name")
            });
            return Ok(departments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

