using MakasAPI.Data.Repositories.Abstract;
using MakasAPI.Dtos.DtosForSaloon;
using MakasAPI.Dtos.DtosForUsers;
using MakasAPI.Helpers;
using MakasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.Repositories.Concrete
{
    public class SaloonRepository : ISaloonRepository
    {
        private DataContext _context;

        public SaloonRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        
        public Saloon GetSaloonById(int saloonId)
        {
            //Entity Frameworkten FindAsync ya da FirstOrDefaultAsync kullanmak daha iyi olabilir mi?
            var saloon = _context.Saloons.Include(s=>s.Prices).Include(s=>s.Workers).Include(s=>s.Reviews).FirstOrDefault(s => s.Id == saloonId);
            if (saloon != null)
            {
                return saloon;

            }
            return null;

        }

        public Worker GetWorkerById(int id)
        {
            var worker = _context.Workers.FirstOrDefault(w => w.Id == id);
            if (worker != null)
            {
                return worker;

            }
            return null;
        }
        public Price GetPriceById(int id)
        {
            var price = _context.Prices.FirstOrDefault(w => w.Id == id);
            if (price != null)
            {
                return price;

            }
            return null;
        }

        public async Task<Worker> AddWorker(AddWorkerDto worker)
        {
            if (worker.WorkerPhoto != null)
            {
                var workerToCreate = new Worker
                {
                    SaloonId = worker.SaloonId,
                    WorkerName = worker.WorkerName,
                    WorkerPhoto = worker.WorkerPhoto

                };
                _context.Add(workerToCreate);
                await _context.SaveChangesAsync();
                return workerToCreate;
            }
            else
            {
                var workerToCreate = new Worker
                {
                    SaloonId = worker.SaloonId,
                    WorkerName = worker.WorkerName

                };
                _context.Add(workerToCreate);
                await _context.SaveChangesAsync();
                return workerToCreate;
            }// return null koymak lazım

        }

        public async Task<Worker> DeleteWorker(int id)
        {
            var worker = GetWorkerById(id);
            var workerAppointments = GetWorkerAppointments(id);
            foreach (var appointment in workerAppointments)
            {
                _context.Appointments.Remove(appointment);
            }
            if (worker != null)
            {

                _context.Workers.Remove(worker);
                await _context.SaveChangesAsync();
                return worker;
            }
            return null;
        }
        public List<Appointment> GetWorkerAppointments(int workerId)
        {
            var appointments = _context.Appointments.Where(a => a.WorkerId == workerId).ToList();
            return appointments;
        }

        public async Task<Price> AddPrice(AddPriceDto price)
        {
            if (price.PriceName != null && price.PriceAmount != 0)
            {
                var priceToCreate = new Price
                {
                    SaloonId = price.SaloonId,
                    PriceName = price.PriceName,
                    PriceAmount = price.PriceAmount

                };
                _context.Add(priceToCreate);
                await _context.SaveChangesAsync();
                return priceToCreate;
            }
            return null;

        }

        public async Task<Price> DeletePrice(int id)
        {
            var price = GetPriceById(id);
            if (price != null)
            {
                _context.Remove(price);
                await _context.SaveChangesAsync();
                return price;
            }
            return null;
        }
        public async Task<Saloon> UpdateSaloonName(UpdateSaloonNameDto saloonObj)
        {
            var saloon = GetSaloonById(saloonObj.Id);
            if (saloon != null)
            {
                saloon.SaloonName = saloonObj.SaloonName;
                _context.Saloons.Update(saloon);
                await _context.SaveChangesAsync();
                return saloon;
            }
            return null;

        }

        public async Task<Saloon> UpdatePassword(UpdatePasswordDto updatePassword)
        {
            var saloon = GetSaloonById(updatePassword.Id);
            CryptographyExtension cryptography = new CryptographyExtension();
            if (cryptography.VerifyPasswordHash(updatePassword.OldPassword, saloon.SaloonPasswordHash, saloon.SaloonPasswordSalt))
            {
                //if burada girilen şifre eski şifre ile aynı mı diye kontrol ediyor
                if (!cryptography.VerifyPasswordHash(updatePassword.NewPassword, saloon.SaloonPasswordHash, saloon.SaloonPasswordSalt))
                {
                    byte[] passwordHash, passwordSalt;
                    cryptography.CreatePasswordHash(updatePassword.NewPassword, out passwordHash, out passwordSalt);
                    saloon.SaloonPasswordHash = passwordHash;
                    saloon.SaloonPasswordSalt = passwordSalt;
                    _context.Saloons.Update(saloon);
                    await _context.SaveChangesAsync();
                    return saloon;
                }
            }
            return null;
        }
        public async Task<Saloon> UpdateSaloonLocation(UpdateSaloonLocation saloonObj)
        {
            var saloon = GetSaloonById(saloonObj.Id);
            if (saloon != null)
            {
                saloon.SaloonLocation = saloonObj.SaloonLocation;
                _context.Saloons.Update(saloon);
                await _context.SaveChangesAsync();
                return saloon;
            }
            return null;
        }

        public async Task<Saloon> UpdateSaloonImage(UpdateSaloonImageDto saloon)
        {
            var result = GetSaloonById(saloon.Id);
            if (result != null)
            {
                result.SaloonImage = saloon.SaloonImage;
                _context.Saloons.Update(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public List<WorkerAppointmentDto> GetWorkerPastAppointments(int workerId)
        {
            var appointments = _context.Appointments.Where(a => a.Date < DateTime.Now && a.WorkerId == workerId).Join(_context.Customers, a => a.CustomerId, c => c.Id,
                (a, c) => new WorkerAppointmentDto
                {
                    Id = a.Id,
                    Date = a.Date,
                    CustomerFullName = c.CustomerName +" "+ c.CustomerSurname 
                }).ToList();
            if (appointments.Count != 0)
            {
                appointments.OrderBy(a => a.Date);
                appointments.Reverse();
                return appointments;
            }
            return null;
        }
      
        public List<WorkerAppointmentDto> GetWorkerFutureAppointments(int workerId)
        {
            var appointments = _context.Appointments.Where(a => a.Date > DateTime.Now && a.WorkerId == workerId).Join(_context.Customers, a => a.CustomerId, c => c.Id,
                (a, c) => new WorkerAppointmentDto
                {
                    Id = a.Id,
                    Date = a.Date,
                    CustomerFullName = c.CustomerName + " " + c.CustomerSurname
                }).ToList();
            if (appointments.Count != 0)
            {
                appointments.OrderBy(a => a.Date);
                return appointments;
            }
            return null;
        }
        public List<Worker> GetWorkersBySaloonId(int saloonId)
        {
            var workers = _context.Workers.Where(w => w.SaloonId == saloonId).ToList();
            if (workers.Count() != 0)
            {
                return workers;

            }
            return null;
        }

        public List<Price> GetPricesBySaloonId(int saloonId)
        {
            var prices = _context.Prices.Where(p => p.SaloonId == saloonId).ToList();
            if (prices.Count() != 0)
            {
                return prices;

            }
            return null;
        }
    }
}

