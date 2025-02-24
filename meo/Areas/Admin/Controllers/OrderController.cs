using System.Diagnostics;
using System.Security.Claims;
using meo.DataAccess.Repository;
using meo.DataAccess.Repository.IRepository;
using meo.Models;
using meo.Models.ViewModels;
using meo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace meo.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        // want to use bindproperty scope always public or it will not work

        public OrderController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
          
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                orderHeader = _UnitOfWork.OrderHeader.Get(u => u.OrderHeaderId == orderId, includeProperties: "applicationUser"),
                orderDetails = _UnitOfWork.OrderDetails.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(OrderVM);
        }

        

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult UpdateOrderDetail(int orderId)
        {
            var orderHeaderFromDb = _UnitOfWork.OrderHeader.Get(u => u.OrderHeaderId == orderId);
            orderHeaderFromDb.name = OrderVM.orderHeader.name;
            orderHeaderFromDb.phoneNumber = OrderVM.orderHeader.phoneNumber;
            orderHeaderFromDb.streetAddress = OrderVM.orderHeader.streetAddress;
            orderHeaderFromDb.city = OrderVM.orderHeader.city;
            orderHeaderFromDb.state = OrderVM.orderHeader.state;
            orderHeaderFromDb.portalCode = OrderVM.orderHeader.portalCode;

            if(!string.IsNullOrEmpty(OrderVM.orderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderVM.orderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.orderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            }
            _UnitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _UnitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction(nameof(Details), new {orderId = orderHeaderFromDb.OrderHeaderId});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _UnitOfWork.OrderHeader.UpdateStatus(OrderVM.orderHeader.OrderHeaderId, SD.StatusInProcess);
            _UnitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully";
            return RedirectToAction(nameof(Details), new {orderId = OrderVM.orderHeader.OrderHeaderId});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            var orderHeader = _UnitOfWork.OrderHeader.Get(u => u.OrderHeaderId ==OrderVM.orderHeader.OrderHeaderId);
            orderHeader.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.orderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.shippingDate = DateTime.Now;
            // below is for company, they are entitile to pay after 30 days 
            if(orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            _UnitOfWork.OrderHeader.Update(orderHeader);
            _UnitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully";
            return RedirectToAction(nameof(Details) , new { orderId = OrderVM.orderHeader.OrderHeaderId});
        }

        //[HttpPost]
        //[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        //public IActionResult CancelOrder()
        //{

        //    var orderHeader = _UnitOfWork.OrderHeader.Get(u => u.OrderHeaderId == OrderVM.orderHeader.OrderHeaderId);

        //    if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
        //    {
        //        var options = new RefundCreateOptions
        //        {
        //            Reason = RefundReasons.RequestedByCustomer,
        //            PaymentIntent = orderHeader.PaymentIntentId
        //        };

        //        var service = new RefundService();
        //        Refund refund = service.Create(options);

        //        _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
        //    }
        //    else
        //    {
        //        _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
        //    }
        //    _unitOfWork.Save();
        //    TempData["Success"] = "Order Cancelled Successfully.";
        //    return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });

        //}

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderHeader;

            if(User.IsInRole(SD.Role_Admin)||User.IsInRole( SD.Role_Employee))
            {
                objOrderHeader = _UnitOfWork.OrderHeader.GetAll(includeProperties: "applicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeader = _UnitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, includeProperties: "applicationUser");
            }
            switch (status)
            {
                case "pending":
                    {
                        objOrderHeader = objOrderHeader.Where(u => u.PaymentStatus == SD.PaymentStatusPending);
                        break;
                    }
                case "inprocess":
                    {
                        objOrderHeader = objOrderHeader.Where(u => u.PaymentStatus == SD.StatusInProcess);
                        break;
                    }
                case "completed":
                    {
                        objOrderHeader = objOrderHeader.Where(u => u.PaymentStatus == SD.StatusShipped);
                        break;
                    }
                case "approved":
                    {
                        objOrderHeader = objOrderHeader.Where(u => u.PaymentStatus == SD.StatusApproved);
                        break;
                    }
                default:
                  
                    break;
            }

            return Json(new { data = objOrderHeader });
        }






        #endregion
    }
}
