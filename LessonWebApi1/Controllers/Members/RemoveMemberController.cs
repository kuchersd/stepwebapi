using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Web.Http.Cors;

namespace LessonWebApi1.Controllers.Members
{
    [Produces("application/json")]
    [Route("api/removemember")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RemoveMemberController : Controller
    {

        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden"
        };

        IFirebaseClient client;

        [HttpDelete]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveMember([FromBody] MEMBER_INFO member)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            FirebaseResponse response = await client.DeleteTaskAsync("OSBB/" + member.osbb_id + "/Members/" + member.user_uid);
            return StatusCode
                (200,
                    new
                    {
                        removedmember_uid = member.user_uid
                        //data
                    }
                );
        }

        public class MEMBER_INFO
        {
            public string osbb_id   { get; set; }
            public string user_uid { get; set; }
        }
    }
}
