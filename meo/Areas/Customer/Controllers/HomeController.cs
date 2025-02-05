using System.Diagnostics;
using System.Security.Claims;
using meo.DataAccess.Repository.IRepository;
using meo.Models;
using meo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace meo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //if(claim != null)
            //{
            //    int count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count();
            //    HttpContext.Session.SetInt32(SD.SessionCart, count);
            //}

            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new() {


                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };

           
            return View(shoppingCart);
        }
        [HttpPost]
        [Authorize]
        public  IActionResult Details(ShoppingCart shoppingCart)
        {
         

            var claimsIdentity =  (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cart = _unitOfWork.ShoppingCart.
            Get(u => u.ApplicationUserId == shoppingCart.ApplicationUserId && u.ProductId == shoppingCart.ProductId);

            if (cart != null) {

                cart.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                HttpContext.Session.SetInt32(SD.SessionCart,_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count()+1);
            }
              
     
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
