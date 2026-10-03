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
        private readonly MailService _mailService;

        public HomeController(ILogger<HomeController> logger, IOptions<SmtpSettings> smtpOptions, ApplicationDbContext context, MailService mailService)
        {
            _logger = logger;
            _smtp = smtpOptions.Value;
            _context = context;
            _mailService = mailService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var biens = await _context.Biens
                    .Where(b => b.Visible)
                    .OrderByDescending(b => b.DateAjout)
                    .Take(3)
                    .ToListAsync();

                if (!biens.Any())
                    biens = BiensController.GetBiensStatiques().Take(3).ToList();

                return View("~/Views/Home/Index.cshtml", biens);
            }
            catch
            {
                var biens = BiensController.GetBiensStatiques().Take(3).ToList();
                return View("~/Views/Home/Index.cshtml", biens);
            }
        }

        public IActionResult About() => View();
        public IActionResult Services() => View();
        public IActionResult Projects() => RedirectToAction("Index", "Projects");
        public IActionResult Contact() => View();
        public IActionResult Privacy() => View("~/Views/Home/Privacy.cshtml");

        [HttpPost]
        public async Task<IActionResult> ServicesContact(ServicesContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Func<string?, string?> enc = System.Net.WebUtility.HtmlEncode;
                    var body = $"<p><strong>Nom :</strong> {enc(model.FullName)}</p>" +
                               $"<p><strong>Email :</strong> {enc(model.Email)}</p>" +
                               $"<p><strong>Domaines sélectionnés :</strong><br>{enc(model.SelectedDomains)}</p>" +
                               $"<p><strong>Services sélectionnés :</strong><br>{enc(model.SelectedServices)}</p>" +
                               $"<p><strong>Message :</strong><br>{enc(model.Message)}</p>";
                    await _mailService.SendEmailAsync("Demande via Services & Formations", body, model.Email);
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