using MakasAPI.Dtos.DtosForCustomers;
using MakasAPI.Dtos.DtosForSaloon;
using MakasAPI.Dtos.DtosForUsers;
using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.Repositories.Abstract
{
    public interface ICustomerRepository:IAppRepository
    {
        List<Saloon> GetSaloons();
        Saloon GetSaloonById(int saloonId);
        Favorite IsFavoriteByCustomer(int saloonId, int customerId);
        List<GetSaloonsByLocationDto> GetSaloonsByLocation(ListedSaloonLocationDto salonObj);
        List<WorkersBySaloonDto> GetWorkersBySaloon(int saloonId);
        Task<Appointment> AddAppointment(AddAppointmentDto app);
        List<AppointmentsWithSaloonDto> GetAppointmentsById(int customerId);
        Task<Review> AddReview(Review review);
        List<ReviewsBySaloon> GetReviewsBySaloon(int saloonId);
        ReviewDto GetReviewIfExists(int saloonId, int customerId,int workerId,int appointmentId);
        Task<Favorite> AddFavorite(AddFavoriteDto favorite);
        Task<Favorite> UnFavorite(int id);
        Task<Favorite> UnFavoriteV2(int customerId, int SaloonId);
        List<FavoriteByCustomerDto> GetFavoritesByCustomer(int customerId);
        Task<Appointment> cancelAppointment(int appointmentId);
        Customer GetCustomerById(int customerId);
        Task<Customer> UpdateCustomerName(UpdateCustomerNameDto customerObj);
        Task<Customer> UpdateCustomerPassword(UpdateCustomerPasswordDto customerUpdatePassword);
        Task<Customer> UpdateCustomerMail(UpdateCustomerMailDto customerUpdateMail);
        List<HourDto> GetAvailableHoursByDate(int workerId, DateTime date);
        Review GetReviewByAppointmentId(int Id);
    }
}
