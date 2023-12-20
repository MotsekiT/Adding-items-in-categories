using Firebase1.Repository.DataConnection;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Firebase1.Repository.Category
{
    public class CategoryRepository
    {
        private FirebaseConnect _connect;
        private Firebase.Auth.IFirebaseAuthProvider _authProvider;
        private IFirebaseClient _firebaseClient;

        public CategoryRepository()
        {
            _connect = new FirebaseConnect();
            _firebaseClient = _connect.firebaseClient;
            _authProvider = _connect.authProvider;
        }

        public void AddCategory(Models.Category category)
        {
            var categoryData = category;
            //PushResponse pushResponse = _firebaseClient.Push("Category/", categoryData);
            //categoryData.CategoryNode = pushResponse.Result.name;
            SetResponse setResponse = _firebaseClient.Set("Category/" + categoryData.CategoryCode, categoryData);
        }



        public void EditCategory(Models.Category category)
        {

            SetResponse firebaseSetResponse = _firebaseClient.Set("Category/" + category.CategoryCode, category);


        }

        public List<Models.Category> CategoryList()
        {
            FirebaseResponse firebaseResponse = _firebaseClient.Get("Category/");
            dynamic categoryData = JsonConvert.DeserializeObject<dynamic>(firebaseResponse.Body);
            var categoryList = new List<Models.Category>();
            if (categoryData != null)
            {
                foreach (var category in categoryData)
                {
                    categoryList.Add(JsonConvert.DeserializeObject<Models.Category>(((JProperty)category).Value.ToString()));
                }
            }
            return categoryList;
        }

        public void RemoveCategory(string categoryCode)
        {
            FirebaseResponse firebaseResponse = _firebaseClient.Delete("Category/" + categoryCode);
        }

        public Models.Category ShowCategory(string categoryCode)
        {
            FirebaseResponse firebaseResponse = _firebaseClient.Get("Category/" + categoryCode);
            Models.Category category = JsonConvert.DeserializeObject<Models.Category>(firebaseResponse.Body);
            return category;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}