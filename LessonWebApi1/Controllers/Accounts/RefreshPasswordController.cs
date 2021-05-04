using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using LessonWebApi1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LessonWebApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RefreshPasswordController : Controller
    {
        private static string ApiKey = "AIzaSyCZk_pPX5pQylzdr1_Ud78zJp8PY9tYFI4";

        // POST for tests: api/refreshpassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshPassword([FromBody] RefreshModel refreshData)
        {
            string EmailErrorMessage;
            bool isEmail = ValidateEmail(refreshData.Email, out EmailErrorMessage);
            if (isEmail == true)
            {
                try
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

                    var ab = auth.SendPasswordResetEmailAsync(refreshData.Email);

                    return StatusCode(200);
                }
                catch (Firebase.Auth.FirebaseAuthException ex)
                {
                    return StatusCode(400, new { error_desc = ex.Reason.ToString() });
                }
            }
            else
            {
                return StatusCode(400, new { error_desc = "Wrong email format." });
            }
            
        }
        public bool ValidateEmail(string email, out string ErrorMessage)
        {
            var input = email;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Email should not be empty.";
                return false;
            }

            var regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");

            if (!regex.IsMatch(input))
            {
                ErrorMessage = "Wrong Email format. Example: example@gmail.com";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    

}
