using System.Collections.Generic;
using TechRent.Domain.Entities;
using TechRent.Domain.Abstract;

namespace TechRent.Domain.Concrete
{
    public class EFTechRepository : ITechRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Tech> Teches
        {
            get { return context.Teches; }
        }
    }
}