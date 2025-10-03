using Microsoft.AspNetCore.Mvc;
using Ecommerce.Data;

namespace Ecommerce.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly DatabaseEcommerceContext db;

        public MenuLoaiViewComponent(DatabaseEcommerceContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new Ecommerce.ViewModels.MenuLoaiVM
            {
                MaLoai = lo.MaLoai,
               TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            }).OrderBy(p=>p.TenLoai);
            return View(data); //default.cshtml dsdasdasdasdasdsd
            //abc

        }
    }
}
