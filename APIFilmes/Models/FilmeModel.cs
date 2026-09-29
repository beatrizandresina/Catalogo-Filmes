using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace APIFilmes.Models
{
    public class FilmeModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int AnoLancamento { get; set; }
        public string? Sinopse { get; set; }
        public int DuracaoMinutos { get; set; }
        public string? ClassificacaoIndicativa { get; set; }
        public string? CapaUrl { get; set; }
        public string? TrailerUrl { get; set; }
        [ValidateNever]
        public ICollection<FilmeProfissionalModel> Profissionais { get; set; } = new List<FilmeProfissionalModel>();
        [ValidateNever]
        public ICollection<AvaliacaoModel> Avaliacoes { get; set; } = new List<AvaliacaoModel>();
        [ValidateNever]
        public ICollection<GeneroModel> Generos { get; set; } = new List<GeneroModel>();
    }
}