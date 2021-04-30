using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Web.Http.Cors;

namespace LessonWebApi1.Controllers.Contacts
{
    [Produces("application/json")]
    [Route("api/removecontact")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RemoveContactController : Controller
    {

        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden"
        };

        IFirebaseClient client;

        [HttpDelete]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveContact([FromBody] CONTACT_INFO contact)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            FirebaseResponse response = await client.DeleteTaskAsync("OSBB/" + contact.name + "/Contact/" + contact.number);
            return StatusCode
                (200,
                    new
                    {
                        removecontact_name = contact.name
                        //data
                    }
                );
        }

        public class CONTACT_INFO
        {
            public string name { get; set; }
            public int number { get; set; }
        }
    }
}
