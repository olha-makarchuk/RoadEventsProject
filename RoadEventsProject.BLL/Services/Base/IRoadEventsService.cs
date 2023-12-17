using RoadEventsProject.BLL.DTO;
using RoadEventsProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.BLL.Services.Base
{
    public interface IRoadEventsService
    {
        Task<int> GetUnprocessedRequests();

        Task<RoadEvent> GetAppById(int idUser);

        Task<RoadEvent> Update(RoadEvent roadEvent);
        Task<List<Region>> GetAllRegions();
        Task<string[]> CreateApp(Event newevent, int idUser);
        Task<List<CityVillage>> GetCitiesVillagesByRegion(int regionId);

        Task<int[]> GetAllRequestsByUser(int idUser);
        
        Task<int[]> GetStatistic();
        Task<List<RoadEvent>> GetAppByUserDateOrWithStatus(int idStatus, int idUser, DateTime dateTime);
        Task<List<RoadEvent>> GetAppByUserOrWithStatus(int idStatus, int idUser);
        Task<List<RoadEvent>> GetAppByStatusOrAllApp(int idstatus);
    }
}
