using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TeleworldShop.Model.Models;
using TeleworldShop.Web.App_Start;
using TeleworldShop.Web.Infrastructure.Extensions;
using TeleworldShop.Web.Mappings;
using TeleworldShop.Web.Models;
using TeleworldShop.Service;

namespace TeleworldShop.Web.Controllers
{
    public class UserController : Controller
    {
        private ApplicationUserManager _userManager;
        IOrderService _orderService;

        public UserController(ApplicationUserManager userManager, IOrderService orderService)
        {
            this._userManager = userManager;
            this._orderService = orderService;
        }

        public ActionResult Info()
        {
            return View();
        }

        public ActionResult Orders()
        {
            return View();
        }

        public ActionResult OrderDetail(int orderId)
        {
            var userId = User.Identity.GetUserId();
            ViewData["User"] = _userManager.FindById(userId);
            ViewData["Order"] = _orderService.GetById(orderId);
            ViewData["OrderDetail"] = _orderService.GetOrderedProducts(orderId);
            return View();
        }

        public JsonResult CancelOrder(int orderId)
        {
            if (Request.IsAuthenticated)
            {
                var dbOrder = _orderService.GetById(orderId);
                dbOrder.OrderStatus = "Canceled";
                _orderService.Update(dbOrder);
                _orderService.Save();

                // Return product quantity
                _orderService.RollBackOrder(orderId);

                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return PartialView("Sidebar");
        }

        public JsonResult GetUserInfo()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                var mapper = new Mapper(AutoMapperConfiguration.Configure());
                var applicationUserViewModel = mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
                return Json(new
                {
                    data = applicationUserViewModel,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public JsonResult GetUserOrders()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var newOrder = _orderService.GetOrdersByUserId(userId);
                return Json(new
                {
                    data = newOrder,
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public async Task<JsonResult> UpdateUserInfo(string appUserVm)
        {
            var user = new JavaScriptSerializer().Deserialize<ApplicationUserViewModel>(appUserVm);

            var newUser = await _userManager.FindByIdAsync(user.Id);

            newUser.UpdateUser(user);

            if (user.Password != null)
            {
                var result = await _userManager.ChangePasswordAsync(user.Id, user.OldPassword, user.Password);
                if (result.Succeeded)
                {
                    var result01 = await _userManager.UpdateAsync(newUser);
                    if (result01.Succeeded)
                    {
                        return Json(new
                        {
                            status = true,
                            message = "Update user information successfully"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = false,
                            message = "Error"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = "WrongPwd"
                    });
                }
            }
            else
            {
                var result = await _userManager.UpdateAsync(newUser);
                if (result.Succeeded)
                {
                    return Json(new
                    {
                        status = true,
                        message = "Update user information successfully"
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = "Error"
                    });
                }
            }
        }
    }
}