using Ecommerce.Data;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly DatabaseEcommerceContext db;

        public HangHoaController(DatabaseEcommerceContext context) { db = context; }
        public IActionResult Index(int? Loai)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if(Loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == Loai.Value);
            }
            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                Hinh = p.Hinh ?? "",
                DonGia = p.DonGia ?? 0,
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return View(result);
        }
    }
}
