using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
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

        public IActionResult Details(int id)
        {
            var orderViewModel = new OrderViewModel
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(id),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(filter: o => o.OrderHeaderId == id)
            };

            return View(orderViewModel);
        }




        #region API

        public IActionResult GetAllOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll() });
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

        public IActionResult Approve(int orderHeaderId)
        {
            _unitOfWork.OrderHeader.ChangeOrderStatus(orderHeaderId, AppConstants.StatusApproved);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Reject(int orderHeaderId)
        {
            _unitOfWork.OrderHeader.ChangeOrderStatus(orderHeaderId, AppConstants.StatusRejected);
            return RedirectToAction(nameof(Index));
        }
    }
}
