using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Firebase.Storage;
using Firebase.Auth;
using System.Threading;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LessonWebApi1.Controllers.Files
{
    [Route("api/uploadfile")]
    [ApiController]
    public class GetFileController : Controller
    {
        public static IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        IFirebaseConfig iconfig = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "hidden",
            BasePath = "hidden",
        };

        IFirebaseClient client;
        public GetFileController(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public class FileUpload
        {
            public IFormFile files { get; set; }
        }

        [HttpPost]
        public async Task<string> Post([FromForm] FileUpload objFile)
        {
            client = new FireSharp.FirebaseClient(iconfig);
            // Block 1: Uploading to wwwroot/Upload.
            try
            {
                if (objFile.files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + objFile.files.FileName))
                    {
                        objFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                        //return "\\Upload\\" + objFile.files.FileName;
                    }
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                //return ex.Message.ToString();
            }
            // Block 2 (OPTIONAL): Some actions with file.

            // 3


            // Block 3: Uploading to Firestorage. 
            //byte[] fileStream1 = System.IO.File.ReadAllBytes(_environment.WebRootPath + "\\Upload\\" + objFile.files.FileName);
            //string output = Convert.ToBase64String(fileStream1);
            //var data = new ImageModel
            //{
            //    outputImg = output
            //};
            //SetResponse response = await client.SetTaskAsync("Image/",data);
            //ImageModel result = response.ResultAs<ImageModel>();
            //--------------------------------------------------------------
            byte[] fileStream1 = System.IO.File.ReadAllBytes(_environment.WebRootPath + "\\Upload\\" + objFile.files.FileName);
            string output = Convert.ToBase64String(fileStream1);

            const string ApiKey = "AIzaSyCZk_pPX5pQylzdr1_Ud78zJp8PY9tYFI4";
            const string Bucket = "steposbb.appspot.com";
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(ApiKey));
            var ab = await auth.SignInWithEmailAndPasswordAsync("beta.steposbb@gmail.com", "321321");
            var cancelation = new CancellationToken();

            string FbToken = ab.FirebaseToken;

            var fs = new FileStream(_environment.WebRootPath + "\\Upload\\" + objFile.files.FileName, FileMode.Open);

            var storage = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(FbToken),
                    ThrowOnCancel = true
                }
                ).Child("documents")
                .Child($"{objFile.files.FileName}.{Path.GetExtension(objFile.files.FileName).Substring(1)}")
                .PutAsync(fs, cancelation);

            var apiKey = _configuration["SendGridAPIKey"];
            var client2 = new SendGridClient(apiKey);
            var from = new EmailAddress("beta.steposbb@gmail.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("kucherenko.sd@gmail.com");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            
            var banner2 = new Attachment()
            {
                Content = output,
                Type = "image/jpeg",
                Filename = "ForgotPassword.jpg",
                Disposition = "inline",
                ContentId = "Banner 2"
            };
            msg.AddAttachment(banner2);
            var response = await client2.SendEmailAsync(msg);

            return "Works!";
        }
    }
    public class ImageModel
    {
        public string outputImg { get; set; }
    }
}
