using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Models
{
    public class Price
    {
        public int Id { get; set; }
        public int SaloonId { get; set; }
        public string PriceName { get; set; }
        public double PriceAmount { get; set; }
    }
}
