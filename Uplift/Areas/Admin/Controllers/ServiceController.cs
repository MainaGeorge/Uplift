using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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
                var uploadedImages = HttpContext.Request.Form.Files;

                if (ServiceViewModel.Service.Id == 0)
                {
                    var imageUrl = AddPhotoToImagesFolderAndReturnItsLocation(HttpContext);

                    ServiceViewModel.Service.ImageUrl = imageUrl;

                    _unitOfWork.Service.Add(ServiceViewModel.Service);
                }
                else
                {
                    //Edit Service
                    var serviceFromDb = _unitOfWork.Service.Get(ServiceViewModel.Service.Id);
                    if (uploadedImages.Count > 0)
                    {
                        var imageToBeDeletedUrlPath = Path.Combine(_webHost.WebRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));

                        DeleteExistingImageFromFolderInCaseOfEditing(imageToBeDeletedUrlPath);

                        var newImageUrl = AddPhotoToImagesFolderAndReturnItsLocation(HttpContext);

                        ServiceViewModel.Service.ImageUrl = newImageUrl;
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

        private string AddPhotoToImagesFolderAndReturnItsLocation(HttpContext context)
        {
            var imageName = Guid.NewGuid();
            var imageFolder = Path.Combine(_webHost.WebRootPath, @"images\services");
            var firstImage = context.Request.Form.Files[0];
            var imageExtension = Path.GetExtension(firstImage.FileName);

            using var fileStreams = new FileStream(Path.Combine(imageFolder, imageName + imageExtension), FileMode.Create);

            firstImage.CopyTo(fileStreams);

            var imageUrl = @"\images\services\" + imageName + imageExtension;

            return imageUrl;
        }

        private static void DeleteExistingImageFromFolderInCaseOfEditing(string imageUrl)
        {
            if (System.IO.File.Exists(imageUrl))
            {
                System.IO.File.Delete(imageUrl);
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

            DeleteExistingImageFromFolderInCaseOfEditing(imagePath);

            _unitOfWork.Service.Remove(toDelete);
            _unitOfWork.SaveChanges();

            return Json(new { success = true, message = "deleted successfully" });
        }

        #endregion
    }
}