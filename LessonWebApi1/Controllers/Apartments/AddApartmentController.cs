using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase;
//using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Web.Http.Cors;

namespace LessonWebApi1.Controllers.Apartments
{
    [Produces("application/json")]
    [Route("api/addapartment")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class AddApartmentController : Controller
    {

        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden"
        };

        IFirebaseClient client;

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddApartment([FromBody] Apartment apartment)
        {
            client = new FireSharp.FirebaseClient(iconfig);

            var data = new Apartment
            {
                osbb_id = apartment.osbb_id,
                apartment_num = apartment.apartment_num,
                apartment_owner_uid = apartment.apartment_owner_uid,
                area_of_apartment = apartment.area_of_apartment,
                floor_num = apartment.floor_num,
                entrance_num = apartment.entrance_num

            };
            SetResponse response = await client.SetTaskAsync("OSBB/"+ apartment.osbb_id + "/Apartments/" + data.apartment_num, data);
            Apartment result = response.ResultAs<Apartment>();

            return StatusCode
                (200,
                    new
                    {
                        new_apartment_num = apartment.apartment_num
                        //data
                    }
                );
        }

        public class Apartment
        {
            public string osbb_id              { get; set; }
            public int apartment_num           { get; set; }
            public string apartment_owner_uid  { get; set; }
            public double area_of_apartment       { get; set; }
            public int floor_num               { get; set; }
            public int entrance_num            { get; set; }
        }

    }
}
