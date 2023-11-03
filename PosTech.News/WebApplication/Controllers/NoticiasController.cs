using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using News.WebApplication.Models;
using News.WebApplication.Support;
using System.Net.Http.Headers;
using System.Net.Http;
using News.Domain.Entities;
using News.WebApplication.Extensions;

namespace News.WebApplication.Controllers
{
    public class NoticiasController : Controller
    {
        private readonly ApiSettings _apiSettings;

        public NoticiasController(IOptions<ApiSettings> options)
        {
            _apiSettings = options.Value;
        }

        // GET: NoticiaViewModels
        public IActionResult Index()
        {
            IEnumerable<NoticiaViewModel> noticias;
            using (var client = new HttpClient())
            {
                AddTokenToClient(client);
                client.BaseAddress = new Uri(_apiSettings.Uri);
                //HTTP GET
                var result = client.GetAsync("Noticias").Result;

                if (result.IsSuccessStatusCode)
                {
                    noticias = result.Content.ReadAsAsync<IList<NoticiaViewModel>>().Result;
                }
                else
                {
                    noticias = Enumerable.Empty<NoticiaViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no servidor. Contate o Administrador.");
                }
                return View(noticias);
            }
        }

        [HttpGet]
        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult create(NoticiaViewModel noticia)
        {
            if (noticia == null)
                return new BadRequestResult();

            using (var client = new HttpClient())
            {
                AddTokenToClient(client);
                client.BaseAddress = new Uri(_apiSettings.Uri);
                var result = client.PostAsJsonAsync<NoticiaViewModel>("Noticias", noticia).Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Erro no Servidor. Contacte o Administrador.");

            return View(noticia);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            NoticiaViewModel noticia = null;

            using (var client = new HttpClient())
            {
                AddTokenToClient(client);
                client.BaseAddress = new Uri(_apiSettings.Uri);
                var result = client.GetAsync("Noticias/" + id.ToString()).Result;

                if (result.IsSuccessStatusCode)
                {
                    noticia = result.Content.ReadAsAsync<NoticiaViewModel>().Result;
                }
            }

            return View(noticia);
        }

        [HttpPost]
        public IActionResult Edit(NoticiaViewModel noticia)
        {
            if (noticia == null)
                return new BadRequestResult();

            using (var client = new HttpClient())
            {
                AddTokenToClient(client);
                client.BaseAddress = new Uri(_apiSettings.Uri);

                var result = client.PutAsJsonAsync<NoticiaViewModel>("Noticias/" + noticia.Id, noticia).Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(noticia);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            NoticiaViewModel noticia = null;

            using (var client = new HttpClient())
            {
                AddTokenToClient(client);
                client.BaseAddress = new Uri(_apiSettings.Uri);
                var result = client.GetAsync("Noticias/" + id.ToString()).Result;

                if (result.IsSuccessStatusCode)
                {
                    noticia = result.Content.ReadAsAsync<NoticiaViewModel>().Result;
                }
            }
            return View(noticia);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return new BadRequestResult();

            using (var client = new HttpClient())
            {
                AddTokenToClient(client);
                client.BaseAddress = new Uri(_apiSettings.Uri);

                var result = client.DeleteAsync("Noticias/" + id.ToString()).Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        private void AddTokenToClient(HttpClient client)
        {
            string accessToken = "";

            using (var clientLogin = new HttpClient())
            {
                clientLogin.BaseAddress = new Uri(_apiSettings.Uri);

                var objectRequest = new { Username = _apiSettings.Username, Password = _apiSettings.Password };
                var result = clientLogin.PostAsJsonAsync("Login", objectRequest).Result;

                if (result.IsSuccessStatusCode)
                {
                    var token = result.Content.ReadFromJsonAsync<Token>().Result;
                    accessToken = token?.AccessToken ?? "";
                }
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}