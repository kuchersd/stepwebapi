using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Web.Http.Cors;

namespace LessonWebApi1.Controllers.Apartments
{
    [Produces("application/json")]
    [Route("api/removeapartment")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RemoveApartmentController : Controller
    {

        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "kATZENTJIdSNH6vxfKrYpzYrBiamvI6YJh007E8x",
            BasePath = "https://steposbb.firebaseio.com/"
        };

        IFirebaseClient client;

        [HttpDelete]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveApartment([FromBody] APARTMENT_INFO apartment)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            FirebaseResponse response = await client.DeleteTaskAsync("OSBB/" + apartment.osbb_id + "/Apartments/" + apartment.apartment_num);
            return StatusCode
                (200,
                    new
                    {
                        removedapartment_num = apartment.apartment_num
                        //data
                    }
                );
        }

        public class APARTMENT_INFO
        {
            public string osbb_id       { get; set; }
            public int apartment_num    { get; set; }
        }
    }
}
