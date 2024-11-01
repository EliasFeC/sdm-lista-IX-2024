using Microsoft.EntityFrameworkCore;
using CinemaApi.Data;
using CinemaApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CinemaApi;

public static class FilmesEndpoints
{
    public static void MapFilmeEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Filme").WithTags(nameof(Filme));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Filme.ToListAsync();
        })
        .WithName("GetAllFilmes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Filme>, NotFound>> (int id, AppDbContext db) =>
        {
            return await db.Filme.AsNoTracking()
                .FirstOrDefaultAsync(model => model.FilmeId == id)
                is Filme model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetFilmeById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Filme filme, AppDbContext db) =>
        {
            var affected = await db.Filme
                .Where(model => model.FilmeId == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.FilmeId, filme.FilmeId)
                    .SetProperty(m => m.Nome, filme.Nome)
                    .SetProperty(m => m.Genero, filme.Genero)
                    .SetProperty(m => m.Ano, filme.Ano)
                    .SetProperty(m => m.cinemaId, filme.cinemaId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateFilme")
        .WithOpenApi();

        group.MapPost("/", async (Filme filme, AppDbContext db) =>
        {
            db.Filme.Add(filme);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Filme/{filme.FilmeId}",filme);
        })
        .WithName("CreateFilme")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, AppDbContext db) =>
        {
            var affected = await db.Filme
                .Where(model => model.FilmeId == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteFilme")
        .WithOpenApi();
    }
}
