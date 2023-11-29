using BLL.ViewModel;
using BLL.ViewModel.Account;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.Helper;
using PL.Models;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSettings emailSett;
		private readonly ISmsService smsService;

		public AccountController(UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager, 
            IEmailSettings _emailSettings, 
            ISmsService _smsService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            emailSett = _emailSettings;
			smsService = _smsService;
		}

        public IActionResult SignUp()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel registerViewModel)
        {
            registerViewModel.IsAgree = true;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email.Split('@')[0],
                    IsAgree = registerViewModel.IsAgree
                };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("SignIn");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            }
            return View(registerViewModel);
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> SignIn(LoginViewModel loginViewModel)
        {
            var user = await userManager.FindByEmailAsync(loginViewModel.Email);
            if (user is not null)
            {
                var password = await userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (password)
                {
                    var res = await signInManager.PasswordSignInAsync(user,
                        loginViewModel.Password, loginViewModel.RememberMe, false);

                    if (res.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "invalid password");

                }

                ModelState.AddModelError(string.Empty, "Invalid Email");

            }

            return View(loginViewModel);
        }



        public async Task<IActionResult> SignOuT()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("SignIn");

        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {


                var user = await userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if (user is not null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordLink = Url.Action("ResetPassword","Account",new
                        { email = forgetPasswordViewModel.Email, token =token },Request.Scheme);
                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        body = resetPasswordLink,
                        to = forgetPasswordViewModel.Email
                    };

                    //EmailSettings.SendEmail(email);
                    emailSett.SendEmail(email);
                    return RedirectToAction("CompleteResetPassword");

                }
                ModelState.AddModelError(string.Empty, "Invalid Email");

            }
            return View(forgetPasswordViewModel);
        }

        public IActionResult ResetPassword(string email, string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(resetPasswordViewModel.Email);
                if(user is not null)
                {
                    var res = await userManager.ResetPasswordAsync(user, 
                        resetPasswordViewModel.Token, resetPasswordViewModel.Password);
                    if (res.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordDone");
                    }

                    foreach (var error in res.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                }
            }
            return View(resetPasswordViewModel);
        }

		[HttpPost]
		public async Task<IActionResult> SendSms(ForgetPasswordViewModel forgetPasswordViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
				if (user is not null)
				{
					var token = await userManager.GeneratePasswordResetTokenAsync(user);
					var resetPasswordLink = Url.Action("ResetPassword", "Account", new
					{ email = forgetPasswordViewModel.Email, token = token }, Request.Scheme);
					var sms = new SmsMessage()
					{
                        PhoneNumber = user.PhoneNumber,
                        body = resetPasswordLink

					};

					//EmailSettings.SendEmail(email);
					smsService.SendSms(sms);

					return Ok("check your phone");

				}
				ModelState.AddModelError(string.Empty, "Invalid Email");

			}
			return View(forgetPasswordViewModel);
		}
		public IActionResult ResetPasswordDone()
        {
            return View();
        }

        public IActionResult CompleteResetPassword()
        {
            return View();
        }
    }
}
    