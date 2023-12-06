using RoadEventsProject.Models.Data;

namespace RoadEventsProject.Controllers
{
    public class ApplicationFunctionController
    {
        private readonly RoadEventContext _context;

        public ApplicationFunctionController(RoadEventContext context)
        {
            _context = context;
        }


    }
}
