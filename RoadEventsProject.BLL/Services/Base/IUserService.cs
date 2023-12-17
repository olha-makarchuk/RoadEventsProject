using RoadEventsProject.BLL.DTO;
using RoadEventsProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.BLL.Services.Base
{
    public interface IUserService
    {
        Task<List<UserInfo>> GetAllUsers();
        Task<UserInfo> GetUserById(int userId);
        Task<UserInfo> Update(UserInfo user);
        Task<UserInfo> Register(UserModel model);
        Task<bool[]> CheckPassword(LoginUserModel model);
        Task ChangeLogin(int iduser, string name);

    }
}
