using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chat_api.Models;
using chat_api.Services;
using chat_api.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;

namespace chat_api.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get() => _userService.Get();

        [HttpGet("id")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return new NotFoundResult();
            }

            return user;
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            var newUser = _userService.Create(user);

            return new CreatedResult("GetUser", newUser);
        }
        
        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            var users = _userService.Get();
            
            var result = users.FirstOrDefault(user => user.Login == loginViewModel.login && user.Password == loginViewModel.password);

            if (result == null)
            {
                return new NotFoundResult();
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
                new Claim("Id", result.Id),
                new Claim("Login", result.Login),
            };

            var jwt = new JwtSecurityToken(
                issuer: "JWPAPI",
                audience: "SampleAudience",
                claims,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(2880))
            );
            
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            HttpContext.Response.Cookies.Append(
                ".AspNetCore.Application.Id",
                token,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(2880)
                }
            );
            
            return new OkResult();
        }
        
        [Route("Me")]
        [HttpGet]
        public ActionResult<User> Me()
        {
            var user = User.Identity;

            return Ok(user);
        }
    }
}