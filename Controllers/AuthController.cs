using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using chat_api.Models;
using chat_api.Services;
using chat_api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace chat_api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/auth")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        private string GetIdentity(LoginViewModel creds)
        {
            var users = _userService.Get();
            
            var result = users.FirstOrDefault(user => user.Login == creds.login && user.Password == creds.password);

            if (result == null)
                return null;
            
            var now = DateTime.Now;

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, result.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "user"),
                new Claim("Id", result.Id),
                new Claim("Name", result.DisplayName?? result.Login)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }
        
        [Route("login")]
        [HttpPost]
        public ActionResult<string> Login(LoginViewModel loginViewModel)
        {
            var token = GetIdentity(loginViewModel);
            
            if (token == null)
            {
                return new NotFoundResult();
            }

            
            HttpContext.Response.Cookies.Append(
                ".AspNetCore.Application.Id",
                token
            );
            
            return Ok(token);
        }
        
        [Route("me")]
        [Authorize]
        [HttpGet]
        public ActionResult<User> Me()
        {
            var user = User.Identity?.Name;

            return Ok(user);
        }

        [Route("logout")]
        [HttpPost]
        public ActionResult Logout()
        {
            HttpContext.Response.Cookies.Append(
                ".AspNetCore.Application.Id",
                "",
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(-1)
                }
            );

            return Ok();
        }
    }
}