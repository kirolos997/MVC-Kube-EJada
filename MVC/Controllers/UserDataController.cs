using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using UserAccount.Models;

namespace UserAccount.Controllers
{
    public class UserDataController : Controller
    {
        string BASE_URL = Environment.GetEnvironmentVariable("BACKEND_STRING_CRUD");

        // GET: UserData
        public async Task<ActionResult> Index(string id)
        {
            var jwtToken = HttpContext.Session.GetString("Token");

            if (jwtToken == null)
            {
                return RedirectToAction("Index", "Login");

            }

            var handler = new JwtSecurityTokenHandler();
            var tokenObj = handler.ReadJwtToken(jwtToken);

            if (tokenObj == null)
            {
                return RedirectToAction("Index", "Login");

            }
            var payloadID = tokenObj.Payload.FirstOrDefault().Value.ToString();

            if (payloadID == null && payloadID == "")
            {
                return RedirectToAction("Index", "Login");

            }

            using (var client = new HttpClient())
            {
                var uri = new Uri(BASE_URL + "api1/emp/" + payloadID);
                
                HttpResponseMessage response = await client.GetAsync(uri);

                if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index", "Login");

                }
                Employee emp =  await response.Content.ReadAsAsync<Employee>(new[] { new JsonMediaTypeFormatter() });
                return View(emp);
            }

        }

        public async Task<ActionResult> Update(Employee employeeData)
        {
            var jwtToken = HttpContext.Session.GetString("Token");

            if (jwtToken == null)
            {
                return RedirectToAction("Index", "Login");

            }

            var handler = new JwtSecurityTokenHandler();
            var tokenObj = handler.ReadJwtToken(jwtToken);

            if(tokenObj == null)
            {
                return RedirectToAction("Index", "Login");

            }
            var payloadID = tokenObj.Payload.FirstOrDefault().Value.ToString();

            if(payloadID == null && payloadID == "")
            {
                return RedirectToAction("Index", "Login");

            }
            using (var client = new HttpClient())
            {

                var uri = new Uri(BASE_URL + "api1/emp/" + payloadID);

                StringContent jsonContent = new(JsonSerializer.Serialize(employeeData), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, jsonContent);

                return RedirectToAction("Index", new { id = employeeData.Id } );

            }


        }

        public async Task<ActionResult> Delete(string id )
        {
            var jwtToken = HttpContext.Session.GetString("Token");

            if (jwtToken == null)
            {
                return RedirectToAction("Index", "Login");

            }

            var handler = new JwtSecurityTokenHandler();
            var tokenObj = handler.ReadJwtToken(jwtToken);

            if (tokenObj == null)
            {
                return RedirectToAction("Index", "Login");

            }
            var payloadID = tokenObj.Payload.FirstOrDefault().Value.ToString();

            if (payloadID == null && payloadID == "")
            {
                return RedirectToAction("Index", "Login");

            }
            using (var client = new HttpClient())
            {

                var uri = new Uri(BASE_URL + "api1/emp/" + payloadID);

                HttpResponseMessage response = await client.DeleteAsync(uri);

                return RedirectToAction("Index", "Login");

            }

        }


    }
}

