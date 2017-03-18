using Microsoft.AspNetCore.Mvc;
using NetCore.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Hosting.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData[nameof(WebResult)] = new WebResult
                {
                    Content = "Invalid model",
                    ResultState = ResultState.Warning,
                    Validations = ModelState.AsValidationResult()
                };
                return View();
            }

            var emailModel = new RegistrationEmailViewModel
            {
                Body = "Welcome to .NET Core Wonderland",
                IsHtml = true,
                Name = model.Name,
                Subject = "Registration",
                RegistrationDate = DateTime.Now,
                To = model.EMail
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "api/mail/sendasync")
            {
                Content = new StringContent(JsonConvert.SerializeObject(emailModel), Encoding.UTF8, "application/json")
            };

            var response = await HostingFactory.HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            ViewData[nameof(WebResult)] = new WebResult
            {
                Content = "Mail has been sent.",
                ResultState = ResultState.Success
            };

            return View(new RegistrationViewModel());
        }
    }
}
