using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForSaloon
{
    public class SaloonsByLocationDto
    {
        public int Id { get; set; }
        public string SaloonName { get; set; }
        public byte[] SaloonImage { get; set; }
        public double SaloonRate { get; set; }
        public int WorkerCount { get; set; }
    }
}
