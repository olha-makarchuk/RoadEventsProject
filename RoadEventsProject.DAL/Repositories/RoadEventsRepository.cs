using Microsoft.EntityFrameworkCore;
using RoadEventsProject.DAL.DBContext;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RoadEventsProject.DAL.Repositories
{
    public class RoadEventsRepository : IRoadEventsRepository
    {
        protected readonly RoadEventsContext _context;

        public RoadEventsRepository(RoadEventsContext context)
        {
            _context = context;
        }

        public async Task<RoadEvent> AddAsync(RoadEvent roadEvent)
        {
            await _context.RoadEvents.AddAsync(roadEvent);
            await _context.SaveChangesAsync();
            return roadEvent;
        }

        public async Task<int> GetAcceptedRequests()
        {
            return await _context.RoadEvents.CountAsync(r => r.IdStatus == 2);
        }


        public async Task<List<RoadEvent>> GetAllApp()
        {
            var events = await _context.RoadEvents
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .Include(u => u.IdVideoNavigation)
                .Include(u => u.IdImageNavigation)
                .OrderByDescending(u => u.DateEvent)
                .ToListAsync();
            return events;
        }

        public async Task<List<Region>> GetAllRegions()
        {
            return await _context.Regions.ToListAsync();
        }
        public async Task<List<CityVillage>> GetCitiesVillagesByRegion(int regionId)
        {
            return await _context.CityVillages.Where(c => c.IdRegion == regionId).ToListAsync();
        }

        public async Task<RoadEvent> GetAppById(int idUser)
        {
            var roadEvent = await _context.RoadEvents
                .Where(a => a.IdRoadEvent == idUser)
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .FirstOrDefaultAsync();
            return roadEvent;
        }

        public async Task<List<RoadEvent>> GetAppByStatus(int idStatus)
        {
            var events = await _context.RoadEvents
                .Where(u => u.IdStatus == idStatus)
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .Include(u => u.IdVideoNavigation)
                .Include(u => u.IdImageNavigation)
                .Include(re => re.IdStatusNavigation)
                .OrderByDescending(u => u.DateEvent)
                .ToListAsync();
            return events;
        }

        public async Task<List<RoadEvent>> GetAppByStatusAndUser(int idStatus, int idUser)
        {
            var events = await _context.RoadEvents
                .Where(u => u.IdStatus == idStatus)
                .Where(u => u.IdUser == idUser)
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .Include(u => u.IdVideoNavigation)
                .Include(u => u.IdImageNavigation)
                .Include(re => re.IdStatusNavigation)
                .OrderByDescending(u => u.DateEvent)
                .ToListAsync();
            return events;
        }

        public async Task<List<RoadEvent>> GetAppByStatusUserDate(int idStatus, int idUser, DateTime dateTime)
        {
            var events = await _context.RoadEvents
                .Where(u => u.IdStatus == idStatus)
                .Where(u => u.IdUser == idUser)
                .Where(re => re.DateEvent.Date.Equals(dateTime))
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .Include(u => u.IdVideoNavigation)
                .Include(re => re.IdStatusNavigation)
                .Include(u => u.IdImageNavigation)
                .OrderByDescending(u => u.DateEvent)
                .ToListAsync();
            return events;
        }

        public async Task<List<RoadEvent>> GetAppByUser(int idUser)
        {
            var events = await _context.RoadEvents
                .Where(u => u.IdUser == idUser)
                .Include(u => u.IdCityVillageNavigation.IdRegionNavigation)
                .Include(u => u.IdVideoNavigation)
                .Include(re => re.IdStatusNavigation)
                .Include(u => u.IdImageNavigation)
                .OrderByDescending(u => u.DateEvent)
                .ToListAsync();
            return events;
        }

        public async Task<List<RoadEvent>> GetAppByUserAndDate(int iduser, DateTime dateTime)
        {
            return await _context.RoadEvents
                .Where(re => re.IdUser == iduser)
                .Where(re => re.DateEvent.Date.Equals(dateTime))
                .Include(re => re.IdCityVillageNavigation)
                .Include(re => re.IdImageNavigation)
                .Include(re => re.IdStatusNavigation)
                .Include(re => re.IdUserNavigation)
                .Include(re => re.IdVideoNavigation)
                .ToListAsync();
        }

        public async Task<List<RoadEvent>> GetAppByUserWithAllDetails(int iduser)
        {
            return await _context.RoadEvents
                .Where(re => re.IdUser == iduser)
                .Include(re => re.IdCityVillageNavigation)
                .Include(re => re.IdImageNavigation)
                .Include(re => re.IdStatusNavigation)
                .Include(re => re.IdUserNavigation)
                .Include(re => re.IdVideoNavigation)
                .ToListAsync();
        }

        public async Task<int> GetRejectedRequests()
        {
            return await _context.RoadEvents.CountAsync(r => r.IdStatus == 3);
        }

        public async Task<int> GetRejectedRequestsByUser(int idUser)
        {
            return await _context.RoadEvents.Where(r => r.IdUser == idUser).CountAsync(r => r.IdStatus == 3);
        }
        public async Task<int> GetAcceptedRequestsByUser(int idUser)
        {
            return await _context.RoadEvents.Where(r => r.IdUser == idUser).CountAsync(r => r.IdStatus == 2);
        }

        public async Task<int> GetTotalRequests()
        {
            return await _context.RoadEvents.CountAsync();
        }

        public async Task<int> GetTotalRequestsByUser(int idUser)
        {
            return await _context.RoadEvents.Where(r => r.IdUser == idUser).CountAsync();
        }

        public async Task<int> GetUnprocessedRequests()
        {
            return await _context.RoadEvents.CountAsync(r => r.IdStatus == 1);
        }

        public async Task<RoadEvent> Update(RoadEvent roadEvent)
        {
            _context.RoadEvents.Update(roadEvent);
            await _context.SaveChangesAsync();
            return roadEvent;
        }

        public async Task<int[]> GetAllRequestsByUser(int idUser)
        {
            int[] arr = new int[3];

            using (var context = new RoadEventsContext())
            {
                var result = await _context.RoadEvents
    .Where(r => r.IdUser == idUser)
    .GroupBy(r => r.IdStatus)
    .Select(g => new { Status = g.Key, Count = g.Count() })
    .ToListAsync();

                foreach (var item in result)
                {
                    if (item.Status == 2)
                        arr[1] = item.Count;
                    else if (item.Status == 3)
                        arr[2] = item.Count;
                    else if (item.Status == 1)
                        arr[0] = item.Count;
                }

                arr[0] = arr[0] + arr[1] + arr[2];
            }

            return arr;
        }
    }
}
