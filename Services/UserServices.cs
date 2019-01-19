﻿using DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modules;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserServices
    {
        private readonly SportContext _context;
        private readonly IConfiguration _configuration;

        public UserServices(SportContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        public Task<string> Authenticate(string login, string password)
        {
            var user = GetUser(login, password);

            if (user == null)
                return Task.FromResult<string>(null);

            //создание токена
            // authentication successful so generate jwt token
            //генерирует токены
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var issuer = _configuration["JwtSettings:Issuer"];
            var consumer = _configuration["JwtSettings:Consumer"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Issuer = issuer,
                Audience = consumer,
                Expires = DateTime.UtcNow.AddYears(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        private User GetUser(string login, string password)
        {
            //var user = _context.Users.SingleOrDefault(searchingUser => searchingUser.Login == login && searchingUser.Password == password);

            //if (user == null)
            //{
            //    return null;
            //}
            //else
            //{
            //    return user;
            //}
            var users = new List<User>
            {
                new User
                {
                    Id=1,
                    Login="CrazyMax",
                    Password="12345",
                    Email="crazymax@mail.com"
                }
            };

            return users.SingleOrDefault(searchingUser => searchingUser.Login == login && searchingUser.Password == password);
        }
    }
}
