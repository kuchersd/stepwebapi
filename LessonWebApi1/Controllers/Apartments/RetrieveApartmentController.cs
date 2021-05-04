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
    [Route("api/retrieve_apartment")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RetrieveApartmentController : Controller
    {
        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "kATZENTJIdSNH6vxfKrYpzYrBiamvI6YJh007E8x",
            BasePath = "https://steposbb.firebaseio.com/"
        };

        IFirebaseClient client;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RetrieveApartmentData([FromBody] ApartmentData apartment)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            FirebaseResponse response = await client.GetTaskAsync("OSBB/" + apartment.osbb_id + "/Apartments/" + apartment.apartment_num);
            ApartmentData obj = response.ResultAs<ApartmentData>();

            return StatusCode
               (200,
                   new
                    {
                        osbb_id = obj.osbb_id,
                        apartment_num = obj.apartment_num,
                        apartment_owner_uid = obj.apartment_owner_uid,
                        area_of_apartment = obj.area_of_apartment,
                        floor_num = obj.floor_num,
                        entrance_num = obj.entrance_num
                        //data
                    }
                );
        }
    }
    public class ApartmentData
    {
        public string osbb_id                { get; set; } 
        public int apartment_num             { get; set; }
        public string apartment_owner_uid    { get; set; }
        public int area_of_apartment         { get; set; }
        public int floor_num                 { get; set; }
        public int entrance_num              { get; set; }
    }
}
