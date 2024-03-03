/*
*
*   Our game engine functions in 2D, but all its features except for graphics can mostly be extended
*       from existing data structures.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/

using System.Numerics;

namespace Shard
{
    class Transform3D : Transform
    {
        private float z, lastZ;
        private double rotx, roty;
        private int scalez;
        private Vector3 forward, right, up, centre;

        public Transform3D(GameObject o) : base(o)
        {
        }

        public float Z
        {
            get => z;
            set => z = value;
        }

        public Vector3 getLast3DDirection()
        {
            return new Vector3(lastX - x, lastY - y, lastZ - z);
        }


        public int Scalez
        {
            get => scalez;
            set => scalez = value;
        }
        public double Rotx { get => rotx; set => rotx = value; }
        public double Roty { get => roty; set => roty = value; }

        public Vector3 translate(Vector3 vertex)
        {
            return vertex + new Vector3(X, Y, Z);
        }


    }
}
