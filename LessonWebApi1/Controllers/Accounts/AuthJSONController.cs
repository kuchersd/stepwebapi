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
        private static string ApiKey = "hidden";
        private const string SECRET_KEY = "hidden";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
        IFirebaseConfig iconfig = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden"
        };

        IFirebaseClient client;

        // POST: api/AuthJSON
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody]PostTest testdata)
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

                    return StatusCode(200, new { token = GetToken(obj, user.IsEmailVerified) }); 
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
                //switch (ex.Reason)
                //{
                //    case AuthErrorReason.WrongPassword:
                //        return StatusCode(400, new { error_desc = "Wrong password." });

                //    case AuthErrorReason.UnknownEmailAddress:
                //        return StatusCode(400, new { error_desc = "Unknown Email address." });

                //    case AuthErrorReason.UserNotFound:
                //        return StatusCode(400, new { error_desc = "User not found" });

                //    case AuthErrorReason.UserDisabled:
                //        return StatusCode(400, new { error_desc = "User disabled." });

                //    case AuthErrorReason.TooManyAttemptsTryLater:
                //        return StatusCode(400, new { error_desc = "Too many attempts, try later." });

                //    case AuthErrorReason.InvalidEmailAddress:
                //        return StatusCode(400, new { error_desc = "Invalid email address." });

                //    case AuthErrorReason.MissingPassword:
                //        return StatusCode(400, new { error_desc = "Missing password." });

                //    case AuthErrorReason.MissingEmail:
                //        return StatusCode(400, new { error_desc = "Missing email." });

                //    default:
                //        return StatusCode(400, new { error = "System error. Code: " + ex.Reason.ToString() });
                //}
            }

        }


        //[HttpGet]
        public object GetToken(AccountInfoModel account, bool isEmailVerified)
        {
            string key = "hidden"; //Secret key which will be used later during validation    
            var issuer = "https://stepwebapideploy.azurewebsites.net/";  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("isVerified", isEmailVerified.ToString()));
            permClaims.Add(new Claim("userLocalId", account.UserLocalId));
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Email, account.Email));
            permClaims.Add(new Claim("name", account.Name));
            permClaims.Add(new Claim("phonenum", account.PhoneNumber));
            permClaims.Add(new Claim("apartmentnum", account.ApartmentNumber));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }
    }

    public class PostTest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}
