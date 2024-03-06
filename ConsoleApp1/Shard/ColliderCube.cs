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
using OpenTK.Mathematics;

namespace Shard
{
    class ColliderCube : Collider3D
    {
        private Transform3D transform3D;
        private bool fromTrans;

        public ColliderCube(CollisionHandler gob, Transform3D t) : base(gob)
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

        public override Vector3? checkCollision(ColliderCube c)
        {
            throw new NotImplementedException();
        }

        public override Vector3? checkCollision(ColliderSphere c)
        {
            throw new NotImplementedException();
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
