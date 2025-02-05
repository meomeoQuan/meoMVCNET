using System.Security.Claims;
using meo.DataAccess.Repository.IRepository;
using meo.Utility;
using Microsoft.AspNetCore.Mvc;

namespace meo.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userClaims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaims != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.
                        GetAll(u => u.ApplicationUserId == userClaims.Value).Count());
                    
                }


                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
          
        }

    }
}
