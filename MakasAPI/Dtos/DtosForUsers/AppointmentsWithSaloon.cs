using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForSaloon
{
    public class AppointmentsWithSaloon
    {
        public int Id { get; set; }
        public string SaloonName { get; set; }
        public DateTime Date { get; set; }
        public double SaloonRate { get; set; }
    }
}
