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

            return View("Checkout",CartViewModel);
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
    }
}
