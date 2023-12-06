using Microsoft.EntityFrameworkCore;
using RoadEventsProject.DAL.DBContext;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.DAL.Repositories
{
    public class VehiclesRepository : IVehiclesRepository
    {
        protected readonly RoadEventsContext _context;

        public VehiclesRepository(RoadEventsContext context)
        {
            _context = context;
        }

        public async Task<List<Vehicle>> GetVehicleWithDrivers()
        {
            return await _context.Vehicles.Include(u => u.IdDriverNavigation).ToListAsync();
        }
    }
}
