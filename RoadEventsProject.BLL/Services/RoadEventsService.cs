using Microsoft.EntityFrameworkCore;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.DBContext;
using RoadEventsProject.DAL.Repositories.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadEventsProject.DAL.Entities;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using RoadEventsProject.BLL.DTO;
using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace RoadEventsProject.BLL.Services
{
    public class RoadEventsService : IRoadEventsService
    {
        private IRoadEventsRepository _roadEventsRepositorie;
        private readonly IConfiguration _config;

        public RoadEventsService(IRoadEventsRepository roadEventsRepositorie, IConfiguration config)
        {
            _roadEventsRepositorie = roadEventsRepositorie;
            _config = config;
        }

        public async Task<string[]> CreateApp(Event newevent, int idUser)
        {
            string[] arr = new string[3];
            GoogleDrive googleDrive = new();

            RoadEvent roadEvent = new RoadEvent();
            

            roadEvent.IdUser = idUser;
            roadEvent.IdStatus = 1;
            roadEvent.IdCityVillage = newevent.IdCityVillage;
            roadEvent.DateEvent = newevent.DateEvent;
            roadEvent.DescriptionEvent = newevent.DescriptionEvent;

            await _roadEventsRepositorie.AddAsync(roadEvent);

            arr[0] = roadEvent.IdRoadEvent.ToString();

            if (newevent.Photo != null)
            {
                string fileName = $"{idUser}_{roadEvent.IdRoadEvent}";
                string link = await googleDrive.UploadAsync(fileName, newevent.Photo, ".jpeg", "image/jpeg");
                arr[1] = link;
            }
            if (newevent.Video != null)
            {
                string fileName = $"{idUser}_{roadEvent.IdRoadEvent}";
                string link = await googleDrive.UploadAsync(fileName, newevent.Video, ".mp4", "video/mp4");
                arr[2] = link;
            }
            return arr;
        }

        public async Task<int> GetUnprocessedRequests()
        {
            return await _roadEventsRepositorie.GetUnprocessedRequests();
        }

        public async Task<List<RoadEvent>> GetAppByUser(int idUser)
        {
            return await _roadEventsRepositorie.GetAppByUser(idUser);
        }

        public async Task<RoadEvent> GetAppById(int idUser)
        {
            return await _roadEventsRepositorie.GetAppById(idUser);
        }

        public Task<RoadEvent> Update(RoadEvent roadEvent)
        {
            return _roadEventsRepositorie.Update(roadEvent);
        }

        public async Task<List<Region>> GetAllRegions()
        {
            return await _roadEventsRepositorie.GetAllRegions();
        }

        public async Task<List<CityVillage>> GetCitiesVillagesByRegion(int regionId)
        {
            return await _roadEventsRepositorie.GetCitiesVillagesByRegion(regionId);
        }

        public async Task<int[]> GetAllRequestsByUser(int idUser)
        {
            return await _roadEventsRepositorie.GetAllRequestsByUser(idUser);
        }


        /////////////////////
        public async Task<int[]> GetStatistic()
        {
            int[] arr= new int[3];
            arr[0] = await _roadEventsRepositorie.GetTotalRequests();
            arr[1] = await _roadEventsRepositorie.GetAcceptedRequests();
            arr[2] = await _roadEventsRepositorie.GetRejectedRequests();
            return arr;
        }

        public async Task<List<RoadEvent>> GetAppByUserDateOrWithStatus(int idStatus, int idUser, DateTime dateTime)
        {
            List<RoadEvent> applications = new();
            if (idStatus != 0)
            {
                applications = await _roadEventsRepositorie.GetAppByStatusUserDate(idStatus, idUser, dateTime);
            }
            else
            {
                applications = await _roadEventsRepositorie.GetAppByUserAndDate(idUser, dateTime);
            }
            return applications;
        }

        public async Task<List<RoadEvent>> GetAppByUserOrWithStatus(int idStatus, int iduser)
        {
            List<RoadEvent> applications = new();
            if (idStatus != 0)
            {
                applications = await _roadEventsRepositorie.GetAppByStatusAndUser(idStatus, iduser);
            }
            else
            {
                applications = await _roadEventsRepositorie.GetAppByUserWithAllDetails(iduser);
            }
            return applications;
        }

        public async Task<List<RoadEvent>> GetAppByStatusOrAllApp(int idstatus)
        {
            List<RoadEvent> events = new();
            if (idstatus != 0)
            {
                events = await _roadEventsRepositorie.GetAppByStatus(idstatus);
            }
            else
            {
                events = await _roadEventsRepositorie.GetAllApp();
            }
            return events;
        }
    }
}
