using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Authorization;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Hosting;

namespace LessonWebApi1.Controllers.Email
{
    [Produces("application/json")]
    [Route("api/sendemail")]
    [ApiController]
    [EnableCors(origins: "https://steposbbwebapi.azurewebsites.net", headers: "*", methods: "*")]
    public class SendEmail : Controller
    {
        private readonly IConfiguration _configuration;

        public SendEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendMail([FromBody] Receiver receiver)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("beta.steposbb@gmail.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(receiver.Email);
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //msg.SetFooterSetting(
            //    true,
            //    "Some Footer HTML",
            //    "<strong>Some Footer Text</strong>");

            var response = await client.SendEmailAsync(msg);

            return StatusCode(200);
        }
    }

    public class Receiver
    {
        public string Email { get; set; }
    }
}
