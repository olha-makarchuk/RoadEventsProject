using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.Models;
using System.Drawing;
using System.Linq;

namespace RoadEventsProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private IRoadEventsService _roadEventsService;
        private IUserService _userService;
        private IViolationService _violationServices;
        private ITypeViolationService _typeViolationService;
        private IVehiclesService _vehiclesService;
        private IViolationTypesConnectedsService _violationTypesConnectedService;
        public int userId { get; set; }

        public AdminController(IConfiguration configuration, IUserService userService, IRoadEventsService roadEventsService, 
            IViolationService violationServices, ITypeViolationService typeViolationService
            , IVehiclesService vehiclesService, IViolationTypesConnectedsService violationTypesConnectedsService)
        {
            _vehiclesService = vehiclesService;
            _typeViolationService = typeViolationService;
            _userService = userService;
            _roadEventsService = roadEventsService;
            _configuration = configuration;
            _violationServices = violationServices;
            _typeViolationService = typeViolationService;
            _violationTypesConnectedService = violationTypesConnectedsService;
        }

        public async Task<IActionResult> MainViewAsync()
        {
            ViewBag.TotalRequests = await _roadEventsService.GetTotalRequests();
            ViewBag.AcceptedRequests = await _roadEventsService.GetAcceptedRequests();
            ViewBag.RejectedRequests = await _roadEventsService.GetRejectedRequests();

            int iduser = 0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }
            var user = await _userService.GetUserById(iduser);

            ViewBag.unprocessedCount = await _roadEventsService.GetUnprocessedRequests();

            return View(user);
        }

        public async Task<IActionResult> AllApplications(int idstatus, int idUser)
        {
            var users = await _userService.GetAllUsers();
            List<RoadEvent> events = new();

            if (users.Exists(u=>u.IdUser == idUser))
            {
                if (idstatus == 0)
                {
                    events = await _roadEventsService.GetAppByUser(idUser);
                }
                else
                {
                    events = await _roadEventsService.GetAppByStatusAndUser(idstatus, idUser);
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
                events = await _roadEventsService.GetAppByStatus(idstatus);
            }
            else
            {
                events = await _roadEventsService.GetAllApp();
            }
            return View(events);
        }

        
        public async Task<IActionResult> CreateViolation(int id)
        {
            var roadEvent = await _roadEventsService.GetAppById(id);

            if (roadEvent != null)
            {
                roadEvent.IdStatus = 2;
                await _roadEventsService.Update(roadEvent);
            }

            var model = new ViolationAndTypesModel();
            model.ViolationModel = new Violation();
            model.TypesModel = await _typeViolationService.GetAll();

            model.ViolationModel.DateEvent = roadEvent.DateEvent;
            model.ViolationModel.IdRoadEvent = roadEvent.IdRoadEvent;
            model.ViolationModel.IdUser = roadEvent.IdUser;
            model.ViolationModel.IdCityVillage = roadEvent.IdCityVillage;
            model.ViolationModel.IdCityVillageNavigation = roadEvent.IdCityVillageNavigation;

            ViewBag.TypeViolation = model.TypesModel;
            ViewBag.VehiclesAll = await _vehiclesService.GetVehicleWithDrivers();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateViolation(ViolationAndTypesModel model)
        {
            var vehicles = await _vehiclesService.GetVehicleWithDrivers();
            model.TypesModel = await _typeViolationService.GetAll();

            ViewBag.TypeViolation = model.TypesModel;
            ViewBag.VehiclesAll = vehicles;

            int iduser = 0;
            if (Request.Cookies.TryGetValue("MyIdCookie", out string idCookie))
            {
                iduser = int.Parse(idCookie);
            }

            if (model.ViolationModel != null)
            {
                bool ifExist = false;
                foreach (var vehicle in vehicles)
                {
                    if (vehicle.NumberCar == model.NumberCar)
                    {
                        ifExist = true;
                        model.ViolationModel.IdDriver = vehicle.IdDriver;
                        model.ViolationModel.IdVehicle = vehicle.IdVehicle;
                        model.ViolationModel.IdUser = iduser;
                        await _violationServices.AddAsync(model.ViolationModel);

                        if (model.SelectedViolationTypes != null)
                        {
                            foreach (var type in model.SelectedViolationTypes)
                            {
                                await _violationTypesConnectedService.AddAsync(new ViolationTypesConnected { IdViolation = model.ViolationModel.IdViolation, IdType = type });
                            }
                        }
                        return RedirectToAction(nameof(AllApplications));
                    }
                }
                if (model.SelectedViolationTypes == null)
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
            var app = await _roadEventsService.GetAppById(id);

            if (app != null)
            {
                app.IdStatus = 3;
                await _roadEventsService.Update(app);
            }
            return RedirectToAction(nameof(AllApplications));
        }
        
        public async Task<IActionResult> AllViolations(int idEvent)
        {
            AllViolationsTypesModel model = new();
            List <Violation> violations = new();
            var types = await _violationTypesConnectedService.GetAllWithItems();

            model.Types.AddRange(types);

            if (idEvent == 0)
            {
                violations = await _violationServices.GetAll();
            }
            else
            {
                violations = await _violationServices.GetViolationsByRoadEvent(idEvent);

                if (violations.Count == 0)
                {
                    ModelState.AddModelError("idViolationError", "Не знайдено порушень за заявою id (" + idEvent + ")");
                    return View(model);
                }
            }

            model.Violations.AddRange(violations);

            return View(model);
        }

        public async Task<IActionResult> SeeUser(int user)
        {
            var userInfo = await _userService.GetUserById(user);

            GetViewBagUserInfo(user);

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

            var user = await _userService.GetUserById(iduser);

            GetViewBagUserInfo(iduser);

            if (user != null)
            {
                user.Blocked = true;
                await _userService.Update(user);
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

            var user = await _userService.GetUserById(iduser);

            if (user!= null)
            {
                user.Blocked = false;
                await _userService.Update(user);
            }

            GetViewBagUserInfo(iduser);

            return PartialView("_UserInfo", user);
        }

        private void GetViewBagUserInfo(int userInfo)
        {
            ViewBag.AllApp = _roadEventsService.GetAppByUser(userInfo);
            ViewBag.AcceptedRequests = _roadEventsService.GetAcceptedRequestsByUser(userInfo);
            ViewBag.RejectedRequests = _roadEventsService.GetRejectedRequestsByUser(userInfo);
        }
    }

}
