using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Firebase1.Models
{
    public class Category
    {
        [Key]

        [Display(Name = "Category Code")]
        [Required(ErrorMessage = "Category Code is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Category code may not be less than 2 characters.")]
        public string CategoryCode { get; set; }

        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name may not be less than 2 characters.")]
        public string CategoryName { get; set; }

        [Display(Name = "Category Image")]
        [Required(ErrorMessage = "Category Image is required")]
        public string CategoryImagePath { get; set; }

        public string CategoryImageName { get; set; }
    }
}