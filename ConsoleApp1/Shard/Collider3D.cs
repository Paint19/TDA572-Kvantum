/*
 * @author Oliver Andersson
 * @version 1.0
 */

using System;
using System.Drawing;
using OpenTK.Mathematics;

namespace Shard
{
    abstract class Collider3D
    {
        internal CollisionHandler gameObject;
        internal Vector3 minDimensions, maxDimensions, myPosition;
        private bool rotateAtOffset;

        public Collider3D(CollisionHandler gob)
        {
            gameObject = gob;
            minDimensions = new Vector3(0, 0, 0);
            maxDimensions = new Vector3(0, 0, 0);

        }

        internal CollisionHandler GameObject { get => gameObject; set => gameObject = value; }
        
        public bool RotateAtOffset { get => rotateAtOffset; set => rotateAtOffset = value; }


        public virtual Boolean checkBoundingCollision(Collider3D c)
        {
            return (
                c.minDimensions.X <= maxDimensions.X &&
                c.maxDimensions.X >= minDimensions.X &&
                c.minDimensions.Y <= maxDimensions.Y &&
                c.maxDimensions.Y >= minDimensions.Y &&
                c.minDimensions.Z <= maxDimensions.Z &&
                c.maxDimensions.Z >= minDimensions.Z
            );
        }
        public abstract Vector3? checkCollision(ColliderCube c);

        public abstract Vector3? checkCollision(ColliderSphere c);

        public abstract Vector3? checkCollision(Vector3 c);

        public virtual Vector3? checkCollision(Collider3D c)
        {

            if (c is ColliderSphere)
            {
                return checkCollision((ColliderSphere)c);
            }

            if (c is ColliderCube)
            {
                return checkCollision((ColliderCube)c);
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

        public void onCollisionEnter(PhysicsBody x)
        {
            gameObject.onCollisionEnter(x);
        }
        public void onCollisionExit(PhysicsBody x)
        {
            gameObject.onCollisionExit(x);
        }
        public void onCollisionStay(PhysicsBody x)
        {
            gameObject.onCollisionStay(x);
        }
    }
}
