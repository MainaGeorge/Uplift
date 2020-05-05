using System;
using System.IO;
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

        [BindProperty]
        public ServiceViewModel ServiceViewModel { get; set; }

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
            ServiceViewModel = new ServiceViewModel
            {
                Service = new Service(),
                CategorySelectListItems = _unitOfWork.Category.GetCategoriesForDropdown(),
                FrequencySelectListItems = _unitOfWork.Frequency.GetFrequencyListForDropDown()
            };

            if (id.HasValue)
            {
                ServiceViewModel.Service = _unitOfWork.Service.Get(id.Value);
            }

            return View(ServiceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                var webRootPath = _webHost.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (ServiceViewModel.Service.Id == 0)
                {
                    //New Service
                    var fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    ServiceViewModel.Service.ImageUrl = @"\images\services\" + fileName + extension;

                    _unitOfWork.Service.Add(ServiceViewModel.Service);
                }
                else
                {
                    //Edit Service
                    var serviceFromDb = _unitOfWork.Service.Get(ServiceViewModel.Service.Id);
                    if (files.Count > 0)
                    {
                        var fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\services");
                        var extensionNew = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extensionNew), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }
                        ServiceViewModel.Service.ImageUrl = @"\images\services\" + fileName + extensionNew;
                    }
                    else
                    {
                        ServiceViewModel.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }

                    _unitOfWork.Service.Update(ServiceViewModel.Service);
                }

                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ServiceViewModel.CategorySelectListItems = _unitOfWork.Category.GetCategoriesForDropdown();
                ServiceViewModel.FrequencySelectListItems = _unitOfWork.Frequency.GetFrequencyListForDropDown();
                return View(ServiceViewModel);
            }
        }




        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includedProperties: "Frequency,Category") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var toDelete = _unitOfWork.Service.GetFirstOrDefault(s => s.Id == id);
            
            if (toDelete == null)
            {
                return Json(new { success = false, message = "Something went wrong while deleting" });
            }

            var webRootPath = _webHost.WebRootPath;
            var imagePath = Path.Combine(webRootPath, toDelete.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _unitOfWork.Service.Remove(toDelete);
            _unitOfWork.SaveChanges();

            return Json(new { success = true, message = "deleted successfully" });
        }
        #endregion
    }
}