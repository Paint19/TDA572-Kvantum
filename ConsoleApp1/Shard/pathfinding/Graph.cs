using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.pathfinding
{
    class Graph<T> where T:GraphNode
    {
        private List<T> nodes;
        private Dictionary<string, List<string>> connections;

        public T getNode(string id)
        {
            return nodes.Find(n => n.getId() == id);
        }

        public List<T> getConnections(string id) {
            return connections[id].Select(id => getNode(id)).ToList();
        }
    }
}
