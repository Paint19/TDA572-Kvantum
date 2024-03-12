using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.pathfinding
{
    interface Scorer<T> where T : GraphNode
    {
        double computeCost(T from, T to);
    }
}
