using Firebase1.Models;
using Firebase1.Repository.Category;
using Firebase1.Utilities.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Firebase1.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryRepository _categoryRepository;
        private ManageFiles _fileUtility;
        public CategoryController()
        {
            _categoryRepository = new CategoryRepository();
            _fileUtility = new ManageFiles();
        }
        // GET: Category
        public ActionResult Index()
        {
            List<Category> categoryList = _categoryRepository.CategoryList();
            if (categoryList == null)
            {
                ModelState.AddModelError(string.Empty, "No items to display");
            }
            return View(_categoryRepository.CategoryList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Models.Category category, HttpPostedFileBase file)
        {
            FileStream stream;

            //validation, check if category already exists. 
            Models.Category categoryNotExist = _categoryRepository.ShowCategory(category.CategoryCode);
            if (categoryNotExist != null)
            {
                ModelState.AddModelError(string.Empty, "Category " + category.CategoryName + " already exists.");
                return RedirectToAction("Index", "Category");
            }

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
                        category.CategoryImagePath = fileStoredPath.ToString();
                        category.CategoryImageName = file.FileName;
                        _categoryRepository.AddCategory(category);
                        return RedirectToAction("Index", "Category");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_categoryRepository.ShowCategory(id));
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            Category selectedCategory = _categoryRepository.ShowCategory(id);
            return View(selectedCategory);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Models.Category category, HttpPostedFileBase file)
        {
            FileStream stream;
            string removeImagePath = string.Empty;
            string removeImageName = string.Empty;
            Category oldCategory = _categoryRepository.ShowCategory(category.CategoryCode);
            removeImagePath = oldCategory.CategoryImagePath;
            removeImageName = oldCategory.CategoryImageName;

            if (file != null && file.ContentLength > 0)
            {

                if (string.IsNullOrEmpty(category.CategoryCode) == false)
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
                            category.CategoryImagePath = fileStoredPath.ToString();
                            category.CategoryImageName = file.FileName;
                        }
                    }
                }

            }
            else
            {
                category.CategoryImagePath = oldCategory.CategoryImagePath;

            }
            _categoryRepository.EditCategory(category);
            return RedirectToAction("Index", "Category");
        }


    }
}