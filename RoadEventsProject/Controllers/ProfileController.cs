using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        public IActionResult Test()
        {
            var regions = _context.Regions.ToList();
            ViewBag.Regions = regions;

            return View();
        }





        [HttpGet]
        public async Task<IActionResult> FillInApplication(int regionId)
        {
            var citiesVillages = await _context.CityVillages
                .Where(c => c.IdRegion == regionId)
                .Select(c => new
                {
                    value = c.IdCityVillage,
                    text = c.NameCityVillage
                })
                .ToListAsync();

            return Json(citiesVillages);
        }

        public IActionResult AllApplication()
        {
            return View();
        }
    }
}
