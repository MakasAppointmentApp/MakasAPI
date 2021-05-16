using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForUsers
{
    public class AddPriceDto
    {
        public int SaloonId { get; set; }
        public string PriceName { get; set; }
        public double PriceAmount { get; set; }
    }
}
