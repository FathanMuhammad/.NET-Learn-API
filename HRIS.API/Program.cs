
using HRIS.API.Extensions;
using HRIS.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHrisServices();

// Register OpenAPI services.
builder.Services.AddOpenApi();

/*
 * Pipeline
 * Ketika hit run, akan dijalankan app ini
 * akan cek apakah env nya dev dengan if IsDev....
 * 
 */
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Generate OpenAPI document at /openapi/v1.json
    app.MapOpenApi();

    // Configure Swagger UI to read the OpenAPI document.
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json","HRIS API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
