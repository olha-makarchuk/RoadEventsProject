using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadEventsProject.Models;
using RoadEventsProject.Models.Data;

namespace RoadEventsProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoadEventsContext _context;
        public int userId { get; set; }

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
            var applications = await _context.RoadEvents
            .Where(u => u.IdStatus == 1)
            .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
            .Include(u => u.IdVideoNavigation)
            .Include(u => u.IdImageNavigation)
            .OrderByDescending(u => u.DateEvent)
            .ToListAsync();
            return View(applications);
        }

        public async Task<IActionResult> ConfirmApplication(int id)
        {
            var app = await _context.RoadEvents.Where(a => a.IdRoadEvent == id).FirstOrDefaultAsync();
            if (app != null)
            {
                app.IdStatus = 2;
                _context.Update(app);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AllApplications));
        }

        public async Task<ActionResult> RejectApplication(int id)
        {
            var app = await _context.RoadEvents.FirstOrDefaultAsync(a => a.IdRoadEvent == id);

            if (app != null)
            {
                app.IdStatus = 3;
                _context.Update(app);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AllApplications));
        }

        public async Task<IActionResult> AllViolations()
        {
            var violations = await _context.Violations.ToListAsync();
            return View(violations);
        }

        public async Task<IActionResult> SeeUser(int user)
        {
            var userInfo = await _context.UserInfos.Where(u => u.IdUser == user)
                .Include(u=>u.IdNameNavigation)
                .FirstOrDefaultAsync();
            userId = userInfo.IdUser;
            Response.Cookies.Append("IdUserApplication", userInfo.IdUser.ToString());
            return PartialView("_UserInfo", userInfo);
        }

        public async Task<IActionResult> BlockUser()
        {
            int iduser = 0;
            if (Request.Cookies.TryGetValue("IdUserApplication", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }
            var user = await _context.UserInfos.Where(u=>u.IdUser== iduser)
                .Include(u => u.IdNameNavigation)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                user.Blocked = true;
                _context.Update(user);
                _context.SaveChanges();
            }
            return PartialView("_UserInfo", user);
        }

        public async Task<IActionResult> UnlockUser()
        {
            int iduser = 0;
            if (Request.Cookies.TryGetValue("IdUserApplication", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }
            var user = await _context.UserInfos.Where(u => u.IdUser == iduser)
                .Include(u => u.IdNameNavigation)
                .FirstOrDefaultAsync();
            if (user!= null)
            {
                user.Blocked = false;
                _context.Update(user);
                _context.SaveChanges();
            }
            return PartialView("_UserInfo", user);
        }

        public async Task<IActionResult> CreateViolations(int idApplication)
        {
            var regions = await _context.Regions.ToListAsync();
            ViewBag.Regions = regions;

            var thisevent = await _context.RoadEvents.Where(a => a.IdRoadEvent == idApplication).FirstOrDefaultAsync();

            Violation violation = new();
            violation.DateEvent = thisevent.DateEvent;
            return View();
        }

        
        
        
        //Функції

        //Пошук всіх заяв користувачів
        public async Task<ActionResult> FindAllUserApplicatons(int idUser)
        {
            var applications = await _context.RoadEvents
            .Where(u => u.IdStatus == 1)
            .Where(u => u.IdUser == idUser)
            .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
            .Include(u => u.IdVideoNavigation)
            .Include(u => u.IdImageNavigation)
            .OrderByDescending(u => u.DateEvent)
            .ToListAsync();
            return null;/////////////передати applications в AllApplications
        }

        //Пошук заяв у місті, області
        public async Task<ActionResult> FindApplicatonsByRegion(int idRegion)
        {
            var applications = await _context.RoadEvents
            .Where(u => u.IdStatus == 1)
            .Where(u => u.IdCityVillageNavigation.IdRegion == idRegion)
            .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
            .Include(u => u.IdVideoNavigation)
            .Include(u => u.IdImageNavigation)
            .OrderByDescending(u => u.DateEvent)
            .ToListAsync();
            return null;
        }
        public async Task<ActionResult> FindApplicatonsByCityOrVillage(int idCitiVillage)
        {
            var applications = await _context.RoadEvents
            .Where(u => u.IdStatus == 1)
            .Where(u => u.IdCityVillage == idCitiVillage)
            .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
            .Include(u => u.IdVideoNavigation)
            .Include(u => u.IdImageNavigation)
            .OrderByDescending(u => u.DateEvent)
            .ToListAsync();
            return null;
        }

        //Пошук заяв за датою
        public async Task<ActionResult> FindApplicatonsByDate(DateTime date)
        {
            var applications = await _context.RoadEvents
            .Where(u => u.IdStatus == 1)
            .Where(u => u.DateEvent.Date == date.Date)
            .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
            .Include(u => u.IdVideoNavigation)
            .Include(u => u.IdImageNavigation)
            .OrderByDescending(u => u.DateEvent)
            .ToListAsync();
            return null;
        }

    }
}
