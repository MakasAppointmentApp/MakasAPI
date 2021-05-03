using MakasAPI.Dtos.DtosForSaloon;
using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.Repositories.Abstract
{
    public interface ISaloonRepository : IAppRepository
    {
        Task<Saloon> UpdateSaloonLocation(int id, string saloonLocation);
        Task<Saloon> UpdateSaloonImage(int id, byte[] image);
        Task<Saloon> UpdateSaloonName(int id, string saloonName);
        Task<Saloon> UpdatePassword(int id, string oldPassword, string newPassword);
        Saloon GetSaloonById(int saloonId);
        Worker GetWorkerById(int id);
        Task<Worker> AddWorker(int id, string workerName, byte[] workerImage);
        Task<Worker> DeleteWorker(int id);
        Price GetPriceById(int id);
        Task<Price> AddPrice(int saloonId, string priceName, double priceAmount);
        Task<Price> DeletePrice(int id);
        List<WorkerAppointmentDto> GetWorkerPastAppointments(int workerId);
        List<WorkerAppointmentDto> GetWorkerFutureAppointments(int workerId);
        List<Worker> GetWorkersBySaloonId(int saloonId);

        //İHTİYACA GÖRE YENİ SORGULAR YAZILABİLİR
    }
}
