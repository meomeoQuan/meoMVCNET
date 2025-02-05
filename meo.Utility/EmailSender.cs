using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.ComponentModel.DataAnnotations;


namespace meo.Utility
{
    public class EmailSender : IEmailSender
    {

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = "DE180924ngoanhquan@gmail.com";
            var pass = "uvjs reiv emzl dlsk";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(mail, pass),
                EnableSsl = true,


            };
            return client.SendMailAsync(
                new MailMessage(
                    from: mail,
                    to: email,
                    subject,
                    body: htmlMessage


                    ));



        }
    }
}


