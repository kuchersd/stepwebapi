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
    [Route("api/addcontact")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class AddContactController : Controller
    {

        IFirebaseConfig iconfig = new FirebaseConfig
        {
            AuthSecret = "kATZENTJIdSNH6vxfKrYpzYrBiamvI6YJh007E8x",
            BasePath = "https://steposbb.firebaseio.com/"
        };

        IFirebaseClient client;
                                      
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddContact([FromBody] Contact contact)
        {
            client = new FireSharp.FirebaseClient(iconfig);

            var data = new Contact
            {
                name = contact.name,
                number = contact.number,
                address = contact.address,
                website = contact.website,

      
            };
            SetResponse response = await client.SetTaskAsync("OSBB/" + contact.name + "/Contacts/" + data.number, data);
            Contact result = response.ResultAs<Contact>();

            return StatusCode(200, new { new_member_id = contact.name });
        }

    }
    public class Contact
    {
        public string name { get; set; }
        public int number { get; set; }
        public string address { get; set; }
        public string website { get; set; }
    }
}
