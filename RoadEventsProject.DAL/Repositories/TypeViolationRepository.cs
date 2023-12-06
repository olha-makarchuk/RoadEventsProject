using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class TypeViolationRepository:ITypeViolationRepository
    {
        protected readonly RoadEventsContext _context;
        public TypeViolationRepository(RoadEventsContext context)
        {
            _context = context;
        }

        public async Task<List<TypeViolation>> GetAll()
        {
            return await _context.TypeViolations.ToListAsync();
        }
    }
}
