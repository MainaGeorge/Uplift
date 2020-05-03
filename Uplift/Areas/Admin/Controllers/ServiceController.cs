using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Models.ViewModels;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceController(IWebHostEnvironment webHost, IUnitOfWork unitOfWork)
        {
            _webHost = webHost;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            var serviceViewModel = new ServiceViewModel
            {
                Service = new Service(),
                CategorySelectListItems = _unitOfWork.Category.GetCategoriesForDropdown(),
                FrequencySelectListItems = _unitOfWork.Frequency.GetFrequencyListForDropDown()
            };

            if (id.HasValue)
            {
                serviceViewModel.Service = _unitOfWork.Service.Get(id.Value);
            }

            return View(serviceViewModel);
        }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public IActionResult Upsert(ServiceViewModel serviceViewModel)
        // {
        //     if (!ModelState.IsValid) return View(serviceViewModel);
        // }

        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includedProperties: "Frequency,Category") });
        }

        #endregion
    }
}