using MakasAPI.Helpers;
using MakasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.AuthRepositories
{
    public class SaloonAuthRepository : ISaloonAuthRepository
    {
        private DataContext _context;
        CryptographyExtension cryptography = new CryptographyExtension();
        public SaloonAuthRepository(DataContext context)
        {
            _context = context;
        }
        public Saloon GetSaloonById(int saloonId)
        {
            var user = _context.Saloons.FirstOrDefault(e => e.Id == saloonId);
            return user;
        }

        public async Task<Saloon> Login(string phone, string password)
        {
            var user = await _context.Saloons.FirstOrDefaultAsync(x => x.SaloonPhone == phone);
            if (user == null)
            {
                return null;
            }

            if (!cryptography.VerifyPasswordHash(password, user.SaloonPasswordHash, user.SaloonPasswordSalt))
            {
                return null;
            }

            return user;
        }
        

        public async Task<Saloon> Register(Saloon saloon, string password)
        {
            byte[] passwordHash, passwordSalt;
            cryptography.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            saloon.SaloonPasswordHash = passwordHash;
            saloon.SaloonPasswordSalt = passwordSalt;

            await _context.Saloons.AddAsync(saloon);
            await _context.SaveChangesAsync();
            return saloon;
        }
       

        public async Task<bool> UserExist(string phone, string email)
        {
            if (await _context.Saloons.AnyAsync(x => x.SaloonPhone == phone))
            {
                return true;
            }
            else if (await _context.Saloons.AnyAsync(x => x.SaloonEmail == email))
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
