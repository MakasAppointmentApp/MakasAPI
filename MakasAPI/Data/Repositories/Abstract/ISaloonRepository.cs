using MakasAPI.Dtos.DtosForSaloon;
using MakasAPI.Dtos.DtosForUsers;
using MakasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data.Repositories.Abstract
{
    public interface ISaloonRepository : IAppRepository
    {
        Task<Saloon> UpdateSaloonLocation(UpdateSaloonLocation saloonObj);
        Task<Saloon> UpdateSaloonImage(UpdateSaloonImageDto saloon);
        Task<Saloon> UpdateSaloonName(UpdateSaloonNameDto saloonObj);
        Task<Saloon> UpdatePassword(UpdatePasswordDto updatePassword);
        Saloon GetSaloonById(int saloonId);
        Worker GetWorkerById(int id);
        Task<Worker> AddWorker(AddWorkerDto worker);
        Task<Worker> DeleteWorker(int id);
        Price GetPriceById(int id);
        Task<Price> AddPrice(AddPriceDto price);
        Task<Price> DeletePrice(int id);
        List<WorkerAppointmentDto> GetWorkerPastAppointments(int workerId);
        List<WorkerAppointmentDto> GetWorkerFutureAppointments(int workerId);
        List<Appointment> GetWorkerAppointments(int workerId);
        List<Worker> GetWorkersBySaloonId(int saloonId);

        List<Price> GetPricesBySaloonId(int saloonId);
        //İHTİYACA GÖRE YENİ SORGULAR YAZILABİLİR
    }
}
