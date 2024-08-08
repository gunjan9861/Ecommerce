using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.DataAccessLayer.Infrastructure.Repository;
using MyApp.Models;
using MyApp.Models.ViewModel;
using System.Globalization;
using static System.Net.WebRequestMethods;

namespace MyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitofWork;
        private IWebHostEnvironment _hostingEnvironment;

        public ProductController(IUnitOfWork unitofWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitofWork = unitofWork;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            ProductVM productvm = new ProductVM
            {
                Products= _unitofWork.Product.GetAll()
            };
            return View(productvm);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitofWork.Category.Add(category);
        //        _unitofWork.Save();
        //        TempData["Success"] = "Category created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View(category);
        //}
        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            ProductVM vm = new ProductVM()
            {
                Product = new(),
                Categories = _unitofWork.Category.GetAll().Select(x=> new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (id == null || id == 0)
            {
                return View(vm);

            }
            else
            {
                vm.Product= _unitofWork.Product.GetT(x => x.Id == id);
                if (vm.Product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vm);
                }

            }
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductVM vm,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string fileName = String.Empty;
                if(file!=null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
                    fileName = Guid.NewGuid().ToString()+"-"+file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);
                    using (var filestream = new FileStream(filePath, FileMode.Create)) 
                    {
                        file.CopyTo(filestream);
                    }
                    vm.Product.ImageUrl = filePath;
                }
                else if (string.IsNullOrEmpty(vm.Product.ImageUrl))
                {
                    // Set a default image URL if no image is uploaded and no existing image URL is present
                    vm.Product.ImageUrl = "https://www.pexels.com/photo/blue-bmw-sedan-near-green-lawn-grass-170811/";
                }
                if (vm.Product.Id == 0)
                {
                    _unitofWork.Product.Add(vm.Product);
                }
                
                
                _unitofWork.Save();
              
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            var category = _unitofWork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteData(int? id)
        {
            var category = _unitofWork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitofWork.Category.Delete(category);
            _unitofWork.Save();
            TempData["Success"] = "Category successfully removed";
            return RedirectToAction("Index");
        }


    }
}
