using System.Linq;
using System.Web.Mvc;
using TechRent.Domain.Entities;
using TechRent.Domain.Abstract;
using TechRent.WebUI.Models;

namespace TechRent.WebUI.Controllers
{
    public class CartController : Controller
    {
        
            
            private IOrderProcessor orderProcessor;

            public CartController(ITechRepository repo, IOrderProcessor processor)
            {
                repository = repo;
                orderProcessor = processor;
            }

            public ViewResult Checkout()
            {
                return View(new ShippingDetails());
            }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
            

            public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        private ITechRepository repository;
        public CartController(ITechRepository repo)
        {
            repository = repo;
        }


        public RedirectToRouteResult AddToCart(Cart cart, int techID, string returnUrl, Tech d)
        {
            Tech tech = repository.Teches
                .FirstOrDefault(g => g.TechID == techID);

            if (tech != null)
            {
                cart.AddItem(tech, d.Days);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int techID, string returnUrl)
        {
            Tech tech = repository.Teches
                .FirstOrDefault(g => g.TechID == techID);

            if (tech != null)
            {
                cart.RemoveLine(tech);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }


    }
}