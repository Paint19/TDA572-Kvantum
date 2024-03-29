﻿using System.Collections.Generic;
using System.Linq;

namespace Shard.pathfinding
{
    class Graph<T> where T : GraphNode
    {
        private List<T> nodes;
        private Dictionary<string, List<string>> connections;

        public T getNode(string id)
        {
            return nodes.Find(n => n.getId() == id);
        }

        public List<T> getConnections(string id)
        {
            return connections[id].Select(it => getNode(it)).ToList();
        }
    }
}
