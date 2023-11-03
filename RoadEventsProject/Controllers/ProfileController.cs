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
                if(newevent.Video==null && newevent.Photo==null)
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
                        var filePath = Path.Combine(uploadsFolder, $"{iduser}_{roadEvent.IdRoadEvent}");

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            newevent.Photo.CopyTo(fileStream);
                        }
                        image.ImageUrl = filePath;
                        _context.Add(image);
                        _context.SaveChanges();
                        roadEvent.IdImage = image.IdImage;
                    }
                    if (newevent.Video != null)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Videos");

                        var filePath = Path.Combine(uploadsFolder, $"{iduser}_{roadEvent.IdRoadEvent}");

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
                }
                ModelState.AddModelError(string.Empty, "Завантажте відео або фото");
            }
            var regions = _context.Regions.ToList();
            ViewBag.Regions = regions;
            return View();
        }
    }
}
