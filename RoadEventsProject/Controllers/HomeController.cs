using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadEventsProject.BLL.DTO;
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
        public async Task<IActionResult> Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                await _userService.Register(model);
                
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
                if(model.UserName ==null || model.Password == null)
                {
                    ModelState.AddModelError(string.Empty, "Заповніть всі поля");
                }
                var user = await _userService.GetUserByName(model.UserName);
                if (user != null)
                {
                    var check = _userService.CheckPassword(model, user.PasswordHash);

                    if (check == true)
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
}