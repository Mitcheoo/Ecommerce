using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.ViewComponents  // chỉnh lại theo namespace thật của bạn
{
    public class TimKiemPanelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(); // trỏ đến Views/Shared/Components/TimKiemPanel/Default.cshtml
        }
    }
}
