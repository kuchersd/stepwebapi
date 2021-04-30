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
    [Route("api/retrieveosbb")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RetrieveController : Controller
    {
        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden"
        };

        IFirebaseClient client;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RetrieveOSBBData([FromBody] OSBB dataOSBB)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            FirebaseResponse response = await client.GetTaskAsync("OSBB/" + dataOSBB.osbb_id);
            OSBB obj = response.ResultAs<OSBB>();

            return StatusCode(200, new
                    {
                        osbb_id = obj.osbb_id,
                        osbb_name = obj.osbb_name,
                        osbb_director_uid = obj.osbb_director_uid,
                        osbb_director_name = obj.osbb_director_name,
                        osbb_details = obj.osbb_details,
                        osbb_living_area = obj.osbb_living_area,
                        osbb_additional_area = obj.osbb_additional_area,
                        osbb_non_living_area = obj.osbb_non_living_area,
                        osbb_num_apartments = obj.osbb_num_apartments
                //data
            }
          );
        }
    }
}
