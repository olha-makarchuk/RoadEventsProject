using RoadEventsProject.DAL.DBContext;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.DAL.Repositories
{
    public class PhotoVideoRepository : IPhotoVideoRepository
    {
        protected readonly RoadEventsContext _context;

        public PhotoVideoRepository(RoadEventsContext context)
        {
            _context = context;
        }

        public async Task<Image> AddPhotoAsync(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<Video> AddVideoAsync(Video video)
        {
            await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();
            return video;
        }
    }
}
