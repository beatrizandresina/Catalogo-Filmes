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
    }
}
