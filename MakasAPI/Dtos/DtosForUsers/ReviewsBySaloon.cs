using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForSaloon
{
    public class ReviewsBySaloon
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public double Rate { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
