using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathSearch.Contract
{
    interface IPlayground
    {
        bool IsBlocked(Point p);
        int Width { get; }
        int Height { get; }
    }
}
