using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Papa_Jhons.ViewModel;
using Papa_Jhons.Entities;
using Papa_Jhons.DAL;
using Papa_Jhons.Utilities;
using Microsoft.EntityFrameworkCore;
using Papa_Jhons.Services;
using System.Security.Claims;

namespace Papa_Jhons.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly PapaJhonsDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ProductService _productService;

        public AccountController(UserManager<User> usermanager, PapaJhonsDbContext context, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ProductService productService)
        {
            _usermanager = usermanager;
            _context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _productService = productService;
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

            ViewBag.Orders = _context.Orders.Include(x => x.OrderItems).Include(x => x.User).Where(x => x.UserId == user.Id).ToList();
            ViewBag.AllProduct = _productService.GetAllProducts();
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
                return RedirectToAction("Index", "Home");
            }
            IdentityResult result = await _usermanager.CreateAsync(user, account.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError message in result.Errors)
                {
                    ModelState.AddModelError("", message.Description);
                }
                return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Home");
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, account.Password, account.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Due to your efforts, our account was blocked for 5 minutes");
                }
                ModelState.AddModelError("", "Username or password is incorrect");
                return RedirectToAction("Index", "Home");
            }
            TempData["Login"] = true;
            return RedirectToAction("Index", "Home");

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
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
            }
            await _usermanager.ResetPasswordAsync(user, account.Token, account.Password);
            TempData["Security"] = true;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProfileVM profileVM)
        {
            TempData["Security"] = false;
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
            }

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
            TempData["Security"] = true;
            return RedirectToAction("index", "Home");
        }



        [HttpGet]
        public IActionResult RegisterWithGoogle(string returnUrl = null)
        {
            var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("GoogleCallback", "Account", new { returnUrl }));
            return Challenge(authenticationProperties, "Google");
        }
        [HttpGet]
        public async Task<IActionResult> GoogleCallback(string returnUrl = null)
        {
            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return RedirectToAction("Register");
            }

            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var userName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Name);

            var existingUser = await _usermanager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                await _signInManager.SignInAsync(existingUser, isPersistent: false);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            var userNameWithoutSpaces = userName.Replace(" ", string.Empty);
            var newUser = new User
            {
                FullName = userName,
                UserName = userNameWithoutSpaces.ToLower(),
                Email = email,
                EmailConfirmed = true

            };

            var result = await _usermanager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                await _usermanager.AddToRoleAsync(newUser, Roles.User.ToString());

                await _signInManager.SignInAsync(newUser, isPersistent: false);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Register");
            }
        }

        [HttpGet]
        public IActionResult RegisterWithFacebook(string returnUrl = null)
        {
            var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("FacebookCallback", "Account", new { returnUrl }));
            return Challenge(authenticationProperties, "Facebook");
        }

        [HttpGet]
        public async Task<IActionResult> FacebookCallback(string returnUrl = null)
        {
            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return RedirectToAction("Register");
            }

            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var userName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Name);

            var existingUser = await _usermanager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                await _signInManager.SignInAsync(existingUser, isPersistent: false);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            var userNameWithoutSpaces = userName.Replace(" ", string.Empty);
            var newUser = new User
            {
                FullName = userName,
                UserName = userNameWithoutSpaces.ToLower(),
                Email = email,
                EmailConfirmed = true
            };

            var result = await _usermanager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                await _usermanager.AddToRoleAsync(newUser, Roles.User.ToString());

                await _signInManager.SignInAsync(newUser, isPersistent: false);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Register");
            }
        }

    }
}
