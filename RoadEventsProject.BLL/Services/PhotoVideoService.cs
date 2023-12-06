using Microsoft.Extensions.Configuration;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.Entities;
using RoadEventsProject.DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.BLL.Services
{
    public class PhotoVideoService : IPhotoVideoService
    {
        private readonly IConfiguration _config;
        private IPhotoVideoRepository _photoVideoRepository;
        public PhotoVideoService(IConfiguration config, IPhotoVideoRepository photoVideoRepository)
        {
            _config = config;
            _photoVideoRepository = photoVideoRepository;
        }

        public async Task<Image> AddPhotoAsync(Image image)
        {
            return await _photoVideoRepository.AddPhotoAsync(image);
        }

        public async Task<Video> AddVideoAsync(Video video)
        {
            return await _photoVideoRepository.AddVideoAsync(video);
        }
    }
}
