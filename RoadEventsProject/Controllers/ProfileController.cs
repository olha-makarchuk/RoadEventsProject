using Elfie.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadEventsProject.Models;
using RoadEventsProject.Models.Data;

namespace RoadEventsProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly RoadEventsContext _context;
        private readonly IWebHostEnvironment _env;

        public ProfileController(RoadEventsContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult MainView()
        {
            return View();
        }

        public async Task<IActionResult> MyProfile()
        {
            int iduser=0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }
            var user = await _context.UserInfos.Where(u=>u.IdUser==iduser).Include(re => re.IdNameNavigation).FirstOrDefaultAsync();
            RegisterUserModel userModel = new() { FirstName = user.IdNameNavigation.FirstName, MiddleName = user.IdNameNavigation.MiddleName, LastName = user.IdNameNavigation.LastName, UserName=user.LoginUser};
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

            var user = await _context.UserInfos.Where(u => u.IdUser == iduser).FirstOrDefaultAsync();
            var username = await _context.Names.Where(n => n.IdName == user.IdName).FirstOrDefaultAsync();

            username.FirstName=userModel.FirstName; 
            username.LastName=userModel.LastName; 
            username.MiddleName=userModel.MiddleName;

            user.LoginUser = userModel.UserName;

            _context.Update(username);
            _context.Update(user);
            _context.SaveChanges();

            return RedirectToAction("MyProfile");
        }

        public async Task<IActionResult> FillInApplication()
        {
            var regions = await _context.Regions.ToListAsync();
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

                    _context.Add(roadEvent);
                    _context.SaveChanges();

                    if (newevent.Photo != null)
                    {
                        string fileName = $"{iduser}_{roadEvent.IdRoadEvent}";
                        string link =await googleDrive.UploadAsync(fileName, newevent.Photo, ".jpeg", "image/jpeg");

                        image.ImageUrl = link;
                        _context.Add(image);
                        _context.SaveChanges();
                        roadEvent.IdImage = image.IdImage;
                    }
                
                    if (newevent.Video != null)
                    {
                        string fileName = $"{iduser}_{roadEvent.IdRoadEvent}";
                        string link = await googleDrive.UploadAsync(fileName, newevent.Video, ".mp4", "video/mp4");

                        //string[] link = await googleDrive.UploadVideoInPartsAsync(fileName, newevent.Video, ".mp4", "video/mp4");
                        //await googleDrive.CompressVideoTo720pAsync(newevent.Video);
                        /*video.VideoUrl = link;
                        _context.Add(video);
                        _context.SaveChanges();
                        roadEvent.IdVideo = video.IdVideo;*/
                    }

                    _context.Update(roadEvent);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Дякуємо за вашу заяву!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Завантажте відео або фото");
                }
            }
            var regions = _context.Regions.ToList();
            ViewBag.Regions = regions;
            return View();
        }

        public IActionResult MyApplication()
        {
            int iduser = 0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }

            var applications = _context.RoadEvents
                .Where(re=>re.IdUser == iduser)
                .Include(re => re.IdCityVillageNavigation)
                .Include(re => re.IdImageNavigation)
                .Include(re => re.IdStatusNavigation)
                .Include(re => re.IdUserNavigation)
                .Include(re => re.IdVideoNavigation)
                .ToList();

            return View(applications);
        }


    }
}
