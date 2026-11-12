
using API.Interfaces;
using API.Repositories;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// nämä kolme liitännäistä tarvitaan siihen, että swagger / OpenAPI-dokumentaatio toimii
// automaattisesti
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUsersRepository, UsersSQLiteRepository>();

builder.Services.AddScoped<SqliteConnection>(provider =>
{
    // luodaan connection instanssi niin kuin konstruktorissakin
    var connection = new SqliteConnection("Data Source=tuntiharjoitus1.db");
    // avataan ja palautetaan se
    connection.Open();
    return connection;
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();