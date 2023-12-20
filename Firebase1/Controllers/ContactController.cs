using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Firebase1.Models;
using Firebase1.Repository.Contact;
using Firebase1.Repository.DataConnection;
using FireSharp.Interfaces;

namespace Firebase1.Controllers
{
    public class ContactController : Controller
    {

        private ContactRepository _contactRepository;

        public ContactController()
        {
            _contactRepository = new ContactRepository();
        }

        // GET: Contact
        public ActionResult Index()
        {
           return View(_contactRepository.ContactsList());   
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Contact contact)
        {
            try
            {
                if (string.IsNullOrEmpty(contact.Message) == false)
                {
                    _contactRepository.AddContact(contact);
                    ModelState.AddModelError(string.Empty, "Your message has been sent, we will be in contact soon.");
                }
                else
                { 
                    ModelState.AddModelError(string.Empty, "Kindly enter a message, then click send message."); 
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Your message was not sent. Kindly view the contact information.");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_contactRepository.ShowContact(id));
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id) == false)
            {
                _contactRepository.RemoveContact(id);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Contact cannot be found.");
                return RedirectToAction("Index", "Contact");
            }
            return RedirectToAction("Index", "Contact");
        }

        public ActionResult Edit (string id)
        {
             return View(_contactRepository.ShowContact(id));
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Contact contact)
        {
            try
            {
               await _contactRepository.EditContact(contact);
                ModelState.AddModelError(string.Empty, "Your message has been sent, we will be in contact soon.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Your message was not sent. Kindly view the contact information.");
            }
            return RedirectToAction("Index","Contact");
        }
    }
}