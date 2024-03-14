namespace Shard.pathfinding
{
    interface Scorer<T> where T : GraphNode
    {
        double computeCost(T from, T to);
    }
}
