using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Models
{
    public class Worker
    {
        public Worker()
        {
            Reviews = new List<Review>();
            Appointments = new List<Appointment>();
        }
        public int Id { get; set; }
        public int SaloonId { get; set; }
        public string WorkerName { get; set; }
        public byte[] WorkerPhoto { get; set; }
        public double WorkerRate { get; set; }
        public Saloon Saloon { get; set; }

        public List<Review> Reviews { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
