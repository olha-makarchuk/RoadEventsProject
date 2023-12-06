using RoadEventsProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.DAL.Repositories.Base
{
    public interface IViolationTypesConnectedsRepository
    {
        Task<ViolationTypesConnected> AddAsync(ViolationTypesConnected entity);
        Task<List<ViolationTypesConnected>> GetAllWithItems();
    }
}
