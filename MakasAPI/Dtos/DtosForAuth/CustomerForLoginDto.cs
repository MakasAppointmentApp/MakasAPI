using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForAuth
{
    public class CustomerForLoginDto
    {
        public string CustomerEmail { get; set; }
        public string CustomerPassword { get; set; }
    }
}
