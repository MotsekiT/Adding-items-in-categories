using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase1.Models;

namespace Firebase1.Repository.Contact
{
    public interface IContact
    {
       void AddContact(Firebase1.Models.Contact contact);
       void RemoveContact(string ContactId);
       Firebase1.Models.Contact ShowContact(string ContactId);
       Task EditContact(Models.Contact contact);
       List<Models.Contact> ContactsList();


    }
}
