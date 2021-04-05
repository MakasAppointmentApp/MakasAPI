using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.AuthRepositories
{
    public interface ICustomerAuthRepository
    {
        Task<Customer> Register(Customer customer, string password);
        Task<Customer> Login(string email, string password);
        Task<bool> UserExist(string email);

        Customer GetCustomerById(int customerId);
    }
}
