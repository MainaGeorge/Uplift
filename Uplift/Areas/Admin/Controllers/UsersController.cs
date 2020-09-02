using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var idClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);

            return View(_unitOfWork.User.GetAll(filter: u => u.Id != idClaim.Value));
        }

        public IActionResult UnLock(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.User.UnlockUser(userId);
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Lock(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.User.LockUser(userId);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
