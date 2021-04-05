using MakasAPI.Data.Repositories.Abstract;
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
            saloons.OrderBy(s => s.SaloonRate);
            saloons.Reverse();
            return saloons;
        }
        public Saloon GetSaloonById(int saloonId)
        {
            var saloon = _context.Saloons.FirstOrDefault(s => s.Id == saloonId);
            return saloon;
        }
        public List<Saloon> GetSaloonsByLocation(string city, string district)
        {
            var saloons = _context.Saloons.Where(s => s.SaloonCity == city && s.SaloonDistrict == district).ToList();
            saloons.OrderBy(s => s.SaloonRate);//Bunu buradan kaldırıp farklı filterlar için sorgular yazabiliriz.
            return saloons;
        }

    }
}
