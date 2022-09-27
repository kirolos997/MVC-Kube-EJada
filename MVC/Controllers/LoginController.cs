using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserAccount.Models;

namespace UserAccount.Controllers
{
    public class LoginController : Controller
    {
        string BASE_URL = Environment.GetEnvironmentVariable("BACKEND_STRING_AUTH");

        public ActionResult Index()
        {
            return View();

        }

        public async Task<ActionResult> Authenticate(Employee employeeData)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(BASE_URL + "/api2/login");

                StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(employeeData), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, jsonContent);


                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index", "Login");

                }
                ResponseObj token = await response.Content.ReadAsAsync<ResponseObj>(new[] { new JsonMediaTypeFormatter() });

                HttpContext.Session.SetString("Token", token.jwt);


               var handler = new JwtSecurityTokenHandler();
                var tokenObj = handler.ReadJwtToken(token.jwt);

                var payLoad = tokenObj.Payload.FirstOrDefault().Value;

                return RedirectToAction("Index", "UserData", new { id = payLoad });

            }

        }

    }
}