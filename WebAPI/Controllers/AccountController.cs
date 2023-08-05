using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Data.Services.Structures;
using WebAPI.Dtos;
using WebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Errors;

namespace WebAPI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IHost host;
        private readonly IConfiguration config;

        public AccountController(IUnitOfWork uow, IHost ihost, IConfiguration config)
        {
            this.uow = uow;
            this.host = ihost;
            this.config = host.Services.GetRequiredService<IConfiguration>();
        }

        //api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDto loginReq)
        {
            var user = await uow.IUserRepo.Authenticate(loginReq.UserName, loginReq.Password);

            if (user == null)
            {
                ApiError apiError = new ApiError(
                    Unauthorized().StatusCode,
                    "Invalid Id or Password",
                    "This error apean when provided user id or password is invalid"
                );
                return Unauthorized(apiError);

            }

            var loginRes = new LoginResDto();
            loginRes.UserName = user.UserName;
            loginRes.Token = CreateJWT(user);

            return Ok(loginRes);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginReqDto loginReq)
        {
            ApiError apiError = new ApiError(null);

            if (loginReq.UserName.Trim().IsNullOrEmpty() ||
                loginReq.Password.Trim().IsNullOrEmpty())
            {

                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User name or password cannot be blank";

                return BadRequest(apiError);
            }

            if (await uow.IUserRepo.UserAlreadyExists(loginReq.UserName))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User already exists, please try different user name";

                return BadRequest(apiError);
            }

            uow.IUserRepo.Register(loginReq);
            await uow.SaveAsync();

            return StatusCode(201);
        }
        private string CreateJWT(User user)
        {
            var secretKey = config.GetValue<string>("AppSettings:Key");
            Console.WriteLine(secretKey);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            var claims = new Claim[]{
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

    }
}