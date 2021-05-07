using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForCustomers
{
    public class ListedSaloonLocationDto
    {
        public string SaloonCity { get; set; }
        public string SaloonDistrict { get; set; }
        public bool SaloonGender { get; set; }
    }
}
