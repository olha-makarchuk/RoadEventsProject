using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public async Task<IActionResult> AllApplications(int idstatus, int idUser)
        {
            var users = await _context.UserInfos.ToListAsync();
            List<RoadEvent> events = new List<RoadEvent>();

            if (users.Exists(u=>u.IdUser == idUser))
            {
                if (idstatus == 0)//статус
                {
                    events = await _context.RoadEvents
                    .Where(u => u.IdUser == idUser)
                    .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                    .Include(u => u.IdVideoNavigation)
                    .Include(u => u.IdImageNavigation)
                    .OrderByDescending(u => u.DateEvent)
                    .ToListAsync();
                }
                else//статус і юзер
                {
                    events = await _context.RoadEvents
                    .Where(u => u.IdStatus == idstatus)
                    .Where(u => u.IdUser == idUser)
                    .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                    .Include(u => u.IdVideoNavigation)
                    .Include(u => u.IdImageNavigation)
                    .OrderByDescending(u => u.DateEvent)
                    .ToListAsync();
                }
                return View(events);
            }
            else if(idUser != 0)
            {
                ModelState.AddModelError("idUserError", "Не знайдено користувачаз id ("+idUser+")");
                return View(events);
            }
            if (idstatus != 0)
            {
                events = await _context.RoadEvents
                .Where(u => u.IdStatus == idstatus)
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .Include(u => u.IdVideoNavigation)
                .Include(u => u.IdImageNavigation)
                .OrderByDescending(u => u.DateEvent)
                .ToListAsync();
            }
            else
            {
                events = await _context.RoadEvents
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .Include(u => u.IdVideoNavigation)
                .Include(u => u.IdImageNavigation)
                .OrderByDescending(u => u.DateEvent)
                .ToListAsync();
            }
            return View(events);
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
            ViewBag.TypeViolation = await _context.TypeViolations.ToListAsync();
            ViewBag.VehiclesAll = await _context.Vehicles.Include(u => u.IdDriverNavigation).ToListAsync();

            int iduser = 0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }

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
                        model.ViolationModel.IdUser = iduser;
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
                if (model.SelectedViolationTypes.Count ==0)
                {
                    ModelState.AddModelError("SelectedViolationTypes", "Не вибрано жодного  типу порушення");
                    return View(model);
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

        public async Task<IActionResult> AllViolations(int idViolation)
        {
            AllViolationsTypesModel model = new();
            List <Violation> violations = new();
            var types = await _context.ViolationTypesConnecteds
                .Include(u => u.IdTypeNavigation)
                .Include(u => u.IdViolationNavigation)
                .ToListAsync();

            model.Types.AddRange(types);

            if (idViolation == 0)
            {
                violations = await _context.Violations
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .ToListAsync();
            }
            else
            {
                violations = await _context.Violations
                .Where(u=>u.IdRoadEvent ==idViolation)
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .ToListAsync();

                if (violations.Count == 0)
                {
                    ModelState.AddModelError("idViolationError", "Не знайдено порушень за заявою id (" + idViolation + ")");
                    return View(model);
                }
            }

            model.Violations.AddRange(violations);

            return View(model);
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
    }

}
