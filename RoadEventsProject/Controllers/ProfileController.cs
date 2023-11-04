using Elfie.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadEventsProject.Models;
using RoadEventsProject.Models.Data;

namespace RoadEventsProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly RoadEventsContext _context;
        public ProfileController(RoadEventsContext context)
        {
            _context = context;
        }
        public IActionResult MainView()
        {
            
            return View();
        }

        public IActionResult Logout() 
        { 
            return View("Index", "Home");
        }

        public IActionResult MyProfile()
        {
            return View();
        }


        public async Task<IActionResult> FillInApplication()
        {
            var regions = await _context.Regions.ToListAsync();
            ViewBag.Regions = regions;
            return View();
        }

        [HttpPost]
        public IActionResult FillInApplication(Event newevent)
        {
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
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Photos");
                        string fileName = $"{iduser}_{roadEvent.IdRoadEvent}.jpeg";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            newevent.Photo.CopyTo(fileStream);
                        }
                        image.ImageUrl = $"/Uploads/Photos/{fileName}";
                        _context.Add(image);
                        _context.SaveChanges();
                        roadEvent.IdImage = image.IdImage;
                    }
                
                    if (newevent.Video != null)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Videos");

                        var filePath = Path.Combine(uploadsFolder, $"{iduser}_{roadEvent.IdRoadEvent}.mp4");

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            newevent.Video.CopyTo(fileStream);
                        }
                        video.VideoUrl = filePath;
                        _context.Add(video);
                        _context.SaveChanges();
                        roadEvent.IdVideo = video.IdVideo;
                    }

                    _context.Update(roadEvent);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Дякую!";
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
