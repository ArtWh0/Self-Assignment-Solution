using System.Collections.Generic;
using System.Linq;

namespace TechRent.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Tech tech, int over_days)
        {
            CartLine line = lineCollection
                .Where(g => g.Tech.TechID == tech.TechID)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Tech = tech,
                    Over_Days = over_days
                });
            }
            else
            {
                line.Over_Days += over_days;
            }
        }

        public void RemoveLine(Tech tech)
        {
            lineCollection.RemoveAll(l => l.Tech.TechID == tech.TechID);
        }

        public decimal ComputeTotalValue()
        {            
            return lineCollection.Sum(e => e.Tech.Min_Price + (e.Over_Days * e.Tech.Rent_Price));
            
        }

        public decimal ComputeTotalPoints()
        {
            return lineCollection.Sum(e => e.Tech.Points);

        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Tech Tech { get; set; }
        public int Over_Days { get; set; }
    }
}