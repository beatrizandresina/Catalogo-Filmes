using FilmesMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Json;

namespace FilmesMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(string pesquisaTitulo)
        {
            var filmes = new List<FilmeViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/");

                var response = await client.GetAsync("api/Filme");

                if (response.IsSuccessStatusCode)
                {
                    filmes = await response.Content.ReadFromJsonAsync<List<FilmeViewModel>>();
                }
            }

            if (!string.IsNullOrEmpty(pesquisaTitulo))
            {
                filmes = filmes
                    .Where(f => f.Titulo.ToLower().Contains(pesquisaTitulo.ToLower()))
                    .ToList();
            }

            return View(filmes);
        }

        [HttpGet]
        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(FilmeViewModel filme)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.PostAsJsonAsync("api/filme",filme);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(filme);
        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {
            var filme = new FilmeViewModel();

            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.GetAsync($"api/Filme/{id}");

                if (response.IsSuccessStatusCode)
                {
                    filme = await response.Content.ReadFromJsonAsync<FilmeViewModel>();
                    return View(filme);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.GetAsync($"api/Filme/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var filme = await response.Content.ReadFromJsonAsync<FilmeViewModel>();
                    return View(filme);
                }
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(FilmeViewModel filme)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.PutAsJsonAsync($"api/filme/{filme.Id}",filme);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Detalhes", new { id = filme.Id });
                }
                return View(filme);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int id)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.GetAsync($"api/filme/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var filme = await response.Content.ReadFromJsonAsync<FilmeViewModel>();
                    return View(filme);
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Excluir")]
        public async Task<IActionResult> ExcluirConfirmado(int id)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.DeleteAsync($"api/filme/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AdicionarAvaliacao(int filmeId)
        {
            var model = new AvaliacaoViewModel { FilmeId = filmeId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarAvaliacao(AvaliacaoViewModel avaliacao)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.PostAsJsonAsync("api/avaliacao",avaliacao);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Detalhes", new {id = avaliacao.FilmeId});
                }
            }
            return View(avaliacao);
        }

        [HttpGet]
        public async Task<IActionResult> VincularGenero(int filmeId)
        {
            ViewBag.FilmeId = filmeId;
            var generos = new List<GeneroViewModel>();

            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.GetAsync($"api/genero");
                if (response.IsSuccessStatusCode)
                {
                    generos = await response.Content.ReadFromJsonAsync<List<GeneroViewModel>>();
                }
            }
            return View(generos);
        }

        [HttpPost]
        public async Task<IActionResult> VincularGeneroConfirmado(int filmeId, int generoId)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.PostAsync($"api/filme/{filmeId}/generos/{generoId}",null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("detalhes", new {id = filmeId});
                }
            }
            return RedirectToAction("Index");
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
