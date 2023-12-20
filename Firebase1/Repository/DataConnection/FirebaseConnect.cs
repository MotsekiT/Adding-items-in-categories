using Firebase.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Firebase1.Repository.DataConnection
{
    public class FirebaseConnect :  IDisposable
    {
        public IFirebaseClient firebaseClient { get;}
        public IFirebaseAuthProvider authProvider { get; }

        public FirebaseConnect()
        {
            IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
            {
                AuthSecret = FirebaseConstants.AuthorizationSecret,
                BasePath = FirebaseConstants.FirebaseDatabaseAddress
            };

            firebaseClient = new FireSharp.FirebaseClient(config);

            authProvider = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(FirebaseConstants.Web_ApiKey));

        }
        public void Dispose()
        {
            this.Dispose();
        }
    }
}