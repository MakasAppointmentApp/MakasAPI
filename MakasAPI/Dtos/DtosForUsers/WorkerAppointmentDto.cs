using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForSaloon
{
    public class WorkerAppointmentDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CustomerFullName { get; set; }

    }
}
