using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadEventsProject.Models.Data;

namespace RoadEventsProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoadEventsContext _context;
        public AdminController(RoadEventsContext context)
        {
            _context = context;
        }
        public IActionResult MainView()
        {
            return View();
        }

        public async Task<IActionResult> AllApplications()
        {
            var applications = await _context.RoadEvents.ToListAsync();
            return View(applications);
        }
        public async Task<IActionResult> AllViolations()
        {
            var violations = await _context.Violations.ToListAsync();
            return View(violations);
        }

        public async Task<IActionResult> CreateViolations()
        {
            var violations = await _context.Violations.ToListAsync();
            return View(violations);
        }

        public async Task<IActionResult> SeeUser(int user)
        {
            var userInfo = await _context.UserInfos.Where(u => u.IdUser == user)
                .Include(u=>u.IdNameNavigation)
                .FirstOrDefaultAsync();
            return PartialView("_UserInfo", userInfo);
        }

        public async Task<IActionResult> RejectApplication(int idApp)
        {
            var app = await _context.RoadEvents.Where(a=>a.IdRoadEvent == idApp).FirstOrDefaultAsync();
            app.IdStatus = 3;
            _context.Update(app);
            return View();
        }

        public async Task<IActionResult> ConfirmApplication(int idApp)
        {
            var app = await _context.RoadEvents.Where(a => a.IdRoadEvent == idApp).FirstOrDefaultAsync();
            app.IdStatus = 2;
            _context.Update(app);
            return View();
        }

        public async Task<IActionResult> BlockUser(int idUser)
        {
            var user = await _context.UserInfos.Where(u=>u.IdUser== idUser).FirstOrDefaultAsync();
            return View();
        }

        public async Task<IActionResult> UnlockUser(int idUser)
        {
            return View();
        }
    }
}
