using Microsoft.EntityFrameworkCore;
using UserSignUp.Data;
using UserSignUp.Interfaces;
using UserSignUp.Models;
using UserSignUp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<SqlDbContext>(options => options.UseInMemoryDatabase("UserDatabaseInMemory"));

builder.Services.AddScoped<IRepo<User>, UserRepo>();
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
