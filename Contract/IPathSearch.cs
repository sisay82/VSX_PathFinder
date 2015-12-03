using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathSearch.Contract
{
    interface IPathSearch
    {
        bool NextStep(Action<Point> onStep);
    }
}
