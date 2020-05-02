using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Category.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryToDelete = _unitOfWork.Category.Get(id);
            if (categoryToDelete == null)
            {
                return Json(new { success = false, message = $"Error! no category with the id {id} was found" });
            }

            _unitOfWork.Category.Remove(categoryToDelete);
            _unitOfWork.SaveChanges();

            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}