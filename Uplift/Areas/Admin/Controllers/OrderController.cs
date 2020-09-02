using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }






        #region API

        public IActionResult GetAllOrders()
        {
            return Json(new {data = _unitOfWork.OrderHeader.GetAll()});
        }

        public IActionResult GetAllPendingOrders()
        {
            return Json(new
            {
                data = _unitOfWork.OrderHeader
                    .GetAll(filter: o => o.Status == AppConstants.StatusSubmitted)
            });
        }

        public IActionResult GetAllApprovedOrders()
        {
            return Json(new
            {
                data = _unitOfWork.OrderHeader.GetAll(filter: o => o.Status == AppConstants.StatusApproved)
            });
        }

        #endregion
    }
}
