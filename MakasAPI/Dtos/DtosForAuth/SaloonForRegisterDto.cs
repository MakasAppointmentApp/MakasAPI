using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForAuth
{
    public class SaloonForRegisterDto
    {
        public string SaloonName { get; set; }
        public string SaloonEmail { get; set; }
        public string SaloonPhone { get; set; }
        public bool SaloonGender { get; set; }
        public string SaloonCity { get; set; }
        public string SaloonDistrict { get; set; }
        public string SaloonPassword { get; set; }

    }
}
