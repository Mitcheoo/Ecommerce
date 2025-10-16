using Ecommerce.Data;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly DatabaseEcommerceContext db;

        public HangHoaController(DatabaseEcommerceContext context) { db = context; }
        public IActionResult Index(int? Loai)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (Loai.HasValue)
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

        public IActionResult Search(string? query)
        {

            var hangHoas = db.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
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
        public IActionResult Detail(int id) {

            var data = db.HangHoas
                .Include(p=>p.MaLoaiNavigation)
                .SingleOrDefault(p => p.MaHh == id);

            if (data == null)
            {
                TempData["Message"] = $"Mặt hàng không tồn tại ma {id}";
                return Redirect("/404");
            }
            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHh = data.TenHh,
                Hinh = data.Hinh ?? string.Empty,
                DonGia = data.DonGia ?? 0,
                MoTaNgan = data.MoTaDonVi ?? string.Empty,
                TenLoai = data.MaLoaiNavigation.TenLoai,
                ChiTiet = data.MoTa ?? string.Empty,
                DiemDanhGia = 5, //check sau
                SoLuongTon = 10 // tinh sau 
            };
            return View(result);
        }


    }
}
