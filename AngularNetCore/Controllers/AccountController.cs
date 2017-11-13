using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AngularNetCore.Data;
using AngularNetCore.ViewModels;

namespace AngularNetCore.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager,                     
                                 ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<ResultViewModel> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return new ResultViewModel(true, "User created a new account with password.");
                }
                return new ResultViewModel(false, result.Errors.Select(x=> x.Description));

            }
            return new ResultViewModel(false, "Invalid registration informations.");
        }

        [HttpPost("[action]")]
        public async Task<ResultViewModel> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                    model.Password, model.RememberMe, lockoutOnFailure: false);

                List<string> errors = new List<string>();

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return new ResultViewModel(true, "User logged in.");
                }
                if (result.RequiresTwoFactor)
                {
                    errors.Add("Requires Two Factor Authentication.");
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    errors.Add("User account locked out.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    errors.Add("Invalid login attempt.");
                }
                return new ResultViewModel(false, errors.ToArray());

            }

            // If we got this far, something failed, redisplay form
            return new ResultViewModel(false, "An error occured".ToArray());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToPage("/Index");
        }
    }
}