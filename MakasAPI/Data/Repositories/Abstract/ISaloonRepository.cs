using MakasAPI.Dtos.DtosForSaloon;
using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.Repositories.Abstract
{
    public interface ISaloonRepository
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool SaveAll();
        Task<Saloon> UpdateSaloonLocation(int id, string saloonLocation);
        Task<Saloon> UpdateSaloonImage(int id, byte[] image);
        Task<Saloon> UpdateSaloonName(int id, string saloonName);
        Task<Saloon> UpdatePassword(int id, string oldPassword, string newPassword);
        List<Saloon> GetSaloons();
        Saloon GetSaloonById(int saloonId);
        List<Saloon> GetSaloonsByLocation(string city, string district);
    }
}
