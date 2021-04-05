using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.Repositories.Abstract
{
    public interface ISaloonRepository: IAppRepository
    {
        List<Saloon> GetSaloons();
        Saloon GetSaloonById(int saloonId);
        List<Saloon> GetSaloonsByLocation(string city, string district);
    }
}
