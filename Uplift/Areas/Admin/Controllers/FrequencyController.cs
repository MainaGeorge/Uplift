using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            var frequency = new Frequency();
            if (!id.HasValue) return View(frequency);

            frequency = _unitOfWork.Frequency.Get(id.Value);

            if (frequency == null)
                return NotFound();

            return View(frequency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if (!ModelState.IsValid) return View(frequency);

            if (frequency.Id == 0)
            {
                _unitOfWork.Frequency.Add(frequency);
            }
            else
            {
                _unitOfWork.Frequency.Update(frequency);
            }

            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        #region APICALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Frequency.GetAll() });
        }


        public IActionResult Delete(int id)
        {
            var toDelete = _unitOfWork.Frequency.Get(id);
            if (toDelete == null)
            {
                return Json(new { success = false, message = "could not find the frequency specified" });
            }

            _unitOfWork.Frequency.Remove(toDelete);
            _unitOfWork.SaveChanges();

            return Json(new { success = true, message = "deletion successful" });
        }

        #endregion
    }
}