
using Firebase1.Models;
using Firebase1.Repository.DataConnection;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Firebase1.Repository.Account
{
    public class AccountRepository: IAccount, IDisposable
    {

        private FirebaseConnect _connect;
        private Firebase.Auth.IFirebaseAuthProvider _authProvider;
        private IFirebaseClient _firebaseClient;

        public AccountRepository()
        {
            _connect = new FirebaseConnect();
            _firebaseClient = _connect.firebaseClient;
            _authProvider = _connect.authProvider;
        }
        public async Task SignUp(SignUp signUp)
        {
             await _authProvider.CreateUserWithEmailAndPasswordAsync(signUp.Email, signUp.Password, signUp.Name, true);
        }

        public async Task<string> Login(Models.Login login, string returnUrl, IOwinContext owinContext)
        {
            bool isAdmin = false;
            var fbAuthenticationResponse = await _authProvider.SignInWithEmailAndPasswordAsync(login.Email, login.Password);
            string token = fbAuthenticationResponse.FirebaseToken;
            var user = fbAuthenticationResponse.User;

            if (String.IsNullOrEmpty(token) == false)
            {
                var claims = new List<Claim>();
                try
                {
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                    claims.Add(new Claim(ClaimTypes.Authentication, token));
                    var claimIdentities = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    var ctx = owinContext;
                    var authenticationManager = ctx.Authentication;
                    authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, claimIdentities);
                    isAdmin = this.IsAdmin(login);
                    if(isAdmin == false)
                    {
                        return "User";
                    }
                    else
                    {
                        return "Admin";
                    }

                }
                catch 
                {
                    return "Authenticated login failed";
                }
            }
            else
            {
                return "Token generation failed";
            }

        }

        public bool IsAdmin(Firebase1.Models.Login login)
        {
     
            FirebaseResponse firebaseResponse = _firebaseClient.Get("AccessRight");
            dynamic accessRightData = JsonConvert.DeserializeObject<dynamic>(firebaseResponse.Body);
            bool isAdmin = false;
            if (accessRightData != null)
            {
                foreach (var accessRightEmail in accessRightData)
                {
                    if (login.Email.Trim() == accessRightEmail.First.Value.ToString())
                    {
                        isAdmin = true;
                    }
                }
            }
            return isAdmin;
        }

        public async Task PasswordResetLink(string EmailID)
        {
                await _authProvider.SendPasswordResetEmailAsync(EmailID);
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}