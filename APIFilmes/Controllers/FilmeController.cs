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
        [HttpGet]
        [Route("{id}")]
        public ActionResult<List<FilmeModel>> BuscarFilmesPorId(int id)
        {
            var filme = _context.Filmes
                .Include(f => f.Avaliacoes)
                .Include(f => f.Generos)
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
            filme.Diretor = filmeModel.Diretor;
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
                .FirstOrDefault(f => f.Id == id);

            if (filme == null)
            {
                return NotFound("Filme não localizado!");
            }

            _context.Filmes.Remove(filme);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
