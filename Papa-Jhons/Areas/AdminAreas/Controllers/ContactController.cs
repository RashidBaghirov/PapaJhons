using Microsoft.AspNetCore.Mvc;
using Papa_Jhons.DAL;
using Papa_Jhons.Entities;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Papa_Jhons.Utilities;

namespace Papa_Jhons.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ContactController : Controller
    {
        private readonly PapaJhonsDbContext _context;

        public ContactController(PapaJhonsDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<ContactUs> messages = _context.Contact.ToList();
            return View(messages);
        }

        public IActionResult Replace(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            ContactUs contact = _context.Contact.FirstOrDefault(x => x.Id == id);
            if (contact is null) return Redirect("~/Error/Error");
            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Replace(int id, string replace)
        {
            if (id == 0) return Redirect("~/Error/Error");
            ContactUs contact = _context.Contact.FirstOrDefault(x => x.Id == id);
            if (contact is null) return Redirect("~/Error/Error");
            MailMessage message = new MailMessage();
            message.From = new MailAddress("papajhons844@gmail.com", "PapaJhons");
            message.To.Add(new MailAddress(contact.Email));
            message.Subject = "PapaJhons Support";
            message.Body = string.Empty;
            message.Body = $"{replace}";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential("papajhons844@gmail.com", "lgwrwquagxyirjkm");
            smtpClient.Send(message);
            contact.IsReply = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            ContactUs contact = _context.Contact.FirstOrDefault(x => x.Id == id);
            if (contact is null) return Redirect("~/Error/Error");
            _context.Contact.Remove(contact);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
