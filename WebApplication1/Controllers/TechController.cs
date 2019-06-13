using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechRent.Domain.Abstract;
using TechRent.Domain.Entities;
using TechRent.WebUI.Models;

namespace TechRent.WebUI.Controllers
{
    public class TechController : Controller
    {
        private ITechRepository repository;
        public int pageSize = 3;

        public TechController(ITechRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string category, int page=1)
        {
            TechListViewModel model = new TechListViewModel
            {
                Teches = repository.Teches
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(ta => ta.TechID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                Paging_Info = new Paging_Info
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
        repository.Teches.Count() :
        repository.Teches.Where(ta => ta.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
    }
}