using RoadEventsProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.DAL.Repositories.Base
{
    public interface IPhotoVideoRepository
    {
        Task<Image> AddPhotoAsync(Image image);
        Task<Video> AddVideoAsync(Video video);
    }
}
