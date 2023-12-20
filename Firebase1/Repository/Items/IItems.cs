using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firebase1.Repository.Items
{
    public interface IItems
    {
        void AddItem(Firebase1.Models.Items item);
        void RemoveItem(string itemNode);
        Firebase1.Models.Items ShowItem(string itemNode);
        void EditItem(Models.Items item);
        List<Models.Items> ItemsList();
    }
}
