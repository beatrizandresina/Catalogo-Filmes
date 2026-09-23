using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace APIFilmes.Models
{
    public class FilmeModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Diretor { get; set; }
        public int AnoLancamento { get; set; }
        public string Sinopse { get; set; }
        public int DuracaoMinutos { get; set; }
        public string ClassificacaoIndicativa { get; set; }
        public string CapaUrl { get; set; }
        public string TrailerUrl { get; set; }
        [ValidateNever]
        public List<AvaliacaoModel>? Avaliacoes { get; set; }
        [ValidateNever]
        public List<GeneroModel>? Generos { get; set; }
    }
}