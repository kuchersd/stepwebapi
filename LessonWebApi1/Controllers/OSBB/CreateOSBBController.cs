using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using LessonWebApi1.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Web.Http.Cors;

namespace LessonWebApi1
{
    [Produces("application/json")]
    [Route("api/createosbb")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class CreateOSBBController : Controller
    {
        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden"
        };

        IFirebaseClient client;

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateOSBB([FromBody] OSBB osbbData)
        {
            client = new FireSharp.FirebaseClient(iconfig);

            var data = new OSBB
            {
                osbb_id = "OS_" + osbbData.osbb_director_uid,
                osbb_name = osbbData.osbb_name,
                osbb_director_uid = osbbData.osbb_director_uid,
                osbb_director_name = osbbData.osbb_director_name,
                osbb_details = osbbData.osbb_details,
                osbb_living_area = osbbData.osbb_living_area,
                osbb_additional_area = osbbData.osbb_additional_area,
                osbb_non_living_area = osbbData.osbb_non_living_area,
                osbb_num_apartments = osbbData.osbb_num_apartments
            };
            SetResponse response = await client.SetTaskAsync("OSBB/" + data.osbb_id, data);
            OSBB result = response.ResultAs<OSBB>();

            return StatusCode(200, new { created_osbb_id = data.osbb_id });
        }

        
    }
}
