﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.ExtensionMethods;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;


        public HomeViewModel HomeViewModel;

        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

        }

        public IActionResult Index()
        {
            HomeViewModel = new HomeViewModel
            {
                CategoryList = _unitOfWork.Category.GetAll(),
                ServiceList = _unitOfWork.Service.GetAll(includedProperties: "Frequency")
            };
            return View(HomeViewModel);
        }

        public IActionResult AddToCart(int serviceId)
        {
            var sessionList = new List<int>();
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(AppConstants.ShoppingCart)))
            {
                sessionList.Add(serviceId);
                HttpContext.Session.SaveObjectInSession(AppConstants.ShoppingCart, sessionList);
            }
            else
            {
                sessionList = HttpContext.Session.RetrieveFromSession<List<int>>(AppConstants.ShoppingCart);
                if (sessionList.Contains(serviceId)) return RedirectToAction(nameof(Index));

                sessionList.Add(serviceId);
                HttpContext.Session.SaveObjectInSession(AppConstants.ShoppingCart, sessionList);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int serviceId)
        {
            var serviceFromDb =
                _unitOfWork.Service.GetFirstOrDefault(includedProperties: "Frequency,Category",
                    filter: c => c.Id == serviceId);
            return View(serviceFromDb);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
