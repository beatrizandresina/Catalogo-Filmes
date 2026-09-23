using APIFilmes.Data;
using APIFilmes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIFilmes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvaliacaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<AvaliacaoModel>> BuscarAvaliacoes()
        {
            var avaliacoes = _context.Avaliacoes.ToList();
            return Ok(avaliacoes);
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult<List<AvaliacaoModel>> BuscarAvaliacoesPorId(int id)
        {
            var avaliacao = _context.Avaliacoes.Find(id);
            if (avaliacao == null)
            {
                return NotFound("Avaliação não localizada!");
            }
            return Ok(avaliacao);
        }

        [HttpPost]
        public ActionResult<AvaliacaoModel> CriarAvaliacao(AvaliacaoModel avaliacaoModel)
        {
            if(avaliacaoModel == null)
            {
                return BadRequest("Ocorreu um erro na requisição!");
            }
            _context.Avaliacoes.Add(avaliacaoModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(BuscarAvaliacoesPorId), new { id = avaliacaoModel.Id }, avaliacaoModel);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<AvaliacaoModel> EditarAvaliacao(int id, AvaliacaoModel avaliacaoModel)
        {
            var avaliacao = _context.Avaliacoes.Find(id);
            if (avaliacao == null)
            {
                return NotFound("Avaliação não localizada!");
            }
            avaliacao.Nota = avaliacaoModel.Nota;
            avaliacao.Comentario = avaliacaoModel.Comentario;
            _context.Avaliacoes.Update(avaliacao);
            _context.SaveChanges();
            return Ok(avaliacao);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeletarAvaliacao(int id)
        {
            var avaliacao = _context.Avaliacoes.Find(id);
            if (avaliacao == null)
            {
                return NotFound("Avaliação não localizada!");
            }
            _context.Avaliacoes.Remove(avaliacao);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
