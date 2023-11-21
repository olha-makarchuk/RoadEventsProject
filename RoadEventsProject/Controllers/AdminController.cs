using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadEventsProject.Models;
using RoadEventsProject.Models.Data;
using System.Drawing;
using System.Linq;

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

        public async Task<IActionResult> CreateViolation(int id)
        {
            var roadEvent = await _context.RoadEvents
                .Where(a => a.IdRoadEvent == id)
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .FirstOrDefaultAsync();

            if (roadEvent != null)
            {
                roadEvent.IdStatus = 2;
                _context.Update(roadEvent);
                await _context.SaveChangesAsync();
            }

            var model = new ViolationAndTypesModel();
            model.ViolationModel = new Violation();
            model.TypesModel = await _context.TypeViolations.ToListAsync();

            model.ViolationModel.DateEvent = roadEvent.DateEvent;
            model.ViolationModel.IdRoadEvent = roadEvent.IdRoadEvent;
            model.ViolationModel.IdUser = roadEvent.IdUser;
            model.ViolationModel.IdCityVillage = roadEvent.IdCityVillage;
            model.ViolationModel.IdCityVillageNavigation = roadEvent.IdCityVillageNavigation;

            ViewBag.TypeViolation = model.TypesModel;
            ViewBag.VehiclesAll = await _context.Vehicles.Include(u=>u.IdDriverNavigation).ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateViolation(ViolationAndTypesModel model)
        {
            if (model.ViolationModel != null)
            {
                bool ifExist = false;
                var vehicles = await _context.Vehicles.Include(u => u.IdDriverNavigation).ToListAsync();
                foreach(var vehicle in vehicles)
                {
                    if (vehicle.NumberCar == model.NumberCar)
                    {
                        ifExist = true;
                        model.ViolationModel.IdDriver = vehicle.IdDriver;
                        model.ViolationModel.IdVehicle = vehicle.IdVehicle;
                        _context.Add(model.ViolationModel);
                        _context.SaveChanges();

                        if (model.SelectedViolationTypes != null)
                        {
                            foreach (var type in model.SelectedViolationTypes)
                            {
                                _context.Add(new ViolationTypesConnected { IdViolation = model.ViolationModel.IdViolation, IdType = type });
                            }
                            _context.SaveChanges();
                        }
                        return RedirectToAction(nameof(AllApplications));
                    }
                }
                ModelState.AddModelError("NumberCar", "Автомобіль не знайдено");
                return View(model);
            }
            return View(model);
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

        //Пошук заяв користувача



    }

}
