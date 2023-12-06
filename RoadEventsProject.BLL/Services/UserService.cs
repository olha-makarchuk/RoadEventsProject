using Microsoft.Extensions.Configuration;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.BLL.Services
{
    public class UserService: IUserService
    {
        private IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<List<UserInfo>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<UserInfo> GetUserById(int userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task<UserInfo> Update(UserInfo user)
        {
            return await _userRepository.Update(user);
        }

        public async Task<UserInfo> AddAsync(UserInfo user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task<Name> AddNameAsync(Name name)
        {
            return await _userRepository.AddNameAsync(name);
        }

        public async Task<UserInfo> GetUserByName(string name)
        {
            return await _userRepository.GetUserByName(name);
        }
    }
}
