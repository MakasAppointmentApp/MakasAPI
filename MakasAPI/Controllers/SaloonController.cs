﻿using MakasAPI.Data.Repositories.Abstract;
using MakasAPI.Dtos.DtosForSaloon;
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
    [Route("api/Saloon")]
    public class SaloonController : Controller
    {
        private ISaloonRepository _saloonRepository;
        private IConfiguration _configuration;
        public SaloonController(ISaloonRepository saloonRepository, IConfiguration configuration)
        {
            _saloonRepository = saloonRepository;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("detail")]
        public ActionResult GetSaloonById(int id)
        {
            var saloon = _saloonRepository.GetSaloonById(id);
            if (saloon == null)
            {
                return BadRequest("Böyle bir salon yok!");
            }
            return Ok(saloon);
        }
        [HttpPost]
        [Route("updatelocation")]//add ve update bir arada
        public ActionResult UpdateSaloonLocation(int id ,string saloonLocation)
        {
            var saloon = _saloonRepository.UpdateSaloonLocation(id, saloonLocation);
            if (saloon.Result == null)
            {
                return BadRequest("Konum değiştirilemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpPost]
        [Route("updateimage")]
        public ActionResult UpdateSaloonImage(int id, byte[] saloonImage)
        {
            var saloon = _saloonRepository.UpdateSaloonImage(id, saloonImage);
            if (saloon.Result == null)
            {
                return BadRequest("Fotoğraf değiştirilemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpPost]
        [Route("updatename")]
        public ActionResult UpdateSaloonName(int id, string saloonName)
        {
            var saloon = _saloonRepository.UpdateSaloonName(id, saloonName);
            if (saloon.Result ==null)
            {
                return BadRequest("Salon adı değiştirilemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpPost]
        [Route("updatepassword")]
        public ActionResult UpdateSaloonPassword(int id, string oldPassword, string newPassword)
        {
            var saloon = _saloonRepository.UpdatePassword(id, oldPassword, newPassword);
            if (saloon.Result == null)
            {
                return BadRequest("Şifre değiştirilemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpPost]
        [Route("addworker")]
        public ActionResult AddWorker(int id, string workerName, byte[] workerImage)
        {
            var saloon = _saloonRepository.AddWorker(id, workerName, workerImage);
            if (saloon.Result == null)
            {
                return BadRequest("Çalışan eklenemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpDelete]
        [Route("deleteworker")]
        public ActionResult DeleteWorker(int id)
        {
            var worker = _saloonRepository.DeleteWorker(id);
            if (worker.Result == null)
            {
                return BadRequest("Çalışan silinemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpPost]
        [Route("addprice")]
        public ActionResult AddPrice(int id, string priceName, double priceAmount)
        {
            var price = _saloonRepository.AddPrice(id, priceName, priceAmount);
            if (price.Result == null)
            {
                return BadRequest("Fiyat eklenemedi, bir hata oluştu!");
            }
            return Ok(200);
        }
        [HttpDelete]
        [Route("deleteprice")]
        public ActionResult DeletePrice(int id)
        {
            var price = _saloonRepository.DeletePrice(id);
            if (price.Result == null)
            {
                return BadRequest("Fiyat silinemedi, bir hata oluştu!");
            }
            return Ok(200);
        }

        [HttpGet]
        [Route("pastappointments")]
        public ActionResult PastAppointments(int saloonId, int workerId, DateTime date)
        {
            var app = _saloonRepository.GetWorkerPastAppointments(saloonId,workerId, date);
            if (app.Count() == 0)
            {
                return BadRequest("Geçmiş randevu bulunamadı");
            }
            return Ok(app);
        }
        [HttpGet]
        [Route("futureappointments")]
        public ActionResult FutureAppointments(int saloonId, int workerId, DateTime date)
        {
            var app = _saloonRepository.GetWorkerFutureAppointments(saloonId, workerId, date);
            if (app.Count() == 0)
            {
                return BadRequest("Gelecek randevu bulunamadı");
            }
            return Ok(app);
        }
    }
}
