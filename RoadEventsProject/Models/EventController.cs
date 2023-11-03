using Microsoft.AspNetCore.Mvc;
using RoadEventsProject.Models.Data;

namespace RoadEventsProject.Models
{
    public class EventController : Controller
    {
        private readonly RoadEventsContext _context;

        public EventController(RoadEventsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCitiesVillages(int regionId)
        {
            // Отримайте список міст для вибраного регіону
            var cities = _context.CityVillages.Where(c => c.IdRegion == regionId).ToList();
            var a = Json(cities);
            // Поверніть список міст у форматі JSON
            return Json(cities);
        }
    }
}
