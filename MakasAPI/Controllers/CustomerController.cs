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

        [HttpGet]
        [Route("workersinsaloon")]
        public ActionResult GetWorkersBySaloon(int saloonId)
        {
            var workers = _customerRepository.GetWorkersBySaloon(saloonId);
            if (workers == null)
            {
                return BadRequest("Bu salonda hiç worker yok!");
            }
            return Ok(workers);
        }

        [HttpPost]
        [Route("addappointment")]
        public ActionResult AddAppointment(int customerId, int saloonId, int workerId, DateTime dateT)
        {
            var appointment = _customerRepository.AddAppointment(customerId, saloonId, workerId, dateT);
            if (appointment.Result == null)
            {
                return BadRequest("Randevu alınamadı, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpGet]
        [Route("customerappointments")]
        public ActionResult GetAppointmentsById(int customerId)
        {
            var appointments = _customerRepository.GetAppointmentsById(customerId);
            if (appointments == null)
            {
                return BadRequest("Müşterinin hiç randevusu yok!");
            }
            return Ok(appointments);
        }
        [HttpPost]
        [Route("addreview")]
        public ActionResult AddReview(int customerId, int saloonId, int workerId, int appointmentId, double rate, string comment)
        {
            var review = _customerRepository.AddReview(customerId, saloonId, workerId, appointmentId, rate, comment);
            if (review.Result == null)
            {
                return BadRequest("Değerlendirme yapılamadı, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpGet]
        [Route("saloonreviews")]
        public ActionResult GetReviewsBySaloon(int saloonId)
        {
            var reviews = _customerRepository.GetReviewsBySaloon(saloonId);
            if (reviews == null)
            {
                return BadRequest("Kuaförün hiç değerlendirmesi yok!");
            }
            return Ok(reviews);
        }
        [HttpPost]
        [Route("addfavorite")]
        public ActionResult AddFavorite(int customerId, int saloonId)
        {
            var favorite = _customerRepository.AddFavorite(customerId, saloonId);
            if (favorite.Result == null)
            {
                return BadRequest("Favorilere eklenemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpDelete]
        [Route("unfavorite")]
        public ActionResult UnFavorite(int id)
        {
            var favorite = _customerRepository.UnFavorite(id);
            if (favorite.Result == null)
            {
                return BadRequest("Favorilerden çıkarılamadı, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpGet]
        [Route("customerfavorites")]
        public ActionResult GetFavoritesByCustomer(int customerId)
        {
            var favorites = _customerRepository.GetFavoritesByCustomer(customerId);
            if (favorites == null)
            {
                return BadRequest("Müşterinin hiç favorisi yok!");
            }
            return Ok(favorites);
        }
    }
}
