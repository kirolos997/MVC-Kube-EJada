using AUTH_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace AUTH_Service.Controllers
{
    [ApiController]
    [Route("api2")]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataBaseContext _db;

        public AuthenticationController(DataBaseContext db)
        {
            _db = db;
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] Request req)
        {
            string email = req.Email;
            string password = req.Password;
            Employee emp = _db.Employees.Where(m => m.Email == email && m.Password == password).FirstOrDefault<Employee>();

            if (emp == null)
            {
                return NoContent();
            }
            // Define const Key this should be private secret key  stored in some safe place
            string key = System.Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            // Create Security key  using private key above:
            // not that latest version of JWT using Microsoft namespace instead of System
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            // Also note that securityKey length should be >256b
            // so you have to make sure that your private key has a proper length
            //
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //  Finally create a Token
            var header = new JwtHeader(credentials);

            //Some PayLoad that contain information about the  customer
            var payload = new JwtPayload { { "id ", emp.Id } };

            //
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            ResponseObj res = new ResponseObj();
            res.jwt = handler.WriteToken(secToken);

            return Ok(res);
        }

    }
}
