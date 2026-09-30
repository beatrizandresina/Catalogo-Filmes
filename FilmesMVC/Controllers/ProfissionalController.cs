using FilmesMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesMVC.Controllers
{
    public class ProfissionalController : Controller
    {
        private readonly string apiUrl = "http://localhost:5291/api/profissional";
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            using var cliente = new HttpClient();
            var response = await cliente.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var profissionais = await response.Content.ReadFromJsonAsync<List<ProfissionalViewModel>>();
                return View(profissionais);
            }
            return View(new List<ProfissionalViewModel>());
        }
        [HttpGet]
        public IActionResult Adicionar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Adicionar(ProfissionalViewModel profissional)
        {
            using var cliente = new HttpClient();
            var response = await cliente.PostAsJsonAsync(apiUrl, profissional);
            if (response.IsSuccessStatusCode)
            {
                var profissionalCriado = await response.Content.ReadFromJsonAsync<ProfissionalViewModel>();

                if (profissional.FotoArquivo != null && profissionalCriado != null)
                {
                    using var content = new MultipartFormDataContent();
                    using var stream = profissional.FotoArquivo.OpenReadStream();

                    content.Add(new StreamContent(stream), "arquivo", profissional.FotoArquivo.FileName);

                    await cliente.PostAsync($"{apiUrl}/{profissionalCriado.Id}/foto", content);
                }
                return RedirectToAction("Index");
            }
            return View(profissional);
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.GetAsync($"api/Profissional/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var profissional = await response.Content.ReadFromJsonAsync<ProfissionalViewModel>();
                    return View(profissional);
                }
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Editar(ProfissionalViewModel profissional)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.PutAsJsonAsync($"api/profissional/{profissional.Id}", profissional);

                if (response.IsSuccessStatusCode)
                {
                    if (profissional.FotoArquivo != null && profissional.FotoArquivo.Length > 0)
                    {
                        using var content = new MultipartFormDataContent();
                        using var stream = profissional.FotoArquivo.OpenReadStream();

                        content.Add(new StreamContent(stream), "arquivo", profissional.FotoArquivo.FileName);

                        await cliente.PostAsync($"api/profissional/{profissional.Id}/foto", content);
                    }
                    return RedirectToAction("Index");
                }
                return View(profissional);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Excluir(int id)
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:5291/");
                var response = await cliente.GetAsync($"api/profissional/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var profissional = await response.Content.ReadFromJsonAsync<ProfissionalViewModel>();
                    return View(profissional);
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
                var response = await cliente.DeleteAsync($"api/profissional/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
