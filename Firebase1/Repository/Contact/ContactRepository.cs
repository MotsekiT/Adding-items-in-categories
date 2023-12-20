using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Firebase1.Repository.DataConnection;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Firebase1.Repository.Utilities;

namespace Firebase1.Repository.Contact
{
    public class ContactRepository : IContact, IDisposable
    {
        private FirebaseConnect _connect;

        private IFirebaseClient _firebaseClient;
        public ContactRepository()
        {
            _connect = new FirebaseConnect();
            _firebaseClient = _connect.firebaseClient;

        }
        public void AddContact(Models.Contact contact)
        {
            var contactData = contact;
            PushResponse pushResponse = _firebaseClient.Push("Contact/", contactData);
            contactData.ContactId = pushResponse.Result.name;
            SetResponse setResponse = _firebaseClient.Set("Contact/" + contactData.ContactId, contactData);
        }

        public List<Models.Contact> ContactsList()
        {
            FirebaseResponse firebaseResponse = _firebaseClient.Get("Contact");
            dynamic contactData = JsonConvert.DeserializeObject<dynamic>(firebaseResponse.Body);
            var contactList = new List<Models.Contact>();
            if (contactData != null)
            {
                foreach (var contact in contactData)
                {
                  contactList.Add(JsonConvert.DeserializeObject<Models.Contact>(((JProperty)contact).Value.ToString()));
                }
            }
            return contactList;
        }

        public void Dispose()
        {
            this.Dispose();
        }

        public async Task EditContact(Models.Contact contact)
        {
            string body = string.Empty;
            var root = AppDomain.CurrentDomain.BaseDirectory; 
            using (var reader = new System.IO.StreamReader(root + FirebaseConstants.ResponseMessageEmailTemplate))
            {
                string readFile = reader.ReadToEnd();
                string StrContent = string.Empty;
                StrContent = readFile;
                //Assing the field values in the template
                StrContent = StrContent.Replace("[FullName]", contact.FullName);
                StrContent = StrContent.Replace("[RespondMessage]", contact.ResponseMessage);
                body = StrContent.ToString();
            }

            MailMessage mailMessage = new MailMessage(FirebaseConstants.FromMail, "SOD316C@gmail.com");
            mailMessage.Subject = "Reference Number: " + contact.ContactId +  " - " + "Organic Foods Response";
            mailMessage.Body = body;
            await Utilities.Mail.SendEmail(mailMessage);
               
            SetResponse firebaseSetResponse = _firebaseClient.Set("Contact/" + contact.ContactId, contact);
            
        }

        public void RemoveContact(string ContactId)
        {
            FirebaseResponse firebaseResponse = _firebaseClient.Delete("Contact/" + ContactId);
        }

        public Models.Contact ShowContact(string ContactId)
        {
            FirebaseResponse firebaseResponse = _firebaseClient.Get("Contact/" + ContactId);
            Models.Contact contact = JsonConvert.DeserializeObject<Models.Contact>(firebaseResponse.Body);
            return contact;
        }
    }
}