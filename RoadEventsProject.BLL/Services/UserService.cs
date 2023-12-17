using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RoadEventsProject.BLL.DTO;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.BLL.Services
{
    public class UserService: IUserService
    {
        private IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task ChangeLogin(int iduser, string login)
        {
            var user = await _userRepository.GetUserById(iduser);
            user.LoginUser = login;
            await _userRepository.Update(user);
        }

        public async Task<UserInfo> Register(UserModel model)
        {
            MyObject myObject = new MyObject();
            myObject.Value = model.Password;

            UserInfo userInfo = new UserInfo();
            Name name = new Name();

            name.FirstName = model.FirstName;
            name.MiddleName = model.MiddleName;
            name.LastName = model.LastName;
            await _userRepository.AddNameAsync(name);

            userInfo.IdName = name.IdName;
            userInfo.IdRole = 1;
            userInfo.LoginUser = model.UserName;

            userInfo.PasswordHash = myObject.GetMd5Hash();
            await _userRepository.AddAsync(userInfo);

            return userInfo;
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

        public async Task<bool[]> CheckPassword(LoginUserModel model)
        {
            bool[] boolean = new bool[3];
            boolean[0] = false;
            
            var user =  await _userRepository.GetUserByName(model.UserName);
            if (user != null)
            {
                boolean[0] = true;
                MyObject myObject = new MyObject();
                myObject.Value = model.Password;
                var hash = myObject.GetMd5Hash();

                if (hash == user.PasswordHash)
                {
                    boolean[1] = true;
                }

                _httpContextAccessor.HttpContext.Response.Cookies.Append("MyIdCookie", user.IdUser.ToString());
                if (user.IdRole == 1)
                {
                    boolean[2] = true;
                }
                else
                {
                    boolean[2] = false;
                }
            }
            return boolean;
            
        }
    }

    public class MyObject
    {
        public string Value { get; set; }

        public string GetMd5Hash()
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(Value);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

    }
}
