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
    public class UserRepository:IUserRepository
    {
        protected readonly RoadEventsContext _context;
        public UserRepository(RoadEventsContext context)
        {
            _context = context;
        }

        public async Task<UserInfo> Update(UserInfo user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
            return user; 
        }

        public async Task<List<UserInfo>> GetAllUsers()
        {
            return await _context.UserInfos.ToListAsync();
        }

        public async Task<UserInfo> GetUserById(int id)
        {
            var user =  await _context.UserInfos
                .Where(u => u.IdUser == id)
                .Include(re => re.IdNameNavigation)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<UserInfo> AddAsync(UserInfo user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Name> AddNameAsync(Name name)
        {
            await _context.AddAsync(name);
            await _context.SaveChangesAsync();
            return name;
        }

        public async Task<UserInfo> GetUserByName(string name)
        {
            var user = await _context.UserInfos
                .Where(u => u.LoginUser == name)
                .Include(re => re.IdNameNavigation)
                .FirstOrDefaultAsync();
            return user;
        }
    }
}
