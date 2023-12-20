using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Firebase1.Models
{
    public class Items
    {
        [Key]
        [Display(Name = "Item Code")]
        [Required(ErrorMessage = "Item Code is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Item code may not be less than 2 characters.")]
        public string ItemCode { get; set; }
        
        public string ItemSystemCode { get; set; }

        

        [Display(Name = "Item Name")]
        [Required(ErrorMessage = "Item Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Item name may not be less than 2 characters.")]
        public string ItemName { get; set; }

        [Display(Name = "Price (R) per Unit")]
        [Required(ErrorMessage = "Price is required")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Price can't have more than 2 decimal places")]
        public decimal ItemPrice { get; set; }

        public string ItemImageName { get; set; }

        [Display(Name = "Item Image")]
        [Required(ErrorMessage = "Item Image is required")]
        public string ItemImagePath { get; set; }

        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }

        [ReadOnly(true)]
        public IEnumerable<SelectListItem> CategoriesSelectList { get; set; }
    }
}