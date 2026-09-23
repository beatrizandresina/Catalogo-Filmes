using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace APIFilmes.Models
{
    public class AvaliacaoModel
    {
        public int Id { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public int FilmeId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public FilmeModel? Filme { get; set; }
    }
}
