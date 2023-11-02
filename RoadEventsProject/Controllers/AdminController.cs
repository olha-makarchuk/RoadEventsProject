using Microsoft.AspNetCore.Mvc;

namespace RoadEventsProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult MainView()
        {
            return View();
        }
    }
}
