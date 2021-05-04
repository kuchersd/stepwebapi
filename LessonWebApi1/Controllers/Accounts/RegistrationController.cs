using System.Threading.Tasks;
using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//  FireSharp
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using LessonWebApi1.Models;
using System.Web.Http.Cors;
using System.Text.RegularExpressions;

namespace LessonWebApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class RegistrationController : Controller
    {
        private static string ApiKey = "AIzaSyCZk_pPX5pQylzdr1_Ud78zJp8PY9tYFI4";

        IFirebaseConfig iconfig = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "kATZENTJIdSNH6vxfKrYpzYrBiamvI6YJh007E8x",
            BasePath = "https://steposbb.firebaseio.com/"
        };

        IFirebaseClient client;

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Registration([FromBody] RegistrationModel testdata)
        {
            var model = testdata;
            // Validation.
            DataValidation validation = new DataValidation();
            string EmailErrorMessage, PasswordErrorMessage, NameErrorMessage, PhoneErrorMessage, ApartmentErrorMessage;
            bool isEmail = validation.ValidateEmail(model.Email, out EmailErrorMessage);
            bool isPassword = validation.ValidatePassword(model.Password, out PasswordErrorMessage);
            bool isName = validation.ValidateName(model.Name, out NameErrorMessage);
            bool isPhoneNum = validation.ValidatePhoneNumber(model.PhoneNumber, out PhoneErrorMessage);
            bool isApartment = validation.ValidateApartment(model.ApartmentNumber, out ApartmentErrorMessage);

            if (isEmail == true && isPassword == true && isPhoneNum == true && isName == true && isApartment == true)
            {
                try
                {
                    var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(ApiKey));
                    var a = await auth.CreateUserWithEmailAndPasswordAsync(model.Email, model.Password, model.Email, true);  // Метод реєстрації користувача через імейл.
                    var user = a.User;

                    client = new FireSharp.FirebaseClient(iconfig);

                    var data = new AccountInfoModel
                    {
                        UserLocalId = user.LocalId,
                        Name = model.Name,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        ApartmentNumber = model.ApartmentNumber
                    };

                    SetResponse response = await client.SetTaskAsync("Accounts/" + user.LocalId, data);
                    AccountInfoModel result = response.ResultAs<AccountInfoModel>();

                    return StatusCode(200);
                }
                catch (FirebaseAuthException ex)
                {
                    //Якщо спіймали екцепшн.
                    return StatusCode(400, new { error = "Email is already in use" });
                }
            }
            else
            {
                string AllErrors = string.Empty;
                if (isEmail == false)
                    AllErrors += EmailErrorMessage + " ";
                if (isPassword == false)
                    AllErrors += PasswordErrorMessage + " ";
                if (isName == false)
                    AllErrors += NameErrorMessage + " ";
                if (isPhoneNum == false)
                    AllErrors += PhoneErrorMessage + " ";
                if (isApartment == false)
                    AllErrors += ApartmentErrorMessage + " ";

                return StatusCode(400, new { error = AllErrors });
            }
        }
        // Validating Email.
        
    }
}