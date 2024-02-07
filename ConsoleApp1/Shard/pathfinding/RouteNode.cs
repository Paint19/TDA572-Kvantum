using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.pathfinding
{
    class RouteNode<T> : IComparable<RouteNode<T>> where T : GraphNode
    {
        private T current;
        private T previous;
        private double routeScore;
        private double estimatedScore;
        public RouteNode(T current, T previous, double routeScore, double estimatedScore)
        {
            this.current = current;
            this.previous = previous;
            this.routeScore = routeScore;
            this.estimatedScore = estimatedScore;
        }
        public RouteNode(T current)
        {
            this.current = current;
            this.previous = default;
            this.routeScore = Double.MaxValue;
            this.estimatedScore = Double.MaxValue;
        }
        public int CompareTo(RouteNode<T> other)
        {
            double comp = this.estimatedScore - other.estimatedScore;
            if (comp > 0) return 1;
            else if (comp < 0) return -1;
            else return 0;
        }

        public double getEstimatedScore()
        {
            return estimatedScore;
        }

        public void setEstimatedScore(double score)
        {
            this.estimatedScore = score;
        }

        public double getRouteScore()
        {
            return routeScore;
        }

        public void setRouteScore(double score)
        {
            this.routeScore = score;
        }

        public T getCurrent()
        {
            return current;
        }

        public void setCurrent(T current)
        {
            this.current = current;
        }

        public T getPrevious()
        {
            return previous;
        }

        public void setPrevious(T previous)
        {
            this.previous = previous;
        }
    }
}
