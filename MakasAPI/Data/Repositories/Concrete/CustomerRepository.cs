using MakasAPI.Data.Repositories.Abstract;
using MakasAPI.Dtos.DtosForCustomers;
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
    public class CustomerRepository : ICustomerRepository
    {
        private DataContext _context;

        public CustomerRepository(DataContext context)
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

        public List<Saloon> GetSaloons()
        {
            var saloons = _context.Saloons.ToList();
            if (saloons != null)
            {
                saloons.OrderBy(s => s.SaloonRate);
                saloons.Reverse();
                return saloons;
            }
            return null;
        }

        public Saloon GetSaloonById(int saloonId)
        {
            var saloon = _context.Saloons.Include(s => s.Prices).Include(s => s.Workers).Include(s => s.Reviews).FirstOrDefault(s => s.Id == saloonId);
            if (saloon != null)
            {
                return saloon;

            }
            return null;
        }
        public Favorite IsFavoriteByCustomer(int saloonId,int customerId)
        {
            var favorite = _context.Favorites.FirstOrDefault(f => f.SaloonId == saloonId && f.CustomerId == customerId);
            if (favorite != null)
            {
                return favorite;

            }
            return null;
        }

        public List<GetSaloonsByLocationDto> GetSaloonsByLocation(ListedSaloonLocationDto salonObj)
        {
            // Bu tamam sıralama için bakılabilir!?
            var saloons = _context.Saloons.Where(s => s.SaloonCity == salonObj.SaloonCity && s.SaloonDistrict == salonObj.SaloonDistrict && salonObj.SaloonGender == s.SaloonGender).Include(c => c.Workers).Select(
            s => new GetSaloonsByLocationDto
            {
                Id = s.Id,
                SaloonName = s.SaloonName,
                SaloonRate = s.SaloonRate,
                SaloonImage = s.SaloonImage,
                WorkerCount = s.Workers.Count()
            }
            ).ToList();
            if (saloons != null)
            {
                saloons.OrderBy(s => s.SaloonRate);
                return saloons;
            }
            return null;

        }

        public List<WorkersBySaloonDto> GetWorkersBySaloon(int saloonId)
        {
            // Bu Tamam!
            var workers = _context.Workers.Where(s => s.SaloonId == saloonId).Select(
                w => new WorkersBySaloonDto
                {
                    Id = w.Id,
                    WorkerName = w.WorkerName,
                    WorkerPhoto = w.WorkerPhoto,
                    WorkerRate = w.WorkerRate,
                }
                ).ToList();
            if (workers != null)
            {
                workers.OrderBy(s => s.WorkerRate);//worker rate ine göre listview e sıralanmış gönderim.
                return workers;
            }
            return null;
        }

        public async Task<Appointment> AddAppointment(int customerId, int saloonId, int workerId, DateTime dateT)
        {
            if (customerId != 0 && saloonId != 0 && workerId != 0)
            {
                var appointmentToCreate = new Appointment
                {
                    CustomerId = customerId,
                    SaloonId = saloonId,
                    WorkerId = workerId,
                    Date = dateT

                };
                _context.Add(appointmentToCreate);
                await _context.SaveChangesAsync();
                return appointmentToCreate;
            }
            return null;
        }

        public List<AppointmentsWithSaloon> GetAppointmentsById(int customerId)
        {
            //BU DA TAMAM!
            var appointments = _context.Appointments.Where(s => s.CustomerId == customerId).Join(_context.Saloons, a => a.SaloonId, s => s.Id,

                (a, s) => new AppointmentsWithSaloon
                {
                    Id = a.Id,
                    SaloonName = s.SaloonName,
                    SaloonRate = s.SaloonRate,
                    Date = a.Date
                }).ToList();
            if (appointments != null)
            {
                appointments.Sort((x, y) => y.Date.CompareTo(x.Date));  //tarihi tersten sıralama fonksiyonu.
                return appointments;
            }
            return null;
        }
   
        public async Task<Review> AddReview(int customerId, int saloonId, int workerId, int appointmentId, double rate, string comment)
        {
            if (rate != 0 && comment != null)
            {
                var reviewToCreate = new Review
                {
                    CustomerId = customerId,
                    SaloonId = saloonId,
                    WorkerId = workerId,
                    AppointmentId = appointmentId,
                    Rate = rate,
                    Comment = comment,
                    Date = DateTime.Now

                };
                _context.Add(reviewToCreate);
                await _context.SaveChangesAsync();
                return reviewToCreate;
            }
            return null;
        }
       
        public List<ReviewsBySaloon> GetReviewsBySaloon(int saloonId)
        {
            // BU DA TAMAM ANCAK YORUMUN YAZILDIĞI DATE İ DATETIME NOW DAN ÇIKARIP INT OLARAKTA TUTULABİLİR
              var reviews = _context.Reviews.Where(s => s.SaloonId == saloonId).Join(_context.Customers, r => r.CustomerId, c => c.Id,
                (r, c) => new ReviewsBySaloon
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    Rate = r.Rate,
                    Date = r.Date,
                    CustomerName = c.CustomerName + " " + c.CustomerSurname
                }
                ).ToList();
            if (reviews != null)
            {
                reviews.Reverse();
                return reviews;
            }
            return null;
        }

        public async Task<Favorite> AddFavorite(int customerId, int saloonId)
        {

            var favoriteToCreate = new Favorite
            {
                CustomerId = customerId,
                SaloonId = saloonId

            };
            _context.Add(favoriteToCreate);
            await _context.SaveChangesAsync();
            return favoriteToCreate;

        }

        public async Task<Favorite> UnFavorite(int id)
        {
            var favorite = _context.Favorites.FirstOrDefault(w => w.Id == id);
            if (favorite != null)
            {
                _context.Remove(favorite);
                await _context.SaveChangesAsync();
                return favorite;
            }
            return null;
        }
        public List<FavoriteByCustomerDto> GetFavoritesByCustomer(int customerId)
        {
            //BU TAMAM!!
            var favorites = _context.Favorites.Where(f => f.CustomerId == customerId).Join(_context.Saloons, f => f.SaloonId, s => s.Id,
                (f, s) => new FavoriteByCustomerDto
                {
                    Id = f.Id,
                    SaloonId = s.Id,
                    SaloonName = s.SaloonName,
                    SaloonImage = s.SaloonImage,
                    SaloonRate = s.SaloonRate
                }).ToList();
            if (favorites != null)
            {
                favorites.Reverse();  //En son eklenen başta duracak şekilde.
                return favorites;
            }
            return null;
        }
        public async Task<Customer> UpdateCustomerName(UpdateCustomerNameDto customerObj)
        {
            var customer = GetCustomerById(customerObj.Id);
            if (customer != null)
            {
                customer.CustomerName = customerObj.CustomerName;
                customer.CustomerSurname = customerObj.CustomerSurName;
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            return null;

        }
        public async Task<Customer> UpdateCustomerMail(UpdateCustomerMailDto customerObj)
        {
            var customer = GetCustomerById(customerObj.Id);
            var ifAny = _context.Customers.FirstOrDefault(c => c.CustomerEmail == customerObj.CustomerMail);
            if (customer != null && ifAny==null)
            {
                customer.CustomerEmail = customerObj.CustomerMail;
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            return null;

        }

        public async Task<Customer> UpdateCustomerPassword(UpdateCustomerPasswordDto updatePassword)
        {
            var customer = GetCustomerById(updatePassword.Id);
            CryptographyExtension cryptography = new CryptographyExtension();
            if (cryptography.VerifyPasswordHash(updatePassword.OldPassword, customer.CustomerPasswordHash, customer.CustomerPasswordSalt))
            {
                //if burada girilen şifre eski şifre ile aynı mı diye kontrol ediyor
                if (!cryptography.VerifyPasswordHash(updatePassword.NewPassword, customer.CustomerPasswordHash, customer.CustomerPasswordSalt))
                {
                    byte[] passwordHash, passwordSalt;
                    cryptography.CreatePasswordHash(updatePassword.NewPassword, out passwordHash, out passwordSalt);
                    customer.CustomerPasswordHash = passwordHash;
                    customer.CustomerPasswordSalt = passwordSalt;
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                    return customer;
                }
            }
            return null;
        }
        public Customer GetCustomerById(int customerId)
        {
            var customer = _context.Customers.FirstOrDefault(s => s.Id == customerId);
            if (customer != null)
            {
                return customer;

            }
            return null;
        }
    }
}
