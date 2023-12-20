
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Firebase1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Firebase1.Repository.Account;


namespace FirebaseAuthentication.Controllers
{
    public class AccountController : Controller
    {
        private AccountRepository _accountRepository;
        public AccountController()
        {
            _accountRepository = new AccountRepository();
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(SignUp signUp)
        {
            try
            {
                await _accountRepository.SignUp(signUp);
                ModelState.AddModelError(string.Empty, "Kindly verify your email then login.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }
        // GET: Account
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            try 
            {
                if (this.Request.IsAuthenticated)
                {
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch
            { 

            }
            return this.View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(Firebase1.Models.Login login, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                IOwinContext owinContext = Request.GetOwinContext();
                string error = await _accountRepository.Login(login, returnUrl, owinContext);
                if(!String.IsNullOrEmpty(error) && (error == "Admin" || error == "User"))
                {
                    System.Web.HttpContext.Current.Session.Add("Email", login.Email);
                    if (error == "Admin")
                    {
                        System.Web.HttpContext.Current.Session.Add("AccessRight", "Admin");
                    }
                    if(error == "User")
                    {
                        System.Web.HttpContext.Current.Session.Add("AccessRight", "User");
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login credentials");
            }
            return View(login);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            System.Web.HttpContext.Current.Session.Remove("AccessRight");
            System.Web.HttpContext.Current.Session.Remove("Email");
            System.Web.HttpContext.Current.Session.Clear();
            return RedirectToAction("Index", "Home", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string EmailID)
        {
            await _accountRepository.PasswordResetLink(EmailID);
            ViewBag.Message = "Reset password link send to mail account:" + EmailID;
            return View();
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                if(Url.IsLocalUrl(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return this.RedirectToAction("LogOff", "Account");
        }
    }
}