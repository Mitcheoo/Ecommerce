using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Helpers;
using Ecommerce.ViewModels;
using ECommerceMVC.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly DatabaseEcommerceContext db;
        private readonly IMapper _mapper;

        public KhachHangController(DatabaseEcommerceContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        #region Reister

        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                var khachHang = _mapper.Map<KhachHang>(model);
                khachHang.RandomKey = MyUtil.GenerateRandomKey();
                khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                khachHang.HieuLuc = true; //se dung mail de active
                khachHang.VaiTro = 0; //mac dinh la khach hang

                if (Hinh != null)
                {
                    khachHang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
                }
                db.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index", "HangHoa");

            }
            return View();
        }
        #endregion
        #region Login in 

        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
                if (khachHang == null)
                {
                    ModelState.AddModelError("loi", "Đăng nhập không thành công, không tồn tại");
                }
                else
                {
                    if (!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("loi", "Tài khoản chưa được kích hoạt, hãy liên hệ Quang Huy");
                        return View();
                    }

                    if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                    {
                        ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                        return View();
                    }

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, khachHang.Email),
                new Claim(ClaimTypes.Name, khachHang.HoTen),
                new Claim("CustomerID", khachHang.MaKh),
                new Claim(ClaimTypes.Role, "Customer")
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);

                    if (Url.IsLocalUrl(ReturnUrl))
                        return Redirect(ReturnUrl);
                    else
                        return RedirectToAction("Index", "HangHoa");
                }
            }

            ModelState.AddModelError("", "Đăng nhập không thành công");
            return View();
        }
        #endregion
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("/");
        }
    }
    }
