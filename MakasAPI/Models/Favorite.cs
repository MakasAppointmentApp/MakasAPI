using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int SaloonId { get; set; }
    }
}
