using Microsoft.Extensions.Options;
using test.Data;

var builder = WebApplication.CreateBuilder(args);

// add Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();


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

// add DB connection string
builder.Services.AddScoped(_ => new DatabaseContext(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// use CORS
app.UseCors();

// use Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = string.Empty; // Це змусить Swagger відкриватися за замовчуванням
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
