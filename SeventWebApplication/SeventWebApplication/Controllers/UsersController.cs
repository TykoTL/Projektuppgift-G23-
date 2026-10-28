using Microsoft.AspNetCore.Mvc;
using SeventWebApplication.Models.Users;

namespace SeventWebApplication.Controllers
{
    // Admin-sidan för att hantera användare (/Users).
    // Bara den som har IAccountManager (Admin) kommer in hit.
    // Controllern gör inga regler själv, allt sånt (vem får ändra vad) ligger i AccountService,
    // här tar vi bara emot formulären och visar resultatet.
    public class UsersController : Controller
    {
        private readonly UserSession _session;
        private readonly AccountService _accounts;

        public UsersController(UserSession session, AccountService accounts)
        {
            _session = session;
            _accounts = accounts;
        }

        // skapar en lista med alla användare, med sök och filtrering på roll

        [HttpGet]
        public IActionResult Index(string? search, Role? role)
        {
            if (GetAdmin(out IAccountManager admin) is IActionResult denied)
                return denied;

            // Räknar bokningar per kund en gång, istället för att loopa alla bokningar för varje användare
            Dictionary<string, int> bookingsPerUser = global::FleetManager.instance.GetBookings()
                .GroupBy(b => b.UserId ?? "")
                .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

            List<User> users = _accounts.GetAllUsers(admin).ToList();

            // Sök på namn, mail eller telefon
            string? trimmed = search?.Trim();
            if (!string.IsNullOrEmpty(trimmed))
            {
                users = users.Where(u =>
                        u.Name.Contains(trimmed, StringComparison.OrdinalIgnoreCase) ||
                        u.Mail.Contains(trimmed, StringComparison.OrdinalIgnoreCase) ||
                        u.Phone.Contains(trimmed, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var model = new UserListViewModel
            {
                Search = trimmed,
                SelectedRole = role,
                CountAll = users.Count,
                CountCustomers = users.Count(u => u.Role == Role.Customer),
                CountEmployees = users.Count(u => u.Role == Role.Employee),
                CountAdmins = users.Count(u => u.Role == Role.Admin),
                Users = users
                    .Where(u => role is null || u.Role == role)
                    // admin överst, sen anställda, sen kunder. Inaktiva sist i varje grupp
                    .OrderByDescending(u => u.Role)
                    .ThenByDescending(u => u.IsActive)
                    .ThenBy(u => u.Name)
                    .Select(u => new UserRowViewModel
                    {
                        User = u,
                        BookingCount = bookingsPerUser.GetValueOrDefault(u.UserId.ToString()),
                        IsMe = u.UserId == admin.UserId
                    })
                    .ToList()
            };

            return View(model);
        }

        // Här skapas en ny användare. Admin kan skapa alla typer av användare, men får inte byta roll på sig själv (det är meningslöst).

        [HttpGet]
        public IActionResult Create()
        {
            if (GetAdmin(out _) is IActionResult denied)
                return denied;

            return View(new CreateUserForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserForm form)
        {
            if (GetAdmin(out IAccountManager admin) is IActionResult denied)
                return denied;

            try
            {
                User created = _accounts.CreateUser(admin, form.Role, form.Name, form.Mail, form.Phone, form.Password);
                TempData["Success"] = $"{created.Name} skapades som {RoleText(created.Role)}.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                // Visa felet och behåll det man skrivit, så man inte behöver börja om
                ViewBag.Error = ex.Message;
                form.Password = "";
                return View(form);
            }
        }

        // Redigerar en användare. Admin kan ändra namn, telefon och roll på alla användare utom sig själv.

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            if (GetAdmin(out IAccountManager admin) is IActionResult denied)
                return denied;

            User? user = FindUser(admin, id);
            if (user is null)
                return NotFound();

            return View(BuildEditModel(user, admin, new EditUserForm
            {
                UserId = user.UserId,
                Name = user.Name,
                Phone = user.Phone,
                Role = user.Role
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditUserForm form)
        {
            if (GetAdmin(out IAccountManager admin) is IActionResult denied)
                return denied;

            User? user = FindUser(admin, form.UserId);
            if (user is null)
                return NotFound();

            try
            {
                // Kolla namnet först, så inget hinner sparas om formuläret är fel
                if (string.IsNullOrWhiteSpace(form.Name))
                    throw new ArgumentException("Namnet får inte vara tomt.");

                // Rollen ska bara bytas om den faktiskt ändrats, annars säger AccountService nej,
                // Rollbytet görs först eftersom det skapar ett nytt objekt, sen uppdateras namn/telefon på det.
                if (form.Role != user.Role)
                    _accounts.ChangeRole(admin, form.UserId, form.Role);

                _accounts.UpdateContactInfo(admin, form.UserId, form.Name, form.Phone);

                TempData["Success"] = $"Ändringarna för {form.Name.Trim()} sparades.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                ViewBag.Error = ex.Message;
                return View(BuildEditModel(user, admin, form));
            }
        }

        // Inaktiverar eller aktiverar en användare. Admin kan inte inaktivera sig själv, och admin-kontot kan inte inaktiveras.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(Guid id)
        {
            return RunAction(admin => _accounts.Deactivate(admin, id), id, "inaktiverades");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reactivate(Guid id)
        {
            return RunAction(admin => _accounts.Reactivate(admin, id), id, "aktiverades igen");
        }


        // Kollar att den inloggade är admin. Returnerar null om allt är ok,
        // annars ett svar (skicka till inloggning, eller 403) som vi skickar tillbaka direkt.
        private IActionResult? GetAdmin(out IAccountManager admin)
        {
            admin = null!;
            User? user = _session.CurrentUser;

            if (user is null)
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path + Request.QueryString });

            if (user is not IAccountManager manager)
                return StatusCode(403);   // inloggad men inte admin

            admin = manager;
            return null;
        }

        // Letar upp en användare via AccountService (så behörighetskollen alltid görs)
        private User? FindUser(IAccountManager admin, Guid id)
        {
            return _accounts.GetAllUsers(admin).FirstOrDefault(u => u.UserId == id);
        }

        private EditUserViewModel BuildEditModel(User user, IAccountManager admin, EditUserForm form)
        {
            return new EditUserViewModel { User = user, Form = form, IsMe = user.UserId == admin.UserId };
        }

        // Gemensam kod för inaktivera/aktivera: kör grejen, visa ett meddelande, tillbaka till listan
        private IActionResult RunAction(Action<IAccountManager> action, Guid id, string doneText)
        {
            if (GetAdmin(out IAccountManager admin) is IActionResult denied)
                return denied;

            try
            {
                string name = FindUser(admin, id)?.Name ?? "Användaren";
                action(admin);
                TempData["Success"] = $"{name} {doneText}.";
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public static string RoleText(Role role) => role switch
        {
            Role.Customer => "Kund",
            Role.Employee => "Anställd",
            Role.Admin => "Admin",
            _ => role.ToString()
        };
    }
}
