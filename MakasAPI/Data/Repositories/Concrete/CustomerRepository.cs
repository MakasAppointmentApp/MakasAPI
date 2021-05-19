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
        public Favorite IsFavoriteByCustomer(int saloonId, int customerId)
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

        public async Task<Appointment> AddAppointment(AddAppointmentDto app)
        {
            //Eğer bir hata olur ve başkası tarafından alınmış bir saate randevu alınmaya çalışılırsa kontrolü
            var notAvailableApp = _context.Appointments
                .FirstOrDefault(a => a.Date == app.Date && a.WorkerId == app.WorkerId);
            //Günde 1 customer sadece 1 tane randevu alsın diye bunu usera göstermeliyiz
            var userApp = _context.Appointments.FirstOrDefault
                (a => a.CustomerId == app.CustomerId && a.Date.Day == app.Date.Day &&
                 a.Date.Month == app.Date.Month && app.Date.Year == app.Date.Year);
            if (userApp == null)
            {

                if (notAvailableApp == null)
                {
                    if (app.CustomerId != 0 && app.SaloonId != 0 && app.WorkerId != 0)
                    {
                        var appointmentToCreate = new Appointment
                        {
                            CustomerId = app.CustomerId,
                            SaloonId = app.SaloonId,
                            WorkerId = app.WorkerId,
                            Date = app.Date

                        };
                        _context.Add(appointmentToCreate);
                        await _context.SaveChangesAsync();
                        return appointmentToCreate;
                    }
                    return null;
                }
                return null;
            }
            return null;
        }

        public List<AppointmentsWithSaloonDto> GetAppointmentsById(int customerId)
        {
            //exists kontrolü eklenecek LİSTE İÇİNE EXİSTS KONTROLÜ YAPMA ÇÖZÜMÜ BULAMADIM 
            var appointments = _context.Appointments.Where(s => s.CustomerId == customerId).Join(_context.Saloons, a => a.SaloonId, s => s.Id,

                (a, s) => new AppointmentsWithSaloonDto
                {
                    Id = a.Id,
                    SaloonName = s.SaloonName,
                    SaloonRate = s.SaloonRate,
                    WorkerName = a.Worker.WorkerName,
                    Date = a.Date,
                    AppointmentId = a.Id,
                    SaloonId = a.SaloonId,
                    WorkerId = a.WorkerId,
                    ReviewControl = "",
                    CustomerId = a.CustomerId
                }).ToList();

            if (appointments != null)
            {
                appointments.Sort((x, y) => y.Date.CompareTo(x.Date));  //tarihi tersten sıralama fonksiyonu.
                return appointments;
            }
            return null;
        }

        public async Task<Review> AddReview(Review review)
        {
            if (review.Rate != 0 && review.Comment != null)
            {
                int workerAppoitmentsCount=_context.Reviews.Count(r => r.WorkerId == review.WorkerId);
                var worker = _context.Workers.FirstOrDefault(w => w.Id == review.WorkerId);
                double newWorkerPoint= ((worker.WorkerRate * workerAppoitmentsCount) + review.Rate) / (workerAppoitmentsCount+1);
                worker.WorkerRate = newWorkerPoint;
                int saloonAppointmentsCount = _context.Reviews.Count(r => r.SaloonId == review.SaloonId);
                var saloon = _context.Saloons.FirstOrDefault(s => s.Id == review.SaloonId);
                double newSaloonPoint = ((saloon.SaloonRate * saloonAppointmentsCount) + review.Rate) / (saloonAppointmentsCount+1);
                saloon.SaloonRate = newSaloonPoint;
                _context.Add(review);
                await _context.SaveChangesAsync();
                return review;
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
                
                return reviews;
            }
            return null;
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
            if (customer != null && ifAny == null)
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

        public async Task<Favorite> AddFavorite(AddFavoriteDto favorite)
        {

            var favToCreate = new Favorite
            {
                SaloonId = favorite.SaloonId,
                CustomerId = favorite.CustomerId

            };
            _context.Add(favToCreate);
            await _context.SaveChangesAsync();
            return favToCreate;

        }

        public Favorite GetFavoriteById(int customerId, int saloonId)
        {
            var favorite = _context.Favorites.FirstOrDefault(f => f.CustomerId == customerId && f.SaloonId == saloonId);
            if (favorite != null)
            {
                return favorite;

            }
            return null;
        }

        public async Task<Favorite> UnFavoriteV2(int customerId, int SaloonId)
        {
            var favorite = GetFavoriteById(customerId, SaloonId);
            if (favorite != null)
            {

                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
                return favorite;
            }
            return null;
        }
        public List<HourDto> GetAvailableHoursByDate(int workerId, DateTime date)
        {
            int startedHour = 8;
            List<HourDto> availableHoursList = new List<HourDto>();
            var appointments = _context.Appointments.Where(a => a.WorkerId == workerId && a.Date.Date == date).ToList();
            List<int> notAvailableHoursList = new List<int>();
            foreach (var item in appointments)
            {
                if (!notAvailableHoursList.Contains(item.Date.TimeOfDay.Hours))
                {
                    notAvailableHoursList.Add(item.Date.TimeOfDay.Hours);
                }
            }
            if (date.Date == DateTime.Now.Date)
            {
                startedHour = DateTime.Now.TimeOfDay.Hours + 1;
            }
            for (int i = startedHour; i < 22; i++)
            {

                if (!notAvailableHoursList.Contains(i))
                {
                    availableHoursList.Add(new HourDto { Hour = i.ToString() });
                }
            }
            return availableHoursList;
        }
        public Favorite GetReviewById(int customerId, int saloonId)
        {
            var favorite = _context.Favorites.FirstOrDefault(f => f.CustomerId == customerId && f.SaloonId == saloonId);
            if (favorite != null)
            {
                return favorite;

            }
            return null;
        }
        public ReviewDto GetReviewIfExists(int saloonId, int customerId, int workerId, int appointmentId)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.AppointmentId == appointmentId && r.CustomerId == customerId && r.SaloonId == saloonId && r.WorkerId == workerId);


            if (review != null)
            {
                ReviewDto review2 = new ReviewDto
                {
                    CustomerId = customerId,
                    SaloonId = saloonId,
                    WorkerId = workerId,
                    AppointmentId = appointmentId
                };

                return review2;
            }
            return null;

        }
        public Review GetReviewByAppointmentId(int Id)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.AppointmentId == Id);
            if (review != null)
            {
                return review;
            }
            return null;
        }
        public List<Review> GetAppointmentReviews(int appId)
        {
            var reviews = _context.Reviews.Where(r => r.AppointmentId == appId).ToList();
            return reviews;
        }
        public async Task<Appointment> cancelAppointment(int appointmentId)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            var workerReviews = GetAppointmentReviews(appointmentId);
            foreach (var review in workerReviews)
            {
                _context.Reviews.Remove(review);
            }
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                return appointment;
            }
            return null;
        }
    }
}
