using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.DataAccessLayer.Infrastructure.Repository;
using MyApp.Models;
using MyApp.Models.ViewModel;

namespace MyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitofWork;

        public ProductController(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
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
            CategoryVM vm = new CategoryVM();
            if (id == null || id == 0)
            {
                return View(vm);

            }
            else
            {
                vm.Category = _unitofWork.Category.GetT(x => x.Id == id);
                if (vm.Category == null)
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
        public IActionResult CreateUpdate(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Category.Id == 0) {
                    _unitofWork.Category.Add(vm.Category);
                    TempData["Success"] = "Category created successfully";
                }
                else
                {
                    _unitofWork.Category.Update(vm.Category);
                    TempData["Success"] = "Category updated successfully";
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
