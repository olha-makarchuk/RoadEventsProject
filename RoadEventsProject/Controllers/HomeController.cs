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

                var check = await _userService.CheckPassword(model);

                if (check[0])
                {
                    if (check[1])
                    {
                        if (check[2])
                        {
                            return RedirectToAction("MainView", "Profile");
                        }
                        else
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