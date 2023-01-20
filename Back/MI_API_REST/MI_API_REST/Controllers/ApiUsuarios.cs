using Microsoft.AspNetCore.Mvc;
using MI_API_REST.Entities;
using MI_API_REST.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MI_API_REST.Controllers
{
    /// <summary>
    /// Controlador para obtener token de seguridad.
    /// </summary>
    [Route("api/Token")]
    [ApiController]
    public class ApiUsuarios : ControllerBase
    {
        public IConfiguration _configuration;
        public ApiUsuarios(IConfiguration cofig)
        {
            _configuration = cofig;
        }

        [HttpPost]
        public IActionResult LoginApi(ModUsuariosApi modUsuarios)
        {
            RepUsuariosApi repUsuarios = new();
            bool usuario = repUsuarios.VerificarUsuario(modUsuarios.Usuario, modUsuarios.Pass);
            if (usuario) 
            {
                Claim[] claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("usuario", modUsuarios.Usuario)
                };

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddSeconds(10),
                    signingCredentials: signIn);

                //ModTokenResponse response = new ModTokenResponse();
                //response.TokenResponse = Convert.ToString(new JwtSecurityTokenHandler().WriteToken(token));

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    
            }
            else
            {
                return BadRequest("Usuario o contraseña invalido.");
            }
        }
    }
}
