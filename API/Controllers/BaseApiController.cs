
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
[ApiController]
[Route("api/[controller]")] 
    public class BaseApiController :ControllerBase
    {
        // we are using this base class as interface so that we dont need to put that route and conrtoller again (to remove duplicate code)
    }
}