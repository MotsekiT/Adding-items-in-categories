using Firebase.Storage;
using Firebase1.Repository.DataConnection;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Firebase1.Utilities.File
{
    public class ManageFiles
    {
        private FirebaseConnect _connect;
        private Firebase.Auth.IFirebaseAuthProvider _authProvider;
        private IFirebaseClient _firebaseClient;

        public ManageFiles()
        {
            _connect = new FirebaseConnect();
            _firebaseClient = _connect.firebaseClient;
            _authProvider = _connect.authProvider;
        }
		public async Task<string> Upload(FileStream stream, string fileName)
		{
			string returnLink_error = string.Empty;
			var getUserToken = await _authProvider.SignInWithEmailAndPasswordAsync(FirebaseConstants.FromMail, FirebaseConstants.FromPsw);
			var cancellation = new CancellationTokenSource();
			var task = new FirebaseStorage(
							FirebaseConstants.Bucket,
							new FirebaseStorageOptions
							{
								AuthTokenAsyncFactory = () => Task.FromResult(getUserToken.FirebaseToken),
								ThrowOnCancel = true
							})
							.Child("images")
							.Child(fileName)
							.PutAsync(stream, cancellation.Token);
			
			try
			{
				string link = await task;
				returnLink_error = link;
			}
			catch (Exception ex)
			{
				returnLink_error = "Error occurred during Upload" + ex.ToString();
			}
			return returnLink_error;
		}


		public async Task Delete(string fileName)
		{
			string returnLink_error = string.Empty;
			var getUserToken = await _authProvider.SignInWithEmailAndPasswordAsync(FirebaseConstants.FromMail, FirebaseConstants.FromPsw);
			var cancellation = new CancellationTokenSource();
			var task = new FirebaseStorage(
							FirebaseConstants.Bucket,
							new FirebaseStorageOptions
							{
								AuthTokenAsyncFactory = () => Task.FromResult(getUserToken.FirebaseToken),
								ThrowOnCancel = true
							})
							.Child("images")
							.Child(fileName)
							.DeleteAsync();

			try
			{
				await task;
			}
			catch (Exception ex)
			{
				returnLink_error = "Error occurred during Upload" + ex.ToString();
			}

		}

	}
}