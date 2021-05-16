using MakasAPI.Data.Repositories.Abstract;
using MakasAPI.Dtos.DtosForCustomers;
using MakasAPI.Dtos.DtosForUsers;
using MakasAPI.Models;
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
        [Route("customerdetail")]
        public ActionResult GetCustomerById(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                return BadRequest("Böyle bir müşteri yok!");
            }
            return Ok(customer);
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
        public ActionResult GetSaloonsByLocation(ListedSaloonLocationDto saloonObj)
        {
            var saloons = _customerRepository.GetSaloonsByLocation(saloonObj);
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
        [Route("isfavorite")]
        public ActionResult IsFavoriteByCustomer(int saloonId, int customerId)
        {
            var favorite = _customerRepository.IsFavoriteByCustomer(saloonId, customerId);
            bool isF = false;
            if (favorite != null)
            {
                isF = true;
            }

            return Ok(isF);
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
        [Route("updatename")]
        public ActionResult UpdateSaloonName([FromBody] UpdateCustomerNameDto customerObject)
        {
            var customer = _customerRepository.UpdateCustomerName(customerObject);
            if (customer.Result == null)
            {
                return BadRequest("Müşteri adı değiştirilemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpPost]
        [Route("updatepassword")]
        public ActionResult UpdateSaloonPassword([FromBody] UpdateCustomerPasswordDto updatePassword)
        {
            var saloon = _customerRepository.UpdateCustomerPassword(updatePassword);
            if (saloon.Result == null)
            {
                return BadRequest("Şifre değiştirilemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpPost]
        [Route("updatecustomermail")]
        public ActionResult UpdateCustomerMail([FromBody] UpdateCustomerMailDto updateMail)
        {
            var saloon = _customerRepository.UpdateCustomerMail(updateMail);
            if (saloon.Result == null)
            {
                return BadRequest("Mail değiştirilemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpPost]
        [Route("addappointment")]
        public ActionResult AddAppointment([FromBody] AddAppointmentDto app)
        {
            var appointment = _customerRepository.AddAppointment(app);
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
        public ActionResult AddReview([FromBody]Review review)
        {
            var response = _customerRepository.AddReview(review);
            if (response.Result == null)
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
        public ActionResult AddFavorite([FromBody] AddFavoriteDto favorite)
        {
            var fav = _customerRepository.AddFavorite(favorite);
            if (fav.Result == null)
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
        [HttpDelete]
        [Route("unfavoritev2")]
        public ActionResult UnFavoriteV2(int customerId, int SaloonId)
        {
            var favorite = _customerRepository.UnFavoriteV2(customerId,SaloonId);
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
        [HttpGet]
        [Route("availablehours")]
        public ActionResult GetAvailableHoursByDate(int workerId, DateTime date)
        {
            var result = _customerRepository.GetAvailableHoursByDate(workerId, date);
            if (result.Count() == 0)
            {
                return BadRequest("Hiç uygun saat yok");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("getexistsreviews")]
        public ActionResult GetReviewIfExists(int saloonId, int customerId, int workerId, int appointmentId)
        {
            var review = _customerRepository.GetReviewIfExists(saloonId, customerId, workerId, appointmentId);
            if (review == null)
            {
                return BadRequest("");
            }
            return Ok(review);
        }
        [HttpGet]
        [Route("specialreview")]
        public ActionResult GetReviewByAppointmentId(int Id)
        {
            var review = _customerRepository.GetReviewByAppointmentId(Id);
            if (review == null)
            {
                return BadRequest("Değerlendirme bulunamadı");
            }
            return Ok(review);
        }
    }
}
