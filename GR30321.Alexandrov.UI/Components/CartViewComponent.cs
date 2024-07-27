using GR30321.Domain.Entities;
using GR30321.Alexandrov.UI.Session;
using Microsoft.AspNetCore.Mvc;

namespace GR30321.Alexandrov.UI.Components
{
    public class CartViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<Cart>("cart");
            return View(cart);

        }
    }
}
