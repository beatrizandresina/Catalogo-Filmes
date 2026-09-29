using APIFilmes.Data;
using APIFilmes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace APIFilmes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FilmeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<FilmeModel>> BuscarFilmes()
        {
            var filmes = _context.Filmes
                .Include(f => f.Avaliacoes)
                .Include(f => f.Generos)
                .Include(f => f.Profissionais)
                .ThenInclude(fp => fp.Profissional)
                .ToList();
            return Ok(filmes);
        }
        [HttpPost("{filmeId}/generos/{generoId}")]
        public ActionResult VincularGenero(int filmeId, int generoId)
        {
            var filme = _context.Filmes
                .Include(f => f.Generos)
                .FirstOrDefault(f => f.Id == filmeId);

            if (filme == null)
                return NotFound("Filme não localizado.");

            var genero = _context.Generos.Find(generoId);

            if (genero == null)
                return NotFound("Gênero não localizado.");

            if (filme.Generos.Any(g => g.Id == generoId))
                return BadRequest("Este gênero já está vinculado a este filme.");

            filme.Generos.Add(genero);

            _context.SaveChanges();

            return Ok("Gênero vinculado ao filme com sucesso!");
        }
        [HttpPost("{filmeId}/profissionais/{profissionalId}")]
        public ActionResult VincularProfissional(int filmeId, int profissionalId, [FromQuery] string funcao)
        {
            var filme = _context.Filmes
                .Include(f => f.Profissionais)
                .FirstOrDefault(f => f.Id == filmeId);

            if (filme == null)
                return NotFound("Filme não localizado.");

            var profissional = _context.Profissionais.Find(profissionalId);

            if (profissional == null)
                return NotFound("Gênero não localizado.");

            if (filme.Profissionais.Any(p => p.ProfissionalId == profissionalId))
            {
                return BadRequest("Este profissional já está vinculado a este filme com está função.");
            }

            var vinculo = new FilmeProfissionalModel
            {
                FilmeId = filmeId,
                ProfissionalId = profissionalId,
                Funcao = funcao
            };

            filme.Profissionais.Add(vinculo);

            _context.SaveChanges();

            return Ok("Profissional vinculado com sucesso!");
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult<List<FilmeModel>> BuscarFilmesPorId(int id)
        {
            var filme = _context.Filmes
                .Include(f => f.Avaliacoes)
                .Include(f => f.Generos)
                .Include(f => f.Profissionais)
                .ThenInclude(fp => fp.Profissional)
                .FirstOrDefault(f => f.Id == id);
            if (filme == null)
            {
                return NotFound("Filme não localizado!");
            }
            return Ok(filme);
        }

        [HttpPost]
        public ActionResult<FilmeModel> CriarFilme(FilmeModel filmeModel)
        {
            if(filmeModel == null)
            {
                return BadRequest("Ocorreu um erro na requisição!");
            }

            _context.Filmes.Add(filmeModel);
            _context.SaveChanges();

            return CreatedAtAction(nameof(BuscarFilmesPorId), new { id = filmeModel.Id }, filmeModel);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<FilmeModel> EditarFilme(int id, FilmeModel filmeModel)
        {
            var filme = _context.Filmes.Find(id);

            if (filme == null)
            {
                return NotFound("Filme não localizado!");
            }

            filme.Titulo = filmeModel.Titulo;
            filme.AnoLancamento = filmeModel.AnoLancamento;
            filme.Sinopse = filmeModel.Sinopse;
            filme.DuracaoMinutos = filmeModel.DuracaoMinutos;
            filme.ClassificacaoIndicativa = filmeModel.ClassificacaoIndicativa;
            filme.CapaUrl = filmeModel.CapaUrl;
            filme.TrailerUrl = filmeModel.TrailerUrl;

            _context.Filmes.Update(filme);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult<FilmeModel> ExcluirFilme(int id)
        {
            var filme = _context.Filmes
                .Include(f => f.Generos)
                .Include(f => f.Avaliacoes)
                .Include(f => f.Profissionais)
                .FirstOrDefault(f => f.Id == id);

            if (filme == null)
            {
                return NotFound("Filme não localizado!");
            }

            if (!string.IsNullOrEmpty(filme.CapaUrl))
            {
                try
                {
                    var uri = new Uri(filme.CapaUrl);

                    var nomeArquivo = Path.GetFileName(uri.LocalPath);

                    var caminhoFisico = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "capas", nomeArquivo);

                    if (System.IO.File.Exists(caminhoFisico))
                    {
                        System.IO.File.Delete(caminhoFisico);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao excluir a imagem física: {ex.Message}");
                }
            }

            _context.Filmes.Remove(filme);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost("{id}/capa")]
        public async Task<IActionResult> UploadCapa(int id, IFormFile arquivo)
        {
            var filme = _context.Filmes.Find(id);
            if (filme == null)
            {
                return NotFound("Filme não localizado!");
            }

            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "capas");
            if (!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(arquivo.FileName);
            var caminhoCompleto = Path.Combine(pasta, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var urlImagem = $"{Request.Scheme}://{Request.Host}/capas/{nomeArquivo}";
            filme.CapaUrl = urlImagem;
            _context.SaveChanges();

            return Ok(new {url = urlImagem});
        }
    }
}
