using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForSaloon
{
    public class WorkersBySaloonDto
    {
        public int Id { get; set; }
        public string WorkerName { get; set; }
        public byte[] WorkerPhoto { get; set; }
        public double WorkerRate { get; set; }
    }
}
