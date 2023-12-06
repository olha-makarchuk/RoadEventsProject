using RoadEventsProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.DAL.Repositories.Base
{
    public interface IViolationRepository
    {
        Task<List<Violation>> GetAll();
        Task<List<Violation>> GetViolationsByRoadEvent(int idEvent);
        Task<Violation> AddAsync(Violation violation); 

    }
}
