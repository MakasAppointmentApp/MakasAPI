using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForUsers
{
    public class UpdateCustomerNameDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurName { get; set; }
    }
}
