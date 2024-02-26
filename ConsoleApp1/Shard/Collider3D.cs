/*
 * @author Oliver Andersson
 * @version 1.0
 */

using System;
using System.Drawing;
using System.Numerics;

namespace Shard
{
    abstract class Collider3D
    {
        private CollisionHandler gameObject;
        internal Vector3 minDimensions, maxDimensions;


        private bool rotateAtOffset;

        public Collider3D(CollisionHandler gob)
        {
            gameObject = gob;
            minDimensions = new Vector3(0, 0, 0);
            maxDimensions = new Vector3(0, 0, 0);

        }

        internal CollisionHandler GameObject { get => gameObject; set => gameObject = value; }
        
        public bool RotateAtOffset { get => rotateAtOffset; set => rotateAtOffset = value; }


        public abstract Boolean checkBoundingCollision(Collider3D c);
        public abstract Vector3? checkCollision(ColliderConvexPolygon c);

        public abstract Vector3? checkCollision(ColliderRadial c);

        public abstract Vector3? checkCollision(Vector3 c);

        public virtual Vector3? checkCollision(Collider3D c)
        {

            if (c is ColliderRadial)
            {
                return checkCollision((ColliderRadial)c);
            }

            if (c is ColliderConvexPolygon)
            {
                return checkCollision((ColliderConvexPolygon)c);
            }

            Debug.getInstance().log("Bug");
            // Not sure how we got here but c'est la vie
            return null;
        }

        public abstract void drawMe(Color col);
        //Also should pass context as argument when drawing the shape

        public abstract Vector3 getMinDimensions();
        public abstract Vector3 getMaxDimensions();

        public abstract void calculateBoundingBox();
    }
}
