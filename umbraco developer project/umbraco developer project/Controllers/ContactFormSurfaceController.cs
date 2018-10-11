using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;
using umbraco_developer_project.ViewModels;
using System.Web.Mvc;
using Umbraco.Core.Models;
using System.Net.Mail;
namespace umbraco_developer_project.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            else
            {

            MailMessage message = new MailMessage();
            message.To.Add("niklasumbracotest@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message + "\n my email is " + model.Email;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("niklasumbracotest@gmail.com", "spawn4ever");
                smtp.Send(message);
            }
                TempData["success"] = true;
                IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "comment");
                comment.SetValue("email", model.Email);
                comment.SetValue("cname", model.Name);
                comment.SetValue("subject", model.Subject);
                comment.SetValue("message", model.Message);
                Services.ContentService.Save(comment);

                return RedirectToCurrentUmbracoPage();
        }

        }
    }
}