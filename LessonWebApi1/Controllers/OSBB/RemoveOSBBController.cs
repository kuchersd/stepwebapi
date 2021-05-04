using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Web.Http.Cors;
using LessonWebApi1.Models;

namespace LessonWebApi1
{
    [Produces("application/json")]
    [Route("api/removeosbb")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RemoveOSBBController : Controller
    {
        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "kATZENTJIdSNH6vxfKrYpzYrBiamvI6YJh007E8x",
            BasePath = "https://steposbb.firebaseio.com/"
        };

        IFirebaseClient client;

        [HttpDelete]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveOSBB([FromBody] OSBB_ID osbbData)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            FirebaseResponse response = await client.DeleteTaskAsync("OSBB/" + osbbData.osbb_id);

            return StatusCode(200, new { removed_osbb_id = osbbData.osbb_id });
        }
    }
}
