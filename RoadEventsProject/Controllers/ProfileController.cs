using Elfie.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            ViewBag.TotalRequests = await _roadEventsService.GetTotalRequests();
            ViewBag.AcceptedRequests = await _roadEventsService.GetAcceptedRequests();
            ViewBag.RejectedRequests = await _roadEventsService.GetRejectedRequests();

            int iduser = 0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }
            var user = await _userService.GetUserById(iduser);

            ViewBag.unprocessedCount = await _roadEventsService.GetUnprocessedRequests();

            return View(user);
        }

        public async Task<IActionResult> MyProfile()
        {
            int iduser=0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }
            var user = await _userService.GetUserById(iduser);
            RegisterUserModel userModel = new() { FirstName = user.IdNameNavigation.FirstName, MiddleName = user.IdNameNavigation.MiddleName, LastName = user.IdNameNavigation.LastName, UserName=user.LoginUser};

            ViewBag.AllApp = await _roadEventsService.GetTotalRequestsByUser(iduser);
            ViewBag.AcceptedRequests = await _roadEventsService.GetAcceptedRequestsByUser(iduser);
            ViewBag.RejectedRequests = await _roadEventsService.GetRejectedRequestsByUser(iduser);

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(RegisterUserModel userModel)
        {
            int iduser = 0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }

            var user = await _userService.GetUserById(iduser);
            //
            //var userName = user.IdNameNavigation;
            //var username = await _context.Names.Where(n => n.IdName == user.IdName).FirstOrDefaultAsync();

            user.LoginUser = userModel.UserName;

            //_context.Update(userName);
            await _userService.Update(user);

            ViewBag.AllApp = await _roadEventsService.GetTotalRequestsByUser(iduser);
            ViewBag.AcceptedRequests = await _roadEventsService.GetAcceptedRequestsByUser(iduser);
            ViewBag.RejectedRequests = await _roadEventsService.GetRejectedRequestsByUser(iduser);

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
            GoogleDrive googleDrive = new();

            if(ModelState.IsValid)
            {
                if(newevent.Video!=null || newevent.Photo!=null)
                {
                    int iduser = 0;
                    RoadEvent roadEvent = new RoadEvent();
                    Image image = new Image();
                    Video video = new Video();
                    if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
                    {
                        iduser = int.Parse(idCookie);
                    }

                    roadEvent.IdUser = iduser;
                    roadEvent.IdStatus = 1;
                    roadEvent.IdCityVillage = newevent.IdCityVillage;
                    roadEvent.DateEvent = newevent.DateEvent;
                    roadEvent.DescriptionEvent = newevent.DescriptionEvent;

                    await _roadEventsService.AddAsync(roadEvent);

                    if (newevent.Photo != null)
                    {
                        string fileName = $"{iduser}_{roadEvent.IdRoadEvent}";
                        string link =await googleDrive.UploadAsync(fileName, newevent.Photo, ".jpeg", "image/jpeg");

                        image.ImageUrl = link;
                        await _photoVideoService.AddPhotoAsync(image);
                        roadEvent.IdImage = image.IdImage;
                    }
                
                    if (newevent.Video != null)
                    {
                        string fileName = $"{iduser}_{roadEvent.IdRoadEvent}";
                        string link = await googleDrive.UploadAsync(fileName, newevent.Video, ".mp4", "video/mp4");

                        video.VideoUrl = link;
                        await _photoVideoService.AddVideoAsync(video);
                        roadEvent.IdVideo = video.IdVideo;
                    }

                    await _roadEventsService.AddAsync(roadEvent);

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
            int iduser = 0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }

            List<RoadEvent> applications = new List<RoadEvent>(); 
            if(date != dateEq)
            {
                applications = await _roadEventsService.GetAppByUserAndDate(iduser, dateTime);
                
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
            else
            {
                applications = await _roadEventsService.GetAppByUserWithAllDetails(iduser);
            }

            if(date == dateEq)
            {
                return View(applications);
            }
            ModelState.AddModelError("iddateError", "Не знайдено заяв з датою (" + date + ")");
            return View(applications);
        }


    }
}
