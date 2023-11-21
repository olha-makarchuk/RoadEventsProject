using RoadEventsProject.Models.Data;

namespace RoadEventsProject.Controllers
{
    public class ApplicationFunctionController
    {
        private readonly RoadEventsContext _context;

        public ApplicationFunctionController(RoadEventsContext context)
        {
            _context = context;
        }


    }
}
