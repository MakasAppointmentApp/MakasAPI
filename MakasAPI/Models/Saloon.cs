using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Models
{
    public class Saloon
    {
        public int Id { get; set; }
        public string SaloonName { get; set; }
        public string SaloonEmail { get; set; }
        public string SaloonPhone { get; set; }
        public bool SaloonGender { get; set; }
        public string SaloonCity { get; set; }
        public string SaloonDistrict { get; set; }
        public byte[] SaloonPasswordHash { get; set; }
        public byte[] SaloonPasswordSalt { get; set; }
        public string SaloonLocation { get; set; }
        public byte[] SaloonImage { get; set; }
        public double SaloonRate { get; set; }

    }
}
