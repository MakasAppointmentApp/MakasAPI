using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForSaloon
{
    public class AppointmentsWithSaloon
    {
        public int Id { get; set; }
        public int SaloonId { get; set; }
        public int CustomerId { get; set; }
        public int WorkerId { get; set; }
        public int AppointmentId { get; set; }
        public string SaloonName { get; set; }
        public string reviewControl { get; set; }
        public string WorkerName { get; set; }
        public string ifExists { get; set; }
        public DateTime Date { get; set; }
        public double SaloonRate { get; set; }
    }
}
