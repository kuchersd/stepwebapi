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
    [Route("api/removeservice")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RemoveServiceController : ControllerBase
    {

        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "kATZENTJIdSNH6vxfKrYpzYrBiamvI6YJh007E8x",
            BasePath = "https://steposbb.firebaseio.com/"
        };

        IFirebaseClient client;

        [HttpDelete]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveService([FromBody] SERVICE_INFO service)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            FirebaseResponse response = await client.DeleteTaskAsync("OSBB/" + service.osbb_id + "/Services/" + service.service_id);
            return StatusCode(200);
        }
        public class SERVICE_INFO
        {
            public string osbb_id { get; set; }
            public int service_id { get; set; }
        }
    }
}
