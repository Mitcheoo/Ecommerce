using Ecommerce.Helpers;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
          var count =  HttpContext.Session.Get<List<CartItemVM>>(MySetting.CART_KEY) ?? new List<CartItemVM>(); // xem mau sessionextensions
            return View("CartPanel",
                new CartModel
                {
                    Quantity = count.Sum(p => p.SoLuong),
                    Total = count.Sum(p => p.ThanhTien)

                }
                );
        }
    }
}
