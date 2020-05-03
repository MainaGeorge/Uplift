﻿using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

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


        [HttpGet]
        public IActionResult Upsert(int? categoryId)
        {
            var category = new Category();

            if (categoryId == null)
            {
                return View(category);
            }

            category = _unitOfWork.Category.Get(categoryId.Value);

            if (category == null)
                return NotFound();


            return View(category);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (!ModelState.IsValid) return View(category);


            if (category.Id == 0)
            {
                _unitOfWork.Category.Add(category);
            }
            else
            {
                _unitOfWork.Category.Update(category);
            }

            _unitOfWork.SaveChanges();

            return RedirectToAction(nameof(Index));

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