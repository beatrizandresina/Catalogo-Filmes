using APIFilmes.Data;
using APIFilmes.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIFilmes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfissionalController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProfissionalController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<ProfissionalModel>> ListarProfissionais()
        {
            return _context.Profissionais.ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<ProfissionalModel> ObterProfissionalPorId(int id)
        {
            var profissional = _context.Profissionais.Find(id);
            if (profissional == null)
            {
                return NotFound("Profissional não encontrado");
            }
            return profissional;
        }
        [HttpPost]
        public ActionResult<ProfissionalModel> CriarProfissional(ProfissionalModel profissional)
        {
            _context.Profissionais.Add(profissional);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterProfissionalPorId), new { id = profissional.Id }, profissional);
        }
        [HttpPut("{id}")]
        public ActionResult EditarProfissional(int id, ProfissionalModel profissionalmodel)
        {
            var profissionalExistente = _context.Profissionais.Find(id);
            if (profissionalExistente == null)
            {
                return NotFound("Profissional não encontrado");
            }
            profissionalExistente.Nome = profissionalmodel.Nome;
            profissionalExistente.FotoUrl = profissionalmodel.FotoUrl;
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult ExcluirProfissional(int id)
        {
            var profissional = _context.Profissionais.Find(id);
            if (profissional == null)
            {
                return NotFound("Profissional não encontrado");
            }
            if (!string.IsNullOrEmpty(profissional.FotoUrl))
            {
                try
                {
                    var uri = new Uri(profissional.FotoUrl);
                    var nomeArquivo = Path.GetFileName(uri.LocalPath);

                    var caminhoFisico = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fotosProfissionais", nomeArquivo);

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
            _context.Profissionais.Remove(profissional);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPost("{id}/foto")]
        public async Task<IActionResult> UploadFoto(int id, IFormFile arquivo)
        {
            var profissional = _context.Profissionais.Find(id);
            if (profissional == null)
            {
                return NotFound("Profissional não localizado!");
            }

            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fotosProfissionais");
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

            var urlImagem = $"{Request.Scheme}://{Request.Host}/fotosProfissionais/{nomeArquivo}";
            profissional.FotoUrl = urlImagem;
            _context.SaveChanges();

            return Ok(new { url = urlImagem });
        }
    }
}
