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
                return BadRequest();
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
                return BadRequest();
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
                return BadRequest();
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
                return BadRequest();
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
                return BadRequest();
            }
            return Ok(200);
        }
    }
}
