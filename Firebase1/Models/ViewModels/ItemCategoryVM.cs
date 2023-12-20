using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Firebase1.Models.ViewModels
{
    public class ItemCategoryVM
    {
        public Items Item { get; set; }
        public string SelectedCategory { get;set; }
        public IEnumerable<SelectListItem> CategoriesSelectList { get; set; }
        
    }
}