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

namespace LessonWebApi1.Controllers.Services
{
    [Produces("application/json")]
    [Route("api/addservice")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class AddServiceController : ControllerBase
    {

        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "kATZENTJIdSNH6vxfKrYpzYrBiamvI6YJh007E8x",
            BasePath = "https://steposbb.firebaseio.com/"
        };

        IFirebaseClient client;

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddService([FromBody] Service service)
        {
            client = new FireSharp.FirebaseClient(iconfig);

            var data = new Service
            {
                osbb_id = service.osbb_id,
                service_id = service.service_id,
                service_name = service.service_name,
                service_cost = service.service_cost,
                serivce_description = service.serivce_description,
                serivce_date = DateTime.Now
            };
            SetResponse response = await client.SetTaskAsync("OSBB/"+ service.osbb_id + "/Services/" + data.service_id, data);
            Service result = response.ResultAs<Service>();

            return StatusCode(200);
        }

        
        public class Service
        {
            public string osbb_id { get; set; }
            public int service_id { get; set; }
            public string service_name { get; set; }
            public double service_cost { get; set; }
            public string serivce_description { get; set; }
            public DateTime serivce_date { get; set; }

        }

    }
}
