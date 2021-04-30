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
    [Route("api/retrievservice")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class ReadDataBaseController : ControllerBase
    {

        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden"
        };

        IFirebaseClient client;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RetrieveService([FromBody] Service service)
        {
            client = new FireSharp.FirebaseClient(iconfig);

            FirebaseResponse response = await client.GetTaskAsync("OSBB/"+ service.osbb_id + "/Services/" + service.service_id);
            Service result = response.ResultAs<Service>();

            return StatusCode(200,
                    new
                    {
                        osbb_id = result.osbb_id,
                        service_id = result.service_id,
                        service_name = result.service_name,
                        service_cost = result.serivce_cost,
                        service_description = result.serivce_description,
                        service_date = result.serivce_date
                        //data
                    }
                );
        }

        
        public class Service
        {
            public string osbb_id { get; set; }
            public int service_id { get; set; }
            public string service_name { get; set; }
            public double serivce_cost { get; set; }
            public string serivce_description { get; set; }
            public DateTime serivce_date { get; set; }

        }

    }
}
