using Microsoft.AspNetCore.Mvc;

namespace RoadEventsProject.Controllers
{
    public class ProfileController : Controller
    {
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
        public IActionResult FillInApplication() 
        {
            return View();
        }

        public IActionResult AllApplication()
        {
            return View();
        }
    }
}
