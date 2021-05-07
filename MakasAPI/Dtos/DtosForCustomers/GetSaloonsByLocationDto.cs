using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForCustomers
{
    public class GetSaloonsByLocationDto
    {
        public int Id { get; set; }
        public string SaloonName { get; set; }
        public byte[] SaloonImage { get; set; }
        public double SaloonRate { get; set; }
        public int WorkerCount { get; set; }
    }
}
