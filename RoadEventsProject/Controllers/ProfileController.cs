using Elfie.Serialization;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadEventsProject.BLL.DTO;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.Models;

namespace RoadEventsProject.Controllers
{
    public class ProfileController : Controller
    {
        private IUserService _userService;
        private IPhotoVideoService _photoVideoService;
        private IRoadEventsService _roadEventsService;
        private readonly IWebHostEnvironment _env;

        public ProfileController(IWebHostEnvironment env, IPhotoVideoService photoVideoService, IRoadEventsService roadEventsService, IUserService userService)
        {
            _env = env;
            _photoVideoService = photoVideoService;
            _roadEventsService = roadEventsService;
            _userService = userService;
        }

        public async Task<IActionResult> MainView()
        {
            var arr = await _roadEventsService.GetStatistic();
            ViewBag.TotalRequests = arr[0];
            ViewBag.AcceptedRequests = arr[1];
            ViewBag.RejectedRequests = arr[2];

            int iduser = GetIdUserCookie();
            var user = await _userService.GetUserById(iduser);

            return View(user);
        }

        public async Task<IActionResult> MyProfile()
        {
            int iduser = GetIdUserCookie();
            var user = await _userService.GetUserById(iduser);

            RegisterUserModel_ userModel = new() { FirstName = user.IdNameNavigation.FirstName, MiddleName = user.IdNameNavigation.MiddleName, LastName = user.IdNameNavigation.LastName, UserName=user.LoginUser};
            await CreateViewBagAppByUserId(iduser);

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(RegisterUserModel_ userModel)
        {
            int iduser = GetIdUserCookie();
            await _userService.ChangeLogin(iduser, userModel.UserName);
            await CreateViewBagAppByUserId(iduser);

            return RedirectToAction("MyProfile");
        }

        public async Task<IActionResult> FillInApplication()
        {
            var regions = await _roadEventsService.GetAllRegions();
            ViewBag.Regions = regions;
            return View();
        }


        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(ValueLengthLimit = 50 * 1024 * 1024)]
        public async Task<IActionResult> FillInApplication(Event newevent)
        {
            if(ModelState.IsValid)
            {
                if(newevent.Video!=null || newevent.Photo!=null)
                {
                    int iduser = GetIdUserCookie();
                    var arr = await _roadEventsService.CreateApp(newevent, iduser);

                    Image image = new Image();
                    Video video = new Video();

                    var roadEvent = await _roadEventsService.GetAppById(Convert.ToInt32(arr[0]));

                    if (newevent.Photo != null)
                    {
                        image.ImageUrl = arr[1];
                        await _photoVideoService.AddPhotoAsync(image);
                        roadEvent.IdImage = image.IdImage;
                    }
                
                    if (newevent.Video != null)
                    {
                        video.VideoUrl = arr[2];
                        await _photoVideoService.AddVideoAsync(video);
                        roadEvent.IdVideo = video.IdVideo;
                    }
                    await _roadEventsService.Update(roadEvent);

                    TempData["SuccessMessage"] = "Дякуємо за вашу заяву!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Завантажте відео або фото");
                }
            }
            var regions = await _roadEventsService.GetAllRegions();
            ViewBag.Regions = regions;
            return View();
        }

        public async Task<IActionResult> MyApplication(int idstatus, DateOnly date)
        {
            DateOnly dateEq = new();
            DateTime dateTime = new(date.Year, date.Month, date.Day);
            int iduser = GetIdUserCookie();

            List<RoadEvent> applications = new();

            if (date != dateEq)
            {
                applications = await _roadEventsService.GetAppByUserDateOrWithStatus(idstatus, iduser, dateTime);

                if (applications.Count == 0)
                {
                    ModelState.AddModelError("iddateError", "Не знайдено заяв з датою (" + date + ")");
                    return View(applications);
                }
                else
                {
                    return View(applications);
                }
            }
            applications = await _roadEventsService.GetAppByUserOrWithStatus(idstatus, iduser);

            return View(applications);
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesVillages(int regionId)
        {
            var cities = await _roadEventsService.GetCitiesVillagesByRegion(regionId);

            return Json(cities);
        }

        private async Task CreateViewBagAppByUserId(int iduser)
        {
              var arr = await _roadEventsService.GetAllRequestsByUser(iduser);

            ViewBag.AllApp = arr[0];
            ViewBag.AcceptedRequests = arr[1];
            ViewBag.RejectedRequests = arr[2];
        }

        private int GetIdUserCookie()
        {
            int iduser = 0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }
            return iduser;
        }
    }
}
