using System.Text.Json.Serialization;

namespace APIFilmes.Models
{
    public class ProfissionalModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? FotoUrl { get; set; }
        [JsonIgnore]
        public ICollection<FilmeProfissionalModel>? Filmes { get; set; } = new List<FilmeProfissionalModel>();
    }
}