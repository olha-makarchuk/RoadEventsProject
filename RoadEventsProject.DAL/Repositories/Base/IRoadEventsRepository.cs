using RoadEventsProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.DAL.Repositories.Base
{
    public interface IRoadEventsRepository
    {
        Task<int> GetTotalRequests();
        Task<int> GetAcceptedRequests();
        Task<int> GetRejectedRequests();
        Task<int> GetUnprocessedRequests();

        Task<List<RoadEvent>> GetAppByStatus(int idStatus);
        Task<List<RoadEvent>> GetAppByUser(int idUser);
        Task<List<RoadEvent>> GetAppByStatusAndUser(int idStatus, int idUser);
        Task<List<RoadEvent>> GetAllApp();
        Task<RoadEvent> GetAppById(int idUser);

        Task<RoadEvent> Update(RoadEvent roadEvent);
        Task<RoadEvent> AddAsync(RoadEvent roadEvent);


        Task<int> GetTotalRequestsByUser(int idUser);
        Task<int> GetAcceptedRequestsByUser(int idUser);
        Task<int> GetRejectedRequestsByUser(int idUser);

        Task<List<RoadEvent>> GetAppByUserAndDate(int iduser, DateTime dateTime);
        Task<List<RoadEvent>> GetAppByUserWithAllDetails(int iduser);

        Task<List<Region>> GetAllRegions();
    }
}
