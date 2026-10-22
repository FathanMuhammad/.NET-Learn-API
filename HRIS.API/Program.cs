using HRIS.API.Extensions;
using HRIS.API.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHrisServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()  //Mengizinkan semua jenis HTTP Request Header yang dikirim oleh klien.
              .AllowAnyMethod() //Mengizinkan semua jenis HTTP Method(Verb).
              .AllowAnyOrigin();  //Mengizinkan semua asal domain untuk mengakses API Anda.
    });
});

// Register OpenAPI services.
builder.Services.AddOpenApi();

/*
 * Pipeline
 * Ketika hit run, akan dijalankan app ini
 * akan cek apakah env nya dev dengan if IsDev....
 * 
 */
var app = builder.Build();

app.UseRequestTiming();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Generate OpenAPI document at /openapi/v1.json
    app.MapOpenApi();

    // Configure Swagger UI to read the OpenAPI document.
    app.UseSwaggerUI(options =>
    {
        options.ConfigObject.AdditionalItems["layout"] = "BaseLayout";
        options.SwaggerEndpoint(
            "/openapi/v1.json","HRIS API");
    });
}

app.UseHttpsRedirection(); // app.Use ini adalah middleware yang diproses pipeline sblm app run

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

//app.Run(async context =>
//{
//    Console.WriteLine("Middleware 3: Terminal - handling request");
//    await context.Response.WriteAsync("Hello Word");
//});
