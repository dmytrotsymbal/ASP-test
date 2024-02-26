using test.Data;

var builder = WebApplication.CreateBuilder(args);

// add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // URL React app
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// Реєстрація DatabaseContext у DI контейнері
builder.Services.AddScoped(_ => new DatabaseContext(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// use CORS
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
