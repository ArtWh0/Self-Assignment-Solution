using System.Collections.Generic;
using TechRent.Domain.Entities;

namespace TechRent.WebUI.Models
{
    public class TechListViewModel
    {
        public IEnumerable<Tech> Teches { get; set; }
        public Paging_Info Paging_Info { get; set; }
        public int Days { get; set; }
        public string CurrentCategory { get; set; }
    }
}