using Microsoft.Extensions.Configuration;
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
    public class VehiclesService: IVehiclesService
    {
        private IVehiclesRepository _vehiclesRepository;
        private readonly IConfiguration _config;
    
        public VehiclesService(IVehiclesRepository vehiclesRepository, IConfiguration config)
        {
            _vehiclesRepository = vehiclesRepository;
            _config = config;
        }

        public async Task<List<Vehicle>> GetVehicleWithDrivers()
        {
            return await _vehiclesRepository.GetVehicleWithDrivers();
        }
    }
}
