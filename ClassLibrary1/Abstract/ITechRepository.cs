using System.Collections.Generic;
using TechRent.Domain.Entities;

namespace TechRent.Domain.Abstract
{
    public interface ITechRepository
    {
        IEnumerable<Tech> Teches { get; }
    }
}
