using System;
using System.Drawing;
using System.Linq;
using OpenTK.Mathematics;

namespace Shard
{
    class ColliderSphere : Collider3D
    {
        float radius = 0.0f;
        public ColliderSphere(CollisionHandler gob, Transform transform) : base(gob)
        {
            this.radius = transform.getVerticesAsVectors().MaxBy(it => it.Length).Length;
        }

        public ColliderSphere(CollisionHandler gob, Transform transform, float x, float y, float z, float radius) : base(gob) { this.radius = radius; }
        public override void calculateBoundingBox()
        {
            minDimensions = getMinDimensions();
            maxDimensions = getMaxDimensions();
        }

        public override Vector3? checkCollision(ColliderCube c)
        {
            // get box closest point to sphere center by clamping
            float x = Math.Max(c.minDimensions.X, Math.Min(X, c.maxDimensions.X));
            float y = Math.Max(c.minDimensions.Y, Math.Min(Y, c.maxDimensions.Y));
            float z = Math.Max(c.minDimensions.Z, Math.Min(Z, c.maxDimensions.Z));

            // this is the same as isPointInsideSphere
            Vector3 closestVertex = new Vector3(x, y, z);
            float distance = (closestVertex - myPosition).Length;
            if (distance < radius)
                return closestVertex;
            return null;
        }

        public override Vector3? checkCollision(ColliderSphere c)
        {
            if ((c.radius + radius) < (myPosition - c.myPosition).Length) 
                return myPosition - c.myPosition;
            else
                return null;
        }

        public override Vector3? checkCollision(Vector3 c)
        {
            if (radius < (myPosition - c).Length)
                return myPosition - c;
            else
                return null;
        }

        public override void drawMe(Color col)
        {
            // throw new NotImplementedException(); TODO: implement
        }

        public override Vector3 getMaxDimensions()
        {
            return myPosition + new Vector3(radius, radius, radius);
        }
        public override Vector3 getMinDimensions()
        {
            return myPosition - new Vector3(radius, radius, radius);
        }

        public float X { get => myPosition.X; set => myPosition.X = value; }
        public float Y { get => myPosition.Y; set => myPosition.Y = value; }
        public float Z { get => myPosition.Z; set => myPosition.Z = value; }
        public float Radius { get => radius; set => radius = value;}
    }
}
