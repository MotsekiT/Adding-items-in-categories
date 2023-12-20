using Firebase1.Repository.Category;
using Firebase1.Repository.DataConnection;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Firebase1.Repository.Items
{
    public class ItemsRepository : IItems, IDisposable
    {
        private FirebaseConnect _connect;
        private Firebase.Auth.IFirebaseAuthProvider _authProvider;
        private IFirebaseClient _firebaseClient;
        private CategoryRepository _categoryRepository;
        public ItemsRepository()
        {
            _connect = new FirebaseConnect();
            _firebaseClient = _connect.firebaseClient;
            _authProvider = _connect.authProvider;
            _categoryRepository = new CategoryRepository();
        }
        public void AddItem(Models.Items item)
        {
            var itemData = item;
            PushResponse pushResponse = _firebaseClient.Push(itemData.SelectedCategory + "/Items/", itemData);
            itemData.ItemSystemCode = pushResponse.Result.name;
            SetResponse setResponse = _firebaseClient.Set(itemData.SelectedCategory + "/Items/" + itemData.ItemSystemCode, itemData);
        }



        public void EditItem(Models.Items item)
        {
            var formatItem = new { item.ItemSystemCode, item.ItemCode, item.ItemName, item.ItemPrice, item.ItemImageName, item.ItemImagePath, item.SelectedCategory };
            SetResponse firebaseSetResponse = _firebaseClient.Set(formatItem.SelectedCategory +"/Items/" + formatItem.ItemSystemCode, formatItem);
        }

        public Models.Items Serialize(object obj)
        {
            dynamic convertedObject;
            Models.Items convertedItem = new Models.Items();
            convertedObject =  System.Text.Json.JsonSerializer.Serialize(obj,
                   new JsonSerializerOptions
                   {
                       IgnoreReadOnlyProperties = true
                   });
            convertedItem = JsonConvert.DeserializeObject<Models.Items>(convertedObject);
            return convertedItem;
        }

        public List<Models.Items> ItemsList()
        {
            //setup the enumerable list categories
            Models.Items currentItem = new Models.Items();

            List<Models.Category> categories = _categoryRepository.CategoryList().ToList();

            currentItem.CategoriesSelectList = categories.Select(i => new SelectListItem
            {
                Text = i.CategoryCode,
                Value = i.CategoryCode
            });

            //create a list that will hold all the items of the given categories. 
            var itemsList = new List<Models.Items>();

            //get the various category items
            foreach (Models.Category category in categories)
            {
                FirebaseResponse firebaseCategoryItems = _firebaseClient.Get(category.CategoryCode + "/Items");
                //format the retrieved data into manageable data 
                dynamic categoryItems = JsonConvert.DeserializeObject<dynamic>(firebaseCategoryItems.Body);
                //loop through the manageable data and add the item to the list
                if (categoryItems != null)
                {
                    foreach (var item in categoryItems)
                    {
                        currentItem = JsonConvert.DeserializeObject<Models.Items>(((JProperty)item).Value.ToString());
                        itemsList.Add(currentItem);
                    }
                }
            }

            //sort the items by category then by name
            var sortItemsList = itemsList.OrderBy(byCatgory => byCatgory.SelectedCategory).ThenBy(byItemName => byItemName.ItemName);

            return sortItemsList.ToList();
        }

        public void RemoveItem(string itemNode)
        {
            Models.Items currentItem = this.ShowItem(itemNode);
            if (currentItem != null)
            {
                FirebaseResponse firebaseResponse = _firebaseClient.Delete(currentItem.SelectedCategory + "/Items/" + itemNode);
            }
        }

        public Models.Items ShowItem(string itemNode)
        {
            List<Models.Category> categories = _categoryRepository.CategoryList().ToList();

            FirebaseResponse firebaseResponse;
            Models.Items item = new Models.Items();
            //loop through each category to build the node for items to gain the record associated with the node.
            foreach (Models.Category category in categories)
            {
                firebaseResponse = _firebaseClient.Get(category.CategoryCode + "/Items/" + itemNode);
                if(firebaseResponse.Body.Trim() != "null")
                {
                   item  = JsonConvert.DeserializeObject<Models.Items>(firebaseResponse.Body);
                   item.CategoriesSelectList = categories.Select(i => new SelectListItem
                    {
                        Text = i.CategoryCode,
                        Value = i.CategoryCode
                    });
                }

            }

            return item;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}