namespace FilmesMVC.Models
{
    public class FilmeProfissionalViewModel
    {
        public int FilmeId { get; set; }
        public FilmeViewModel? Filme { get; set; }
        public int ProfissionalId { get; set; }
        public ProfissionalViewModel Profissional { get; set; }
        public string Funcao { get; set; } = string.Empty;
    }
}