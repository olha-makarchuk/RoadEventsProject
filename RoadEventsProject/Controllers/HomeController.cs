using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace RoadEventsProject.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _userService = userService;
            _logger = logger;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            MyObject myObject = new MyObject();
            myObject.Value = model.Password;

            if (ModelState.IsValid)
            {
                UserInfo userInfo = new UserInfo();
                Name name = new Name();

                name.FirstName = model.FirstName;
                name.MiddleName = model.MiddleName;
                name.LastName = model.LastName;
                await _userService.AddNameAsync(name);

                userInfo.IdName = name.IdName;
                userInfo.IdRole = 1;
                userInfo.LoginUser = model.UserName;

                userInfo.PasswordHash = myObject.GetMd5Hash();
                await _userService.AddAsync(userInfo);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByName(model.UserName);
                if (user != null)
                {
                    MyObject myObject = new MyObject();
                    myObject.Value = model.Password;
                    if (myObject.GetMd5Hash() == user.PasswordHash)
                    {
                        if (user.IdRole == 1)
                        {
                            Response.Cookies.Append("MyIdCookie", user.IdUser.ToString());

                            return RedirectToAction("MainView", "Profile");
                        }
                        if (user.IdRole == 2)
                        {
                            return RedirectToAction("MainView", "Admin");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Неправильне ім'я користувача або пароль.");
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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