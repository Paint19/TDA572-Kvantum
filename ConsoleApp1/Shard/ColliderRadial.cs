using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class ColliderRadial : Collider3D
    {
        public ColliderRadial(CollisionHandler gob) : base(gob)
        {
        }

        public override void calculateBoundingBox()
        {
            throw new NotImplementedException();
        }

        public override bool checkBoundingCollision(Collider3D c)
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

        public override Vector3? checkCollision(Vector3 c)
        {
            throw new NotImplementedException();
        }

        public override void drawMe(Color col)
        {
            throw new NotImplementedException();
        }

        public override float[] getMinAndMaxX()
        {
            throw new NotImplementedException();
        }

        public override float[] getMinAndMaxY()
        {
            throw new NotImplementedException();
        }
    }
}
