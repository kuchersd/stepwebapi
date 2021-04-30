using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LessonWebApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationController : Controller
    {
        [HttpGet("")]
        [AllowAnonymous]
        public ActionResult Navigation()
        {
            return View("Navigation");
            
        }
    }
}
