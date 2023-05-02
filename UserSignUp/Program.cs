using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using UserSignUp.Data;
using UserSignUp.Interfaces;
using UserSignUp.Models;
using UserSignUp.Repositories;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<SqlDbContext>(options => options.UseInMemoryDatabase("UserDatabaseInMemory"));
builder.Services.AddScoped<IRepo<User>, UserRepo>();
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create a Serilog logger configuration that writes to Seq
var seqServerUrl = Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "http://localhost:5341";
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.Seq(seqServerUrl)
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Initialize the database. and remove this to get back to ef core db
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetService<SqlDbContext>();
        var dbInitializer = services.GetService<IDbInitializer>();
        dbInitializer.Initialize(dbContext);
    }
}

app.UseCors(config => config
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Log any unhandled exceptions that occur during request processing
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>().Error;
        await Task.Run(() => Log.Error(ex, "Unhandled exception occurred during request processing"));
    });
});

app.Run();
