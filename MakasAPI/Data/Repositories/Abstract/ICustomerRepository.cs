using MakasAPI.Dtos.DtosForSaloon;
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
        List<SaloonsByLocationDto> GetSaloonsByLocation(string city, string district, bool gender);
        List<WorkersBySaloonDto> GetWorkersBySaloon(int saloonId);
        Task<Appointment> AddAppointment(int customerId, int saloonId, int workerId,DateTime dateT);
        List<AppointmentsWithSaloon> GetAppointmentsById(int customerId);
        Task<Review> AddReview(int customerId, int saloonId, int workerId, int appointmentId, double rate, string comment);
        List<ReviewsBySaloon> GetReviewsBySaloon(int saloonId);
        Task<Favorite> AddFavorite(int customerId, int saloonId);
        Task<Favorite> UnFavorite(int id);
        List<FavoriteByCustomerDto> GetFavoritesByCustomer(int customerId);


    }
}
