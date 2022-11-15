using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MicroApi.Demo.Controllers
{
    [ApiController]
    [EnableCors("allow_all")]
    [Authorize]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
    }
}
