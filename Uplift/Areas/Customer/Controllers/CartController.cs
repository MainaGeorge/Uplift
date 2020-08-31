using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
            var listServiceIdFromSession =
                HttpContext.Session.RetrieveFromSession<List<int>>(AppConstants.ShoppingCart);

            if (listServiceIdFromSession == null) return View(CartViewModel);

            foreach (var serviceId in listServiceIdFromSession)
            {
                CartViewModel.ServicesInCart
                    .Add(_unitOfWork.Service.GetFirstOrDefault(filter: s => s.Id == serviceId,
                        includedProperties: "Frequency,Category"));
            }
            return View(CartViewModel);
        }
    }
}
