using System.Text.Json.Serialization;

namespace APIFilmes.Models
{
    public class FilmeProfissionalModel
    {
        public int FilmeId { get; set; }
        [JsonIgnore]
        public FilmeModel Filme { get; set; }
        public int ProfissionalId { get; set; }
        public ProfissionalModel Profissional { get; set; }
        public string Funcao { get; set; } = string.Empty;
    }
}
