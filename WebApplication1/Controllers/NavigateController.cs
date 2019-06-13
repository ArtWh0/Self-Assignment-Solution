using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechRent.Domain.Abstract;

namespace TechRent.WebUI.Controllers
{
    public class NavigateController : Controller
    {
        private ITechRepository repository;

        public NavigateController(ITechRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Teches
                .Select(ta => ta.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}