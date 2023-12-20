using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Firebase1.Models
{
    public class Contact
    {
        [Key]
        public string ContactId { get; set; }

        [Display(Name = "Full Name and Surname")]
        [Required(ErrorMessage = "Fullname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Your fullname may not be less than 2 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Your Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Your Message")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Response is required.")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Response")]
        public string ResponseMessage { get; set; }
    }
}