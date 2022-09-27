using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserAccount.Models;

namespace UserAccount.Controllers
{
    public class NewAccountController : Controller
    {
        string BASE_URL = Environment.GetEnvironmentVariable("BACKEND_STRING_CRUD");


        // GET: UserData
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Cancel()
        {
            return RedirectToAction("Index", "Login");
        }
        // POST: HomeControlhhler1/Create
        [HttpPost]
        public async Task<ActionResult> Create(Employee employeeData)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(BASE_URL + "api1/emp");

                StringContent jsonContent = new(JsonSerializer.Serialize(employeeData),Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri,jsonContent);

            }

            return RedirectToAction("Index", "Login");

        }


    }
}
