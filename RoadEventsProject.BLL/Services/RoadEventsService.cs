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


        public async Task<int> GetAcceptedRequests()
        {
            return await _roadEventsRepositorie.GetAcceptedRequests();
        }

        public async Task<int> GetRejectedRequests()
        {
            return await _roadEventsRepositorie.GetRejectedRequests();
        }

        public async Task<int> GetTotalRequests()
        {
            return await _roadEventsRepositorie.GetTotalRequests();
        }

        public async Task<int> GetUnprocessedRequests()
        {
            return await _roadEventsRepositorie.GetUnprocessedRequests();
        }

        public async Task<List<RoadEvent>> GetAppByStatus(int idStatus)
        {
            return await _roadEventsRepositorie.GetAppByStatus(idStatus);
        }

        public async Task<List<RoadEvent>> GetAppByUser(int idUser)
        {
            return await _roadEventsRepositorie.GetAppByUser(idUser);
        }

        public async Task<List<RoadEvent>> GetAppByStatusAndUser(int idStatus, int idUser)
        {
            return await _roadEventsRepositorie.GetAppByStatusAndUser(idStatus, idUser);
        }

        public async Task<List<RoadEvent>> GetAllApp()
        {
            return await _roadEventsRepositorie.GetAllApp();
        }

        public async Task<RoadEvent> GetAppById(int idUser)
        {
            return await _roadEventsRepositorie.GetAppById(idUser);
        }

        public Task<RoadEvent> Update(RoadEvent roadEvent)
        {
            return _roadEventsRepositorie.Update(roadEvent);
        }

        public async Task<int> GetTotalRequestsByUser(int idUser)
        {
            return await _roadEventsRepositorie.GetTotalRequestsByUser(idUser);
        }

        public async Task<int> GetAcceptedRequestsByUser(int idUser)
        {
            return await _roadEventsRepositorie.GetAcceptedRequestsByUser(idUser);
        }

        public async Task<int> GetRejectedRequestsByUser(int idUser)
        {
            return await _roadEventsRepositorie.GetRejectedRequestsByUser(idUser);
        }

        public async Task<List<RoadEvent>> GetAppByUserAndDate(int iduser, DateTime dateTime)
        {
            return await _roadEventsRepositorie.GetAppByUserAndDate(iduser, dateTime);
        }

        public async Task<List<RoadEvent>> GetAppByUserWithAllDetails(int iduser)
        {
            return await _roadEventsRepositorie.GetAppByUserWithAllDetails(iduser);
        }

        public async Task<RoadEvent> AddAsync(RoadEvent roadEvent)
        {
            return await _roadEventsRepositorie.AddAsync(roadEvent);
        }

        public async Task<List<Region>> GetAllRegions()
        {
            return await _roadEventsRepositorie.GetAllRegions();
        }

        public async Task<List<CityVillage>> GetCitiesVillagesByRegion(int regionId)
        {
            return await _roadEventsRepositorie.GetCitiesVillagesByRegion(regionId);
        }
    }
}
