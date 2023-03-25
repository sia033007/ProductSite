using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopSite.Data;
using ShopSite.Models;
using System.Security.Claims;

namespace ShopSite.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<MyUser> _signInManager;
        private readonly UserManager<MyUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly AppDb _appDb;

        public UserController(IMapper mapper, SignInManager<MyUser> signInManager, UserManager<MyUser> userManager, IEmailSender emailSender, AppDb appDb)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _appDb = appDb;
        }
        public ViewResult RegisterUser() => View();
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                return View(registerModel);
            }
            var user = _mapper.Map<MyUser>(registerModel);
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData["failed"] = error.Description;
                    var r = Newtonsoft.Json.JsonConvert.SerializeObject(registerModel);
                    TempData["RegisterFailed"] = r;
                    var x = Newtonsoft.Json.JsonConvert.SerializeObject(registerModel);
                    TempData["RegisterFailed"] = x;
                    return RedirectToAction("EnterAdmin", registerModel);
                }
                return RedirectToAction("EnterAdmin");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role,"Admin")

            };
            await _userManager.AddClaimsAsync(user, claims);
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.ActionLink(action: "ConfirmEmail", controller: "User", values:
                new { userId = user.Id, token = confirmationToken });
            try
            {
                await _emailSender.SendAsync("Account Confirmation", user.Email,
                "Confirm your account", $"Click the link below to confirm your account : {confirmationLink}");
                TempData["success"] = "Check your mail inbox to confirm your account";
            }
            catch(Exception exp)
            {
                TempData["failed"] = exp.Message;
                await _userManager.DeleteAsync(user);
                return RedirectToAction("EnterAdmin");
            }
            return RedirectToAction("EnterAdmin");
        }
        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        TempData["failed"] = "کاربری با این مشخصات یافت نشد";
        //        return View();
        //    }
        //    var result = await _userManager.ConfirmEmailAsync(user, token);
        //    if (!result.Succeeded)
        //    {
        //        foreach (var error in result.Errors)
        //        {
        //            TempData["failed"] = error.Description;
        //        }
        //        return View();
        //    }
        //    TempData["success"] = "حساب کاربری با موفقیت تایید شد";
        //    return View();

        //}
        //public async Task<IActionResult> UpdateUser()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var updateModel = new UpdateUserModel
        //    {
        //        UserName = user.UserName,
        //        OldPassword = null,
        //        NewPassword = String.Empty,
        //        ConfirmNewPassword = String.Empty
        //    };
        //    return View(updateModel);
        //}
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserModel updateModel)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            user.UserName = updateModel.UserName;
            var changeResult = await _userManager.ChangePasswordAsync(user, updateModel.OldPassword, updateModel.NewPassword);
            if (!changeResult.Succeeded)
            {
                foreach(var error in changeResult.Errors)
                {
                    TempData["failed"] = error.Description;
                    return RedirectToAction("AdminPanel", "Home");
                }
            }
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach(var error in updateResult.Errors)
                {
                    TempData["failed"] = error.Description;
                    return RedirectToAction("AdminPanel", "Home");
                }
            }
            TempData["success"] = "The operation was successful";
            await _signInManager.SignOutAsync();
            return RedirectToAction("EnterAdmin");
            
        }
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password,
                loginModel.RememberMe, false);
            if (!result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);
                if (user == null)
                {
                    TempData["failed"] = "Not found such a user";
                    var s = Newtonsoft.Json.JsonConvert.SerializeObject(loginModel);
                    TempData["NotFoundUser"] = s;
                    return RedirectToAction("EnterAdmin");

                }
                else if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    TempData["failed"] = "Email not confirmed";
                    var n = Newtonsoft.Json.JsonConvert.SerializeObject(loginModel);
                    TempData["NotConfirmedEmail"] = n;
                    return RedirectToAction("EnterAdmin", loginModel);
                }
                else
                {
                    TempData["failed"] = "Wrong password";
                    var b = Newtonsoft.Json.JsonConvert.SerializeObject(loginModel);
                    TempData["WrongPassword"] = b;
                    return RedirectToAction("EnterAdmin", loginModel);
                }
            }
            TempData["success"] = "Logged in successfully";
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "Logged out successfully";
            return RedirectToAction("Index", "Home");
        }
        [Route("~/AdminCredentials/Enter")]
        public IActionResult EnterAdmin()
        {
            if (TempData["NotFoundUser"] is string s)
            {
                var loginModel = JsonConvert.DeserializeObject<LoginModel>(s);
                return View(loginModel);
                // use newUser object now as needed
            }
            else if(TempData["NotConfirmedEmail"] is string n)
            {
                var loginModel = JsonConvert.DeserializeObject<LoginModel>(n);
                return View(loginModel);
            }
            else if(TempData["WrongPassword"] is string b)
            {
                var loginModel = JsonConvert.DeserializeObject<LoginModel>(b);
                return View(loginModel);
            }
            else if(TempData["RegisterFailed"] is string r)
            {
                var registerModel = JsonConvert.DeserializeObject<RegisterModel>(r);
                return View(registerModel);
            }

            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["failed"] = "Not found such a user";
                return View();
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData["failed"] = "Something unexpected happened. Try again";
                }
                return View();
            }
            TempData["success"] = "Your account was confirmed successfully";
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user == null)
            {
                TempData["failed"] = "Not found such a user";
                return RedirectToAction("EnterAdmin","User");
            }
            try
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.ActionLink(action: "ResetPassword", controller: "User", values: new
                { userId = user.Id, token = resetToken });
                await _emailSender.SendAsync("Forgot Password", user.Email,
                    "Reset your password", $"Click the link below to reset your password : {resetLink}");
                TempData["success"] = "Password recovery email sent";
            }
            catch(Exception exp)
            {
                TempData["failed"] = "Something unexpected happened. Try again";
                return RedirectToAction("EnterAdmin", "User");
            }
            
            return RedirectToAction("EnterAdmin", "User");
        }
        public ViewResult ResetPassword(string userId, string token)
        {
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                userId = userId,
                token = token,
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };
            return View(resetPasswordViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid) return View(resetPasswordViewModel);
            var user = await _userManager.FindByIdAsync(resetPasswordViewModel.userId);
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.token,
                resetPasswordViewModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData["failed"] = error.Description;
                    return View();
                }
            }
            TempData["success"] = $"Password was reset successfully for {user.UserName}";
            return RedirectToAction("EnterAdmin");
        }
     
        public IActionResult Index()
        {
            return View();
        }
    }
}
