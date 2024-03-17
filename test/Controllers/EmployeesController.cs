using Microsoft.AspNetCore.Mvc;
using test.Models;
using test.Data;
using MySqlConnector;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly string _connectionString;

    public EmployeesController(IConfiguration configuration)
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
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var offset = (pageNumber - 1) * pageSize; 
            var sql = "SELECT * FROM Employees LIMIT @offset, @pageSize;";
            var parameters = new Dictionary<string, object>
            {
                {"@offset", offset},
                {"@pageSize", pageSize}
            };

            var employees = await QueryAsync(sql, parameters, reader => new Employee 
            {
                EmployeeId = reader.GetInt32("EmployeeId"),
                Name = reader.GetString("Name"),
                Position = reader.GetString("Position"),
                DepartmentId = reader.GetInt32("DepartmentId")
            });
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/Employee/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        try
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/Employees/search?name=John
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployees(string name)
    {
        try
        {
            var sql = "SELECT * FROM Employees WHERE Name LIKE @Name";
            var parameters = new Dictionary<string, object>
            {
                {"@Name", $"%{name}%"}
            };
            var employees = await QueryAsync(sql, parameters, reader => new Employee
            {
                EmployeeId = reader.GetInt32("EmployeeId"),
                Name = reader.GetString("Name"),
                Position = reader.GetString("Position"),
                DepartmentId = reader.GetInt32("DepartmentId")
            });
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    // POST: api/Employees
    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    {
        try
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/Employees/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, Employee employee)
    {
        try
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/Employees/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/Employees/count
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetEmployeesCount() // отримати загальну кількість записів
    {
        try
        {
            var sql = "SELECT COUNT(*) FROM Employees;";
            var count = await QueryAsync<int>(sql, new Dictionary<string, object>(), reader => reader.GetInt32(0));
            return Ok(count.FirstOrDefault());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    } 

    //private async Task<bool> EmployeeExists(int id)
    //{
    //    var sql = "SELECT COUNT(*) FROM Employees WHERE EmployeeId = @id";
    //    var parameters = new Dictionary<string, object>
    //    {
    //        {"@id", id}
    //    };
    //    using var connection = new MySqlConnection(_connectionString);
    //    await connection.OpenAsync();
    //    using var command = new MySqlCommand(sql, connection);
    //    foreach (var param in parameters)
    //    {
    //        command.Parameters.AddWithValue(param.Key, param.Value);
    //    }
    //    var result = (long)await command.ExecuteScalarAsync();
    //    return result > 0;
    //}
}
