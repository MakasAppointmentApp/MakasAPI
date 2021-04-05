using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.AuthRepositories
{
    public interface ISaloonAuthRepository
    {
        Task<Saloon> Register(Saloon saloon, string password);
        Task<Saloon> Login(string phone, string password);
        Task<bool> UserExist(string phone, string email);

        Saloon GetSaloonById(int saloonId);
    }
}
