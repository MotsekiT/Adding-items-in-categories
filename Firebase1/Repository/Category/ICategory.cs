using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firebase1.Repository.Category
{
    public interface ICategory
    {
        void AddCategory(Firebase1.Models.Category category);
        void RemoveCategory(string categoryCode);
        Firebase1.Models.Category ShowCategory(string categoryCode);
        void EditCategory(Models.Category category);
        List<Models.Category> CategoryList();
    }
}
