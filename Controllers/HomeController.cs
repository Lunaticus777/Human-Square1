using Human_Evolution.Data;
using Human_Evolution.Models;
using Human_Evolution.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Human_Evolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SmtpSettings _smtp;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IOptions<SmtpSettings> smtpOptions, ApplicationDbContext context)
        {
            _logger = logger;
            _smtp = smtpOptions.Value;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var biensRecents = await _context.Biens
                    .Where(b => b.Visible)
                    .OrderByDescending(b => b.DateAjout)
                    .Take(3)
                    .ToListAsync();

                return View("~/Views/Home/Index.cshtml", biensRecents);
            }
            catch
            {
                return View("~/Views/Home/Index.cshtml", new List<Human_Evolution.Models.Bien>());
            }
        }

        public IActionResult About() => View();
        public IActionResult Services() => View();
        public IActionResult Projects() => RedirectToAction("Index", "Projects");
        public IActionResult Contact() => View();
        public IActionResult Privacy() => View("~/Views/Home/Privacy.cshtml");

        [HttpPost]
        public IActionResult ServicesContact(ServicesContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = $"Nom: {model.FullName}\nEmail: {model.Email}\n\nDomaines sélectionnés:\n{model.SelectedDomains}\n\nServices sélectionnés:\n{model.SelectedServices}\n\nMessage:\n{model.Message}";
                    var mail = new MailMessage
                    {
                        From = new MailAddress(_smtp.From),
                        Subject = "Demande via Services & Formations",
                        Body = body,
                        IsBodyHtml = false
                    };
                    mail.To.Add("admin@human-square.com");
                    using var smtpClient = new SmtpClient(_smtp.Host, _smtp.Port)
                    {
                        Credentials = new NetworkCredential(_smtp.User, _smtp.Password),
                        EnableSsl = _smtp.EnableSsl
                    };
                    smtpClient.Send(mail);
                    TempData["SuccessMessage"] = "Votre message a bien été envoyé.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Erreur lors de l'envoi : {ex.Message}";
                }
            }
            return RedirectToAction("Services");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl = null)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
            );
            return LocalRedirect(returnUrl ?? "/");
        }
    }
}