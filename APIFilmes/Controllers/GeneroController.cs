using APIFilmes.Data;
using APIFilmes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIFilmes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GeneroController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<GeneroModel>> BuscarGeneros()
        {
            var generos = _context.Generos.ToList();
            return Ok(generos);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<GeneroModel> BuscarGeneroPorId(int id)
        {
            var genero = _context.Generos.Find(id);
            if (genero == null)
            {
                return NotFound("Gênero não localizado!");
            }
            return Ok(genero);
        }

        [HttpPost]
        public ActionResult<GeneroModel> CriarGenero(GeneroModel generoModel)
        {
            if (generoModel == null)
            {
                return BadRequest("Ocorreu um erro na requisição!");
            }
            _context.Generos.Add(generoModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(BuscarGeneros), new { id = generoModel.Id }, generoModel);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<GeneroModel> EditarGenero(int id, GeneroModel generoModel)
        {
            var genero = _context.Generos.Find(id);
            if (genero == null)
            {
                return NotFound("Gênero não localizado!");
            }
            genero.Nome = generoModel.Nome;
            _context.Generos.Update(genero);
            _context.SaveChanges();
            return Ok(genero);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeletarGenero(int id)
        {
            var genero = _context.Generos.Find(id);
            if (genero == null)
            {
                return NotFound("Gênero não localizado!");
            }
            _context.Generos.Remove(genero);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
