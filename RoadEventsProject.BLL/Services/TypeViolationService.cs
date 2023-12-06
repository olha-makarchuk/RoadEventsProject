using Microsoft.Extensions.Configuration;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.DBContext;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.BLL.Services
{
    public class TypeViolationService:ITypeViolationService
    {
        private ITypeViolationRepository _typeViolationRepository;
        private readonly IConfiguration _config;

        public TypeViolationService(ITypeViolationRepository typeViolationRepository, IConfiguration config)
        {
            _typeViolationRepository = typeViolationRepository;
            _config = config;
        }

        public async Task<List<TypeViolation>> GetAll()
        {
            return await _typeViolationRepository.GetAll();
        }
    }
}
