/*
*
*   The specific collider for rectangles.   Handles rect/circle, rect/rect and rect/vector.
*   @author Oliver Andersson
*   @version 1.0
*   
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Mathematics;

namespace Shard
{
    class ColliderCube : Collider3D
    {
        private Transform transform3D;
        private bool fromTrans;

        public ColliderCube(CollisionHandler gob, Transform t) : base(gob)
        {
            this.MyRect = t;
            fromTrans = true;
            RotateAtOffset = false;
            calculateBoundingBox();
        }


        private void calculateBoundingBox(IEnumerable<Vector3> vertices)
        {
            minDimensions.X = vertices.MinBy(it => it.X).X;
            minDimensions.Y = vertices.MinBy(it => it.Y).Y;
            minDimensions.Z = vertices.MinBy(it => it.Z).Z;

            maxDimensions.X = vertices.MaxBy(it => it.X).X;
            maxDimensions.Y = vertices.MaxBy(it => it.Y).Y;
            maxDimensions.Z = vertices.MaxBy(it => it.Z).Z;
        }

        public override void calculateBoundingBox()
        {
            calculateBoundingBox(transform3D.getVerticesAsVectors());
        }

        internal Transform MyRect { get => transform3D; set => transform3D = value; }
        public float X { get => myPosition.X; set => myPosition.X = value; }
        public float Y { get => myPosition.Y; set => myPosition.Y = value; }
        public float Z { get => myPosition.Z; set => myPosition.Z = value; }

        public override void drawMe(Color col)
        {
            throw new NotImplementedException();
        }

        public override Vector3? checkCollision(Vector3 other)
        {
            bool isWithinAxisAlignedBoundingBox = VectorIsWithin(minDimensions, maxDimensions, other);
            if (!isWithinAxisAlignedBoundingBox)
                return null;
            return transform3D.Translation - other;
        }

        public override Vector3? checkCollision(ColliderCube c)
        {
            if (checkBoundingCollision(c))
                return myPosition - c.myPosition;
            return null;
        }

        public override Vector3? checkCollision(ColliderSphere c)
        {
            // get box closest point to sphere center by clamping
            float x = Math.Max(minDimensions.X, Math.Min(c.X, maxDimensions.X));
            float y = Math.Max(minDimensions.Y, Math.Min(c.Y, maxDimensions.Y));
            float z = Math.Max(minDimensions.Z, Math.Min(c.Z, maxDimensions.Z));

            // this is the same as isPointInsideSphere
            Vector3 closestVertex = new Vector3(x, y, z);
            float distance = (closestVertex - myPosition).Length;
            if (distance < c.Radius)
                return myPosition - c.myPosition;
            return null;
        }


        public override Vector3 getMinDimensions()
        {
            return minDimensions;
        }

        public override Vector3 getMaxDimensions()
        {
            return maxDimensions;
        }
    }


}
