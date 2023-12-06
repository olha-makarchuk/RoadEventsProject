using RoadEventsProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.DAL.Repositories.Base
{
    public interface IUserRepository
    {
        Task<List<UserInfo>> GetAllUsers();
        Task<UserInfo> GetUserById(int userId);
        Task<UserInfo> GetUserByName(string name);

        Task<UserInfo> Update(UserInfo user);
        Task<UserInfo> AddAsync(UserInfo user);
        Task<Name> AddNameAsync(Name name);




    }
}
