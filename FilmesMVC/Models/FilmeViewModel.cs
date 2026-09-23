namespace FilmesMVC.Models
{
    public class FilmeViewModel
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
        public List<GeneroViewModel>? Generos { get; set; }
        public List<AvaliacaoViewModel>? Avaliacoes { get; set; }
    }
}
