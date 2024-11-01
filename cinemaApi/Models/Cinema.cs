using System.ComponentModel.DataAnnotations;
namespace CinemaApi.Models;

public class Cinema
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Cnpj { get; set; }
    public ICollection<Filme> Filmes { get; } = new List<Filme>();
}