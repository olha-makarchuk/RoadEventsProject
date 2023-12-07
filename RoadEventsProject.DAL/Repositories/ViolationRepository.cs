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
    public class ViolationRepository: IViolationRepository
    {
        protected readonly RoadEventsContext _context;
        public ViolationRepository(RoadEventsContext context)
        {
            _context = context;
        }

        public async Task<Violation> AddAsync(Violation violation)
        {
            await _context.AddAsync(violation);
            await _context.SaveChangesAsync();
            return violation;
        }

        public async Task<List<Violation>> GetAll()
        {
            return await _context.Violations
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .Include(u => u.IdVehicleNavigation)
                .Include(u => u.IdDriverNavigation)
                .Include(u => u.IdRoadEventNavigation.IdUserNavigation)
                .ToListAsync();
        }

        public async Task<List<Violation>> GetViolationsByRoadEvent(int idEvent)
        {
            return await _context.Violations
                .Where(u => u.IdRoadEvent == idEvent)
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .ToListAsync();
        }
        public async Task<List<Vehicle>> GetVehicleWithDrivers()
        {
            return await _context.Vehicles.Include(u => u.IdDriverNavigation)
                .ToListAsync();
        }
        public async Task<List<TypeViolation>> GetAllTypes()
        {
            return await _context.TypeViolations.ToListAsync();
        }
    }
}
