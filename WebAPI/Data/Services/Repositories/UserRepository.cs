using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Services.Structures;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.Data.Services.Repositories
{
    public class UserRepository : IUser
    {
        private readonly DataContext daContext;

        public UserRepository(DataContext daContext)
        {
            this.daContext = daContext;
        }
        async Task<User?> IUser.Authenticate(string userName, string passwordText)
        {
            var user = await daContext.Users.FirstOrDefaultAsync(x=> x.UserName == userName);

            if(user == null)
                return null;

            if(!MatchPasswordHash(passwordText, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            byte[] passwordHash;
            using(var hmac = new HMACSHA512(passwordKey))
            {
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));
            }

            for(int ii=0; ii< passwordHash.Length; ii++)
            {
                if(passwordHash[ii] != password[ii]){

                    return false;
                }
            }

            return true;
        }

        void IUser.Register(LoginReqDto loginReq)
        {
            byte[] passwordHash, passwordKey;

            using(var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginReq.Password));
            }

            User user = new User(loginReq.UserName, passwordHash, passwordKey);
            user.email = loginReq.email;
            user.mobile = loginReq.mobile;
            daContext.Users.Add(user);
        }

        async Task<bool> IUser.UserAlreadyExists(string userName)
        {
            return await daContext.Users.AnyAsync(x=> x.UserName == userName);
        }
    }
}