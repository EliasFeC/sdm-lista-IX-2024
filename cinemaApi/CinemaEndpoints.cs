using Microsoft.EntityFrameworkCore;
using CinemaApi.Data;
using CinemaApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CinemaApi;

public static class CinemaEndpoints
{
    public static void MapCinemaEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cinema").WithTags(nameof(Cinema));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Cinema.ToListAsync();
        })
        .WithName("GetAllCinemas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cinema>, NotFound>> (int id, AppDbContext db) =>
        {
            return await db.Cinema.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Cinema model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCinemaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Cinema Cinema, AppDbContext db) =>
        {
            var affected = await db.Cinema
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, Cinema.Id)
                    .SetProperty(m => m.Nome, Cinema.Nome)
                    .SetProperty(m => m.Cnpj, Cinema.Cnpj)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCinema")
        .WithOpenApi();

        group.MapPost("/", async (Cinema cinema, AppDbContext db) =>
        {
            db.Cinema.Add(cinema);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cinema/{cinema.Id}",cinema);
        })
        .WithName("CreateCinema")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, AppDbContext db) =>
        {
            var affected = await db.Cinema
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCinema")
        .WithOpenApi();
    }
}
