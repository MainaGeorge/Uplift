using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.ExtensionMethods;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public CartViewModel CartViewModel { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            CartViewModel = new CartViewModel
            {
                ServicesInCart = new List<Service>(),
                OrderHeader = new OrderHeader()
            };
        }
        public IActionResult Index()
        {
            SetAndInitializeCartViewModel();
            return View(CartViewModel);
        }

        public IActionResult Remove(int serviceId)
        {
            var listFromSession = HttpContext.Session.RetrieveFromSession<List<int>>(AppConstants.ShoppingCart);

            listFromSession.Remove(serviceId);
            HttpContext.Session.SaveObjectInSession(AppConstants.ShoppingCart, listFromSession);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            SetAndInitializeCartViewModel();

            return View("Checkout", CartViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            CartViewModel.ServicesInCart = new List<Service>();
            SetAndInitializeCartViewModel();
            if (!ModelState.IsValid) return View("Checkout", CartViewModel);

            CartViewModel.OrderHeader.DateOrdered = DateTime.Now;
            CartViewModel.OrderHeader.Status = AppConstants.StatusSubmitted;
            CartViewModel.OrderHeader.ServiceCount = CartViewModel.ServicesInCart.Count;

            _unitOfWork.OrderHeader.Add(CartViewModel.OrderHeader);
            _unitOfWork.SaveChanges();

            foreach (var service in CartViewModel.ServicesInCart)
            {
                var orderDetails = new OrderDetails
                {
                    ServiceId = service.Id,
                    ServicePrice = service.Price,
                    ServiceName = service.Name,
                    OrderHeaderId = CartViewModel.OrderHeader.Id
                };

                _unitOfWork.OrderDetails.Add(orderDetails);
            }

            _unitOfWork.SaveChanges();
            HttpContext.Session.SaveObjectInSession(AppConstants.ShoppingCart, new List<int>());

            return RedirectToAction("OrderConfirmation", "Cart", new {id = CartViewModel.OrderHeader.Id});
        }


        private void SetAndInitializeCartViewModel()
        {
            var listServiceIdFromSession =
                HttpContext.Session.RetrieveFromSession<List<int>>(AppConstants.ShoppingCart);

            if (listServiceIdFromSession == null) return;

            foreach (var serviceId in listServiceIdFromSession)
            {
                CartViewModel.ServicesInCart
                    .Add(_unitOfWork.Service.GetFirstOrDefault(filter: s => s.Id == serviceId,
                        includedProperties: "Frequency,Category"));
            }

            CartViewModel.TotalPrice = CartViewModel.ServicesInCart.Sum(x => x.Price * x.Frequency.FrequencyCount);
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}
