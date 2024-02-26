/*
*
*   The specific collider for rectangles.   Handles rect/circle, rect/rect and rect/vector.
*   @author Oliver Andersson
*   @version 1.0
*   
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Shard
{
    class ColliderConvexPolygon : Collider3D
    {
        private Transform3D transform3D;
        private Vector3 myPosition;
        private IEnumerable<Vector3> myVertices;
        private IEnumerable<int> myIndices;
        private bool fromTrans;



        public ColliderConvexPolygon(CollisionHandler gob, Transform3D t) : base(gob)
        {

            this.MyRect = t;
            fromTrans = true;
            RotateAtOffset = false;
            calculateBoundingBox();
        }

        public ColliderConvexPolygon(CollisionHandler gob, Transform3D t, float x, float y, float z) : base(gob)
        {
            X = x;
            Y = y;
            Z = z;
            RotateAtOffset = true;
            this.MyRect = t;

            fromTrans = false;

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
            calculateBoundingBox(myVertices
                .Select(it => transform3D
                    .translate(it)
                )
            );
        }

        internal Transform3D MyRect { get => transform3D; set => transform3D = value; }
        public float X { get => myPosition.X; set => myPosition.X = value; }
        public float Y { get => myPosition.Y; set => myPosition.Y = value; }
        public float Z { get => myPosition.Z; set => myPosition.Z = value; }

        public override void drawMe(Color col)
        {
            throw new NotImplementedException();
        }

        public override Vector3? checkCollision(Vector3 other)
        {

            throw new NotImplementedException();
        }

        public override Vector3? checkCollision(ColliderConvexPolygon c)
        {
            throw new NotImplementedException();
        }

        public override Vector3? checkCollision(ColliderRadial c)
        {
            throw new NotImplementedException();
        }

        public override bool checkBoundingCollision(Collider3D c)
        {
            Vector3 cMin = c.getMinDimensions(), cMax = c.getMaxDimensions();
            //get references of c:s bounding boxes corners
            Vector3 compMaxMin = cMax - minDimensions, compMaxMax = cMax - maxDimensions;
            Vector3 compMinMin = cMin - minDimensions, compMinMax = cMin - maxDimensions;
            //
            Vector3 maxCheck = (compMaxMax * compMaxMin), minCheck = (compMinMax * compMinMin);
            //
            float[] results = new float[6];
            maxCheck.CopyTo(results, 0);
            minCheck.CopyTo(results, 3);
            return results.Any(it => it < 0);
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
