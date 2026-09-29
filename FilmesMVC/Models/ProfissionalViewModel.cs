namespace FilmesMVC.Models
{
    public class ProfissionalViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string FotoUrl { get; set; }
        public List<FilmeProfissionalViewModel>? Filmes { get; set; } = new List<FilmeProfissionalViewModel>();
    }
}