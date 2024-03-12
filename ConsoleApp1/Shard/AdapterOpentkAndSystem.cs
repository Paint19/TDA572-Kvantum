using OpenTK.Mathematics;

namespace Shard
{
    class AdapterOpentkAndSystem
    {
        public static Vector3 vectorToVector(System.Numerics.Vector3 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}
