using System;
using System.Collections.Generic;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BilmemYol.Data.Models;
using BilmemYol.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BilmemYol.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _Configuration;
        private BilmemYolContext _context;
        public LoginController(IConfiguration Configuration, BilmemYolContext context)
        {
            _Configuration = Configuration;
            _context = context;
        }
        [HttpPost("Login")]
        public ApiResponse Login([FromBody]LoginRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Kullanıcı var mı?
                    //Varsa token üret gönder.
                    var user = _context.User.FirstOrDefault(u => u.Email == request.UserName && u.Password == request.Password);
                    if (user == null)
                    {
                        return new ApiResponse { Code = 404, Message = "User not exists", Set = null };
                    }

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, request.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["SigninKey"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var tokenHandler = new JwtSecurityToken(
                        issuer: _Configuration["Issuer"],
                            audience: _Configuration["Audience"],
                            claims: claims,
                            expires: DateTime.UtcNow.AddDays(30),
                            notBefore:DateTime.UtcNow,
                            signingCredentials: credentials

                        );
                    IdentityModelEventSource.ShowPII = true;
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenHandler);

                    return new ApiResponse { Code = 200, Message = "Success", Set = token };
                }
                else
                {
                    return new ApiResponse { Code = 401, Message = "Model is not valid", Set = null };
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse { Code = 500, Message = "Internal Server Error", Set = ex.StackTrace };
            }
        }
        
    }
   
}
