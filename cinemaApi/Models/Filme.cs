using System.Text.Json.Serialization;

namespace CinemaApi.Models;

public class Filme
{
    public int FilmeId { get; set; }
    public string? Nome { get; set; }
    public string? Genero { get; set; }
    public int Ano { get; set; }
    public int cinemaId { get; set; }
    [JsonIgnore]
    public Cinema Cinema {get; set;} = null!;
}