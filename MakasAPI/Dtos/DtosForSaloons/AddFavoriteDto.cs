using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Dtos.DtosForUsers
{
    public class AddFavoriteDto
    {
        public int CustomerId { get; set; }
        public int SaloonId { get; set; }
    }
}
