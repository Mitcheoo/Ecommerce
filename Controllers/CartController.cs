using Ecommerce.Data;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Helpers;

namespace Ecommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly DatabaseEcommerceContext db;
        const string CART_KEY = "MYCART";

        public CartController(DatabaseEcommerceContext context)
        {
            db = context;
        }

        public List<CartItemVM> Cart
        {
            get
            {
                return HttpContext.Session.Get<List<CartItemVM>>(CART_KEY) ?? new List<CartItemVM>();
            }
        }

        public IActionResult Index()
        {
            var cart = Cart;
            return View(cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);

            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Mặt hàng không tồn tại mã {id}";
                    return Redirect("/404");
                }

                item = new CartItemVM
                {
                    MaHh = hangHoa.MaHh,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    TenHh = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    SoLuong = quantity
                };

                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(CART_KEY, gioHang);
            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if(item!=null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(CART_KEY, gioHang); //cap nhat lai  gio hang
            }
            return RedirectToAction("Index"); //toacction la hanh dong luon 
        }
    }
}
