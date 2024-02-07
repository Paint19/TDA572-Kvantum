using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Shard.Shard.pathfinding
{
    class RouteFinder<T> where T : GraphNode
    {
        private Graph<T> graph;
        private Scorer<T> nextNodeScorer;
        private Scorer<T> targetScorer;

        public void setNextNodeScorer(Scorer<T> scorer)
        {
            this.nextNodeScorer = scorer;
        }

        public void setTargetScorer(Scorer<T> scorer)
        {
            this.targetScorer = scorer;
        }

        List<T> findRoute(T from, T to)
        {
            PriorityQueue<RouteNode<T>, double> openSet = new PriorityQueue<RouteNode<T>, double>();
            Dictionary<T, RouteNode<T>> allNodes = new Dictionary<T, RouteNode<T>>();

            RouteNode<T> start = new RouteNode<T>(from, default, 0d, targetScorer.computeCost(from, to));
            openSet.Enqueue(start, start.getRouteScore());
            allNodes.Add(from, start);

            while (openSet.Count > 0)
            {
                RouteNode<T> next = openSet.Dequeue();
                if (next.getCurrent().getId() == to.getId())
                {
                    List<T> route = new List<T>();
                    RouteNode<T> current = next;
                    do
                    {
                        route.Insert(0, current.getCurrent());
                        current = allNodes[current.getPrevious()];
                    } while (current != null);
                    return route;
                }
                graph.getConnections(next.getCurrent().getId()).ForEach(connection => {
                    RouteNode<T> nextNode = allNodes.GetValueOrDefault(connection, new RouteNode<T>(connection));
                    allNodes.Add(connection, nextNode);

                    double newScore = next.getRouteScore() + nextNodeScorer.computeCost(next.getCurrent(), connection);
                    if (newScore < nextNode.getRouteScore())
                    {
                        nextNode.setPrevious(next.getCurrent());
                        nextNode.setRouteScore(newScore);
                        nextNode.setEstimatedScore(newScore + targetScorer.computeCost(connection, to));
                        openSet.Enqueue(nextNode, nextNode.getRouteScore());
                    }
                });
            }
            

            throw new Exception("No route found");
        }

    }
}
