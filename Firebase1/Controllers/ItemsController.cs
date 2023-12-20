using Firebase1.Models;
using Firebase1.Models.ViewModels;
using Firebase1.Repository.Category;
using Firebase1.Utilities.File;
using Firebase1.Repository.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Firebase1.Controllers
{
    public class ItemsController : Controller
    {

        private ItemsRepository _itemsRepository;
        private ManageFiles _fileUtility;
        private CategoryRepository _categoryRepository;

        public ItemsController()
        {
            _itemsRepository = new ItemsRepository();
            _fileUtility = new ManageFiles();
            _categoryRepository = new CategoryRepository();
        }
        // GET: Items
        public ActionResult Index()
        {
            List<Items> itemsList = _itemsRepository.ItemsList();
            if(itemsList == null)
            {
                ModelState.AddModelError(string.Empty, "No items to display");
            }
            return View(itemsList);
        }

              

        [HttpGet]
        public ActionResult Edit(string id)
        {
            Items selectedItem = _itemsRepository.ShowItem(id);

            List<Category> categories = _categoryRepository.CategoryList().ToList();

            selectedItem.CategoriesSelectList = categories.Select(i => new SelectListItem
            {
                Text = i.CategoryCode,
                Value = i.CategoryCode
            });


            return View(selectedItem);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Items item, HttpPostedFileBase file)
        {
            FileStream stream;
            string removeImagePath = string.Empty;
            string removeImageName = string.Empty;
            Items oldItem = _itemsRepository.ShowItem(item.ItemSystemCode);
            removeImagePath = oldItem.ItemImagePath;
            removeImageName = oldItem.ItemImageName;
            List<Category> categories = _categoryRepository.CategoryList().ToList();

            item.CategoriesSelectList = categories.Select(i => new SelectListItem
            {
                Text = i.CategoryCode,
                Value = i.CategoryCode
            });

            oldItem.CategoriesSelectList = item.CategoriesSelectList;


            if (file != null && file.ContentLength > 0)
            {
                
                if(string.IsNullOrEmpty(item.ItemSystemCode) == false)
                {
                    
                    if (file.FileName != removeImageName)
                    {
                        string removepath = Path.Combine(Server.MapPath("~/Content/images/"), removeImageName);
                        //REMOVE THE OLD IMAGE FROM THE CONTENT DIRECTORY
                        if (System.IO.File.Exists(removepath))
                        {
                            System.IO.File.Delete(removepath);
                            await _fileUtility.Delete(removeImageName);
                        }
                    }

                    string path = Path.Combine(Server.MapPath("~/Content/images/"), file.FileName);
                    file.SaveAs(path);
                    stream = new FileStream(Path.Combine(path), FileMode.Open);
                    var fileStoredPath = await Task.Run(() => _fileUtility.Upload(stream, file.FileName));
                    if (fileStoredPath != null)
                    {
                        if (fileStoredPath.Contains("Error"))
                        {
                            ModelState.AddModelError(string.Empty, fileStoredPath.ToString());
                        }
                        else
                        {
                            item.ItemImagePath = fileStoredPath.ToString();
                            item.ItemImageName = file.FileName;
                        }
                    }
                }
                
            }
            else
            {
                item.ItemImagePath = oldItem.ItemImagePath;
                _itemsRepository.EditItem(item);
            }

            return RedirectToAction("Index", "Items");
        }


        [HttpGet]
        public ActionResult Create()
        {

            List<Category> categories = _categoryRepository.CategoryList().ToList();
            Items item = new Items
            {

                CategoriesSelectList = categories.Select(i => new SelectListItem
                {
                    Text = i.CategoryCode,
                    Value = i.CategoryCode
                })

            };
            return View(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Items item, HttpPostedFileBase file)
        {
            FileStream stream;
            if (file.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Content/images/"), file.FileName);
                file.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                var fileStoredPath = await Task.Run(() => _fileUtility.Upload(stream, file.FileName));
                if (fileStoredPath != null)
                {
                    if (fileStoredPath.Contains("Error"))
                    {
                        ModelState.AddModelError(string.Empty, fileStoredPath.ToString());
                    }
                    else
                    {
                        item.ItemImagePath = fileStoredPath.ToString();
                        item.ItemImageName = file.FileName;
                        _itemsRepository.AddItem(item);
                        return RedirectToAction("Index", "Items");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_itemsRepository.ShowItem(id));
        }
    }
}