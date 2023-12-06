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
    public class ViolationTypesConnectedsService : IViolationTypesConnectedsService
    {
        private readonly IConfiguration _config;
        private IViolationTypesConnectedsRepository _violationTypesConnectedsRepository;

        public ViolationTypesConnectedsService(IConfiguration config, IViolationTypesConnectedsRepository violationTypesConnectedsRepository)
        {
            _config = config;
            _violationTypesConnectedsRepository = violationTypesConnectedsRepository;
        }

        public async Task<ViolationTypesConnected> AddAsync(ViolationTypesConnected entity)
        {
            return await _violationTypesConnectedsRepository.AddAsync(entity);
        }

        public async Task<List<ViolationTypesConnected>> GetAllWithItems()
        {
            return await _violationTypesConnectedsRepository.GetAllWithItems();
        }
    }
}
