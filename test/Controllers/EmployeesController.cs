using Microsoft.AspNetCore.Mvc;
using test.Models;
using test.Data;
using MySqlConnector;
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly string _connectionString = "server=localhost;port=3307;database=test;user=root;password=root";

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

    private async Task<int> ExecuteAsync(string sql, Dictionary<string, object> parameters)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        using var command = new MySqlCommand(sql, connection);
        foreach (var param in parameters)
        {
            command.Parameters.AddWithValue(param.Key, param.Value);
        }
        return await command.ExecuteNonQueryAsync();
    }

    // GET: api/Employees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        var sql = "SELECT * FROM Employees";
        var employees = await QueryAsync(sql, new Dictionary<string, object>(), reader => new Employee
        {
            EmployeeId = reader.GetInt32("EmployeeId"),
            Name = reader.GetString("Name"),
            Position = reader.GetString("Position"),
            DepartmentId = reader.GetInt32("DepartmentId")
        });
        return Ok(employees);
    }

    // GET: api/Employees/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var sql = "SELECT * FROM Employees WHERE EmployeeId = @id";
        var parameters = new Dictionary<string, object>
            {
                {"@id", id}
            };
        var employees = await QueryAsync(sql, parameters, reader => new Employee
        {
            EmployeeId = reader.GetInt32("EmployeeId"),
            Name = reader.GetString("Name"),
            Position = reader.GetString("Position"),
            DepartmentId = reader.GetInt32("DepartmentId")
        });
        var employee = employees.FirstOrDefault();
        if (employee == null)
        {
            return NotFound();
        }
        return employee;
    }

    // POST: api/Employees
    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    {
        var sql = "INSERT INTO Employees (Name, Position, DepartmentId) VALUES (@Name, @Position, @DepartmentId)";
        var parameters = new Dictionary<string, object>
            {
                {"@Name", employee.Name},
                {"@Position", employee.Position},
                {"@DepartmentId", employee.DepartmentId}
            };
        var result = await ExecuteAsync(sql, parameters);
        return CreatedAtAction(nameof(GetEmployee), new { id = result }, employee);
    }

    // PUT: api/Employees/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, Employee employee)
    {
        var sql = "UPDATE Employees SET Name = @Name, Position = @Position, DepartmentId = @DepartmentId WHERE EmployeeId = @id";
        var parameters = new Dictionary<string, object>
            {
                {"@id", id},
                {"@Name", employee.Name},
                {"@Position", employee.Position},
                {"@DepartmentId", employee.DepartmentId}
            };
        var result = await ExecuteAsync(sql, parameters);
        if (result == 0)
        {
            return NotFound();
        }
        return NoContent();
    }

    // DELETE: api/Employees/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var sql = "DELETE FROM Employees WHERE EmployeeId = @id";
        var parameters = new Dictionary<string, object>
            {
                {"@id", id}
            };
        var result = await ExecuteAsync(sql, parameters);
        if (result == 0)
        {
            return NotFound();
        }
        return NoContent();
    }

    private async Task<bool> EmployeeExists(int id)
    {
        var sql = "SELECT COUNT(*) FROM Employees WHERE EmployeeId = @id";
        var parameters = new Dictionary<string, object>
            {
                {"@id", id}
            };
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        using var command = new MySqlCommand(sql, connection);
        foreach (var param in parameters)
        {
            command.Parameters.AddWithValue(param.Key, param.Value);
        }
        var result = (long)await command.ExecuteScalarAsync();
        return result > 0;
    }
}