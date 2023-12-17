using RoadEventsProject.BLL.DTO;
using RoadEventsProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.BLL.Services.Base
{
    public interface IViolationService
    {
        Task<List<Violation>> GetAll();
        Task<List<Violation>> GetViolationsByRoadEvent(int idEvent);
        Task<Violation> AddAsync(Violation violation);
        Task<List<Vehicle>> GetVehicleWithDrivers();
        Task<List<TypeViolation>> GetAllTypes();
        Task<ViolationAndTypesModel> CreateViolationAndTypesModel(RoadEvent roadEvent);

    }
}
