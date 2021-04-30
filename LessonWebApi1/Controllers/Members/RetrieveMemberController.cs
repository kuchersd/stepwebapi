using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Web.Http.Cors;

namespace LessonWebApi1.Controllers.OSBB
{
    [Produces("application/json")]
    [Route("api/retrieve_member")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RetrieveMemberController : Controller
    {
        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden"
        };

        IFirebaseClient client;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RetrieveOSBBData([FromBody] DataMember dataMember)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            FirebaseResponse response = await client.GetTaskAsync("OSBB/" + dataMember.osbb_id + "/Members/" + dataMember.user_uid);
            DataMember obj = response.ResultAs<DataMember>();

            return StatusCode
                (200,
                    new
                    {
                        osbb_id = obj.osbb_id,
                        user_uid = obj.user_uid,
                        user_fullname = obj.user_fullname,
                        apartment = obj.apartment,
                        attitude = obj.attitude,
                        square_apartment = obj.square_apartment,
                        square_nonapartment = obj.square_nonapartment,
                        subsidy = obj.subsidy
                        //data
                    }
                ); 
        }
    }
    public class DataMember
    {
        public string osbb_id { get; set; }
        public string user_uid { get; set; }
        public string user_fullname { get; set; }
        public int apartment { get; set; }
        public string attitude { get; set; }
        public double square_apartment { get; set; }
        public double square_nonapartment { get; set; }
        public double subsidy { get; set; }
    }
}
