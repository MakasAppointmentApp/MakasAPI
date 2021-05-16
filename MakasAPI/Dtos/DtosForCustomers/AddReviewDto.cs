using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForCustomers
{
    public class AddReviewDto
    {
        public int CustomerId { get; set; }
        public int SaloonId { get; set; }
        public int WorkerId { get; set; }
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public double Rate { get; set; }
        public string Comment { get; set; }
    }
}
