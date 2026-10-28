using Microsoft.AspNetCore.Mvc;
using SeventWebApplication.Models.Users;

namespace SeventWebApplication.Controllers
{
    // Tar emot det man gör på inloggningssidan (logga in, registrera, logga ut)
    // och skickar vidare till UserSession/AuthService. Controller i GRASP alltså.
    public class AccountController : Controller
    {
        private readonly UserSession _session;
        private readonly AuthService _auth;

        public AccountController(UserSession session, AuthService auth)
        {
            _session = session;
            _auth = auth;
        }

        // returnUrl = sidan man försökte gå till innan man blev skickad hit (t.ex. /Bookings)
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string mail, string password, string? returnUrl)
        {
            try
            {
                _session.Login(mail, password);

                // Tillbaka dit man var på väg. IsLocalUrl så ingen kan skicka iväg folk
                // till en annan sida via länken
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException ex)
            {
                // Visa felet på sidan istället för att allt kraschar
                ViewBag.Error = ex.Message;
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(string name, string mail, string phone, string password)
        {
            try
            {
                _auth.Register(name, mail, phone, password);
                _session.Login(mail, password);   // loggar in direkt så man slipper skriva allt igen
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                ViewBag.Error = ex.Message;
                return View("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _session.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
