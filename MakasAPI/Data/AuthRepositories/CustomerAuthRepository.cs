using MakasAPI.Helpers;
using MakasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.AuthRepositories
{
    public class CustomerAuthRepository : ICustomerAuthRepository
    {
        private DataContext _context;
        CryptographyExtension cryptography = new CryptographyExtension();

        public CustomerAuthRepository(DataContext context)
        {
            _context = context;
        }
        public Customer GetCustomerById(int customerId)
        {
            var user = _context.Customers.FirstOrDefault(e => e.Id == customerId);
            return user;
        }

        public async Task<Customer> Login(string email, string password)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerEmail == email);
            if (user == null)
            {
                return null;
            }

            if (!cryptography.VerifyPasswordHash(password, user.CustomerPasswordHash, user.CustomerPasswordSalt))
            {
                return null;
            }

            return user;
        }
        public async Task<Customer> Register(Customer customer, string password)
        {
            byte[] passwordHash, passwordSalt;
            cryptography.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            customer.CustomerPasswordHash = passwordHash;
            customer.CustomerPasswordSalt = passwordSalt;

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<bool> UserExist(string email)
        {

            if (await _context.Customers.AnyAsync(x => x.CustomerEmail == email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
