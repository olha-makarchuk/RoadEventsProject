using Microsoft.Extensions.Configuration;
using RoadEventsProject.BLL.DTO;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.BLL.Services
{
    public class ViolationService : IViolationService
    {
        private IViolationRepository _violationRepository;
        private readonly IConfiguration _config;

        public ViolationService(IViolationRepository violationRepository, IConfiguration config)
        {
            _violationRepository = violationRepository;
            _config = config;
        }

        public async Task<Violation> AddAsync(Violation violation)
        {
            return await _violationRepository.AddAsync(violation);
        }

        public async Task<List<Vehicle>> GetVehicleWithDrivers()
        {
            return await _violationRepository.GetVehicleWithDrivers();
        }
        public async Task<List<TypeViolation>> GetAllTypes()
        {
            return await _violationRepository.GetAllTypes();
        }

        public async Task<ViolationAndTypesModel> CreateViolationAndTypesModel(RoadEvent roadEvent)
        {
            var model = new ViolationAndTypesModel();
            model.ViolationModel = new Violation();
            model.TypesModel = await _violationRepository.GetAllTypes();

            model.ViolationModel.DateEvent = roadEvent.DateEvent;
            model.ViolationModel.IdRoadEvent = roadEvent.IdRoadEvent;
            model.ViolationModel.IdUser = roadEvent.IdUser;
            model.ViolationModel.IdCityVillage = roadEvent.IdCityVillage;
            model.ViolationModel.IdCityVillageNavigation = roadEvent.IdCityVillageNavigation;
            return model;
        }

        public async Task<List<Violation>> GetAll()
        {
            return await _violationRepository.GetAll();
        }

        public async Task<List<Violation>> GetViolationsByRoadEvent(int idEvent)
        {
            return await _violationRepository.GetViolationsByRoadEvent(idEvent);
        }
    }
}
