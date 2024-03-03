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
using System.Xml.Serialization;

namespace Shard
{
    class Transform3D : Transform
    {
        private float z, lastZ, depth;
        private double rotx, roty;
        private float scalez;
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


        public float Scalez
        {
            get => scalez;
            set => scalez = value;
        }
        public double Rotx { get => rotx; set => rotx = value; }
        public double Roty { get => roty; set => roty = value; }

        public void rotate(float x, float y, float z)
        {

        }

        public override void recalculateCentre()
        {
            base.recalculateCentre();
            centre.Z = (float)(z + ((this.depth * scalez) / 2));
        }

        public void translate(float x, float y, float z)
        {
            lastX = this.x; 
            lastY = this.y; 
            lastZ = this.z;
            this.x += x; 
            this.y += y; 
            this.z += z;
            recalculateCentre();
        }
        public void translate(Vector3 offset)
        {
            translate(offset.X, offset.Y, offset.Z);
        }
    }
}
