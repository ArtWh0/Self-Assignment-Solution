using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechRent.Domain.Entities
{
    public class Tech
    {
        public int TechID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Min_Price { get; set; }
        public decimal Rent_Price { get; set; }
        public decimal Points { get; set; }
        public int Days { get; set; }
    }
}
