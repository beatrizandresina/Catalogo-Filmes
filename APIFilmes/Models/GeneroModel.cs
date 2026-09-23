using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace APIFilmes.Models
{
    public class GeneroModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public List<FilmeModel>? Filmes { get; set; }
    }
}
