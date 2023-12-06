using RoadEventsProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadEventsProject.BLL.Services.Base
{
    public interface IPhotoVideoService
    {
        Task<Image> AddPhotoAsync(Image image);
        Task<Video> AddVideoAsync(Video video);
    }
}
