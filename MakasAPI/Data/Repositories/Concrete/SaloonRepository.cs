using MakasAPI.Data.Repositories.Abstract;
using MakasAPI.Dtos.DtosForSaloon;
using MakasAPI.Helpers;
using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.Repositories.Concrete
{
    public class SaloonRepository : ISaloonRepository
    {
        private DataContext _context;

        public SaloonRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
        public List<Saloon> GetSaloons()
        {
            var saloons = _context.Saloons.ToList();
            if (saloons != null)
            {
                saloons.OrderBy(s => s.SaloonRate);
                saloons.Reverse();
                return saloons;
            }
            return null;
        }

        public Saloon GetSaloonById(int saloonId)
        {
            //Entity Frameworkten FindAsync ya da FirstOrDefaultAsync kullanmak daha iyi olabilir mi?
            var saloon = _context.Saloons.FirstOrDefault(s => s.Id == saloonId);
            if (saloon != null)
            {
                return saloon;

            }
            return null;
        }
        public List<Saloon> GetSaloonsByLocation(string city, string district)
        {
            var saloons = _context.Saloons.Where(s => s.SaloonCity == city && s.SaloonDistrict == district).ToList();
            if (saloons !=null)
            {
                saloons.OrderBy(s => s.SaloonRate);//Bunu buradan kaldırıp farklı filterlar için sorgular yazabiliriz.
                return saloons;
            }
            return null;

        }

        public async Task<Saloon> UpdateSaloonLocation(int id, string saloonLocation)
        {
            var saloon = GetSaloonById(id);
            if (saloon != null)
            {
                saloon.SaloonLocation = saloonLocation;
                _context.Saloons.Update(saloon);
                await _context.SaveChangesAsync();
                return saloon;
            }
            return null;
        }

        public async Task<Saloon> UpdateSaloonImage(int id, byte[] image)
        {
            var saloon = GetSaloonById(id);
            if (saloon != null)
            {
                saloon.SaloonImage = image;
                _context.Saloons.Update(saloon);
                await _context.SaveChangesAsync();
                return saloon;
            }
            return null;
        }

        public async Task<Saloon> UpdateSaloonName(int id, string saloonName)
        {
            var saloon = GetSaloonById(id);
            if (saloon != null)
            {
                saloon.SaloonName = saloonName;
                _context.Saloons.Update(saloon);
                await _context.SaveChangesAsync();
                return saloon;
            }
            return null;
           
        }

        public async Task<Saloon> UpdatePassword(int id, string oldPassword, string newPassword)
        {
            var saloon = GetSaloonById(id);
            CryptographyExtension cryptography = new CryptographyExtension();
            if (cryptography.VerifyPasswordHash(oldPassword, saloon.SaloonPasswordHash, saloon.SaloonPasswordSalt))
            {
                //if burada girilen şifre eski şifre ile aynı mı diye kontrol ediyor
                if (!cryptography.VerifyPasswordHash(newPassword, saloon.SaloonPasswordHash, saloon.SaloonPasswordSalt))
                {
                    byte[] passwordHash, passwordSalt;
                    cryptography.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
                    saloon.SaloonPasswordHash = passwordHash;
                    saloon.SaloonPasswordSalt = passwordSalt;
                    _context.Saloons.Update(saloon);
                    await _context.SaveChangesAsync();
                    return saloon;
                }
            }
            return null;
        }
    }
}
