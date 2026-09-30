using FilmesMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesMVC.Controllers
{
    public class GeneroController : Controller
    {
        private readonly string apiUrl = "http://localhost:5291/api/genero";
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            using var cliente = new HttpClient();
            var response = await cliente.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var generos = await response.Content.ReadFromJsonAsync<List<GeneroViewModel>>();
                return View(generos);
            }
            return View(new List<GeneroViewModel>());
        }
        [HttpGet]
        public IActionResult Adicionar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Adicionar(GeneroViewModel genero)
        {
            using var cliente = new HttpClient();
            var response = await cliente.PostAsJsonAsync(apiUrl, genero);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(genero);
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.GetAsync($"api/Genero/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var genero = await response.Content.ReadFromJsonAsync<GeneroViewModel>();
                    return View(genero);
                }
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Editar(GeneroViewModel genero)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.PutAsJsonAsync($"api/genero/{genero.Id}", genero);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View(genero);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Excluir(int id)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.GetAsync($"api/genero/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var genero = await response.Content.ReadFromJsonAsync<GeneroViewModel>();
                    return View(genero);
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
                var response = await cliente.DeleteAsync($"api/genero/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
