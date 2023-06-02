using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Papa_Jhons.ViewModel;
using Papa_Jhons.Entities;
using Papa_Jhons.DAL;
using Papa_Jhons.Utilities;

namespace Papa_Jhons.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly PapaJhonsDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> usermanager, PapaJhonsDbContext context, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _usermanager = usermanager;
            _context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction(nameof(Login));
            }
            ProfileVM profileVM = new()
            {
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName
            };

            return View(profileVM);
        }




        public async Task<IActionResult> Register(RegisterVM account)
        {
            TempData["Register"] = false;
            if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());
            User user = new()
            {
                FullName = string.Concat(account.Firstname, " ", account.Lastname),
                Email = account.Email,
                UserName = account.Username
            };
            if (_usermanager.Users.Any(x => x.NormalizedEmail == account.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "Bu e-poçtda istifadəçi mövcuddur");
                return Redirect(Request.Headers["Referer"].ToString());
            }
            IdentityResult result = await _usermanager.CreateAsync(user, account.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError message in result.Errors)
                {
                    ModelState.AddModelError("", message.Description);
                }
                return Redirect(Request.Headers["Referer"].ToString());
            }



            string token = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
            string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("papajhons844@gmail.com", "PapaJhons");
            mail.To.Add(new MailAddress(user.Email));

            mail.Subject = "Verify Email";


            mail.Body = string.Empty;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/verifyemail.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{userFullName}}", user.FullName);
            mail.Body = body.Replace("{{link}}", link);
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;


            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("papajhons844@gmail.com", "lgwrwquagxyirjkm");
            smtp.Send(mail);
            await _usermanager.AddToRoleAsync(user, Roles.User.ToString());
            TempData["Register"] = true;
            return RedirectToAction("Index", "Home");

        }

        //public async Task CreateRoles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        //    await _roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
        //    await _roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        //}

        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            User user = await _usermanager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            await _usermanager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Login(LoginVm account)
        {
            TempData["Login"] = false;
            if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());

            User user = await _usermanager.FindByNameAsync(account.UserName);
            if (user is null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return Redirect(Request.Headers["Referer"].ToString());
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, account.Password, account.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Due to your efforts, our account was blocked for 5 minutes");
                }
                ModelState.AddModelError("", "Username or password is incorrect");
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["Login"] = true;
            return Redirect(Request.Headers["Referer"].ToString());

        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ForgotPassword(AccountVM account)
        {
            TempData["ForgotPassword"] = false;
            if (account.User.Email is null) return Redirect(Request.Headers["Referer"].ToString());
            User user = await _usermanager.FindByEmailAsync(account.User.Email);

            if (user is null) return Redirect(Request.Headers["Referer"].ToString());

            string token = await _usermanager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());


            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("papajhons844@gmail.com", "PapaJhons");
            mail.To.Add(new MailAddress(user.Email));

            mail.Subject = "Reset Password";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/ResetPassword.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{userFullName}}", user.FullName);
            mail.Body = body.Replace("{{link}}", link);
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;


            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("papajhons844@gmail.com", "lgwrwquagxyirjkm");

            smtp.Send(mail);
            TempData["ForgotPassword"] = true;
            return Redirect(Request.Headers["Referer"].ToString());


        }

        public async Task<IActionResult> ResetPassword(string email, string token)
        {

            User user = await _usermanager.FindByEmailAsync(email);
            if (user == null) BadRequest();

            AccountVM model = new()
            {
                User = user,
                Token = token
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(AccountVM account)
        {
            TempData["Security"] = false;
            User user = await _usermanager.FindByEmailAsync(account.User.Email);
            AccountVM model = new()
            {
                User = user,
                Token = account.Token
            };
            await _usermanager.ResetPasswordAsync(user, account.Token, account.Password);
            TempData["Security"] = true;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Details(ProfileVM profileVM)
        {
            if (!ModelState.IsValid) return View();

            User member = await _usermanager.FindByNameAsync(User.Identity.Name);


            if (!string.IsNullOrWhiteSpace(profileVM.ConfirmNewPassword) && !string.IsNullOrWhiteSpace(profileVM.NewPassword))
            {
                var passwordChangeResult = await _usermanager.ChangePasswordAsync(member, profileVM.CurrentPassword, profileVM.NewPassword);

                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var item in passwordChangeResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View();
                }

            }
            member.UserName = profileVM.UserName;
            var result = await _usermanager.UpdateAsync(member);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }


    }
}
