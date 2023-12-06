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
    public class ViolationTypesConnectedsRepository : IViolationTypesConnectedsRepository
    {
        protected readonly RoadEventsContext _context;

        public ViolationTypesConnectedsRepository(RoadEventsContext context)
        {
            _context = context;
        }

        public async Task<ViolationTypesConnected> AddAsync(ViolationTypesConnected entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<ViolationTypesConnected>> GetAllWithItems()
        {
            return await _context.ViolationTypesConnecteds
                .Include(u => u.IdTypeNavigation)
                .Include(u => u.IdViolationNavigation)
                .ToListAsync();
        }
    }
}
