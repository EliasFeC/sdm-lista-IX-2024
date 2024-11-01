using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CinemaApi.Data;
using CinemaApi;

var builder = WebApplication.CreateBuilder(args);

var connectionString= builder.Configuration.GetConnectionString("Cinema") ?? "Data Source=CinemaDataBase.db";
builder.Services.AddSqlite<AppDbContext>(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCinemaEndpoints();

app.MapFilmeEndpoints();

app.Run();
