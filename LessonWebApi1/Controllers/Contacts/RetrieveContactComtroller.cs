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
    [Route("api/retrieve_contact")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RetrieveContactController : Controller
    {
        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "kATZENTJIdSNH6vxfKrYpzYrBiamvI6YJh007E8x",
            BasePath = "https://steposbb.firebaseio.com/"
        };

        IFirebaseClient client;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RetrieveOSBBData([FromBody] DataContact dataContact)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            FirebaseResponse response = await client.GetTaskAsync("OSBB/" + dataContact.name + "/Members/" + dataContact.number);
            DataContact obj = response.ResultAs<DataContact>();

            return StatusCode
                (200,
                    new
                    {
                        name = obj.name,
                        number = obj.number,
                        address = obj.address,
                        website = obj.website,
                        //data
                    }
                );
        }
    }
    public class DataContact
    {
        public string name { get; set; }
        public int number { get; set; }
        public string address { get; set; }
        public string website { get; set; }
    }
}
