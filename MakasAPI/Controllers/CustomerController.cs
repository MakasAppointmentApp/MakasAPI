using MakasAPI.Data.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        private ICustomerRepository _customerRepository;
        private IConfiguration _configuration;
        public CustomerController(ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("allsaloons")]
        public ActionResult GetSaloons()
        {
            var saloons = _customerRepository.GetSaloons();
            if (saloons == null)
            {
                return BadRequest("Sistemde hiç salon yok!");
            }
            return Ok(saloons);
        }
        [HttpGet]
        [Route("locationsaloons")]
        public ActionResult GetSaloonsByLocation(string city, string district)
        {
            var saloons = _customerRepository.GetSaloonsByLocation(city,district);
            if (saloons == null)
            {
                return BadRequest("Bu konumda hiç salon yok!");
            }
            return Ok(saloons);
        }

        [HttpGet]
        [Route("saloondetail")]
        public ActionResult GetSaloonById(int id)
        {
            var saloon = _customerRepository.GetSaloonById(id);
            if (saloon == null)
            {
                return BadRequest("Böyle bir salon yok!");
            }
            return Ok(saloon);
        }
    }
}
