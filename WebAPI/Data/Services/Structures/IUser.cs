using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.Data.Services.Structures
{
    public interface IUser
    {
        Task<User?>Authenticate(string userName, string password);
        void Register(LoginReqDto loginReq);
        Task<bool> UserAlreadyExists(string userName);
    }
}