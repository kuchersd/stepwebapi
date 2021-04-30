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
    [Route("api/addmember")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class AddMemberController : Controller
    {

        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden"
        };

        IFirebaseClient client;

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddMember([FromBody] Member member)
        {
            client = new FireSharp.FirebaseClient(iconfig);

            var data = new Member
            {
                osbb_id = member.osbb_id,
                user_uid = member.user_uid,
                user_fullname = member.user_fullname,
                apartment = member.apartment,
                attitude = member.attitude,
                square_apartment = member.square_apartment,
                square_nonapartment = member.square_nonapartment,
                subsidy = member.subsidy,

            };
            SetResponse response = await client.SetTaskAsync("OSBB/" + member.osbb_id + "/Members/" + data.user_uid, data);
            Member result = response.ResultAs<Member>();

            return StatusCode(200, new { new_member_id = member.user_uid });
        }


        public class Member
        {
            public string osbb_id          { get; set; }
            public string user_uid         { get; set; }
            public string user_fullname    { get; set; }

            public int apartment           { get; set; }
            public string attitude         { get; set; }
            public double square_apartment    { get; set; }
            public double square_nonapartment { get; set; }
            public double subsidy             { get; set; }
        }

    }
}
