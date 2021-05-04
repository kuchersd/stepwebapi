using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

using LessonWebApi1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Web.Http.Cors;

namespace LessonWebApi1.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class AuthJSONController : Controller
    {
        private static string ApiKey = "AIzaSyCZk_pPX5pQylzdr1_Ud78zJp8PY9tYFI4";
        private const string SECRET_KEY = "asdas13";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
        IFirebaseConfig iconfig = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "kATZENTJIdSNH6vxfKrYpzYrBiamvI6YJh007E8x",
            BasePath = "https://steposbb.firebaseio.com/"
        };

        IFirebaseClient client;

        // POST: api/AuthJSON
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] PostTest testdata)
        {
            var model = testdata;

            try
            {
                var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(ApiKey));
                var ab = await auth.SignInWithEmailAndPasswordAsync(model.Email, model.Password);

                string FbToken = ab.FirebaseToken;
                string FbRefreshToken = ab.RefreshToken;

                var user = ab.User;
                client = new FireSharp.FirebaseClient(iconfig);

                if (FbToken != "")
                {
                    FirebaseResponse response = await client.GetTaskAsync("Accounts/" + user.LocalId);
                    AccountInfoModel obj = response.ResultAs<AccountInfoModel>();

                    return StatusCode(200, new { token = FbToken });
                }
                else
                {
                    // Якщо трабли з авторизацією.
                    return StatusCode(400);
                }
            }
            catch (FirebaseAuthException ex)
            {
                // Якщо спіймали екцепшн.
                return StatusCode(400, new { error = ex.Reason.ToString() });
            }
        }
    }

    public class PostTest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}
