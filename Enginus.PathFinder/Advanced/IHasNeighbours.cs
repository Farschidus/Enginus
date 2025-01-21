using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enginus.Navigation.Advanced
{
    public interface IHasNeighbours<N>
    {
        IEnumerable<N> Neighbours { get; }
    }
}
