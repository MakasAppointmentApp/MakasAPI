using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public int SaloonId { get; set; }
        public string WorkerName { get; set; }
        public byte[] WorkerImage { get; set; }
        public double WorkerRate { get; set; }
    }
}
