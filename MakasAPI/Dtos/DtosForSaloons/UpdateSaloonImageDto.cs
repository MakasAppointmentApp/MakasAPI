using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForUsers
{
    public class UpdateSaloonImageDto
    {
        public int Id { get; set; }
        public byte[] SaloonImage { get; set; }

    }
}
