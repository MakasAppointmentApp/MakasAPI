using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForUsers
{
    public class AddWorkerDto
    {
        public int SaloonId { get; set; }
        public string WorkerName { get; set; }
        public byte[] WorkerPhoto { get; set; }
    }
}
