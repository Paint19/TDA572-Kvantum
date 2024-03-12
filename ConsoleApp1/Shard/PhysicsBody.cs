/*
*
*   The physics body class does... a lot.  It handles the computation of internal values such as 
*       the min and max values for X and Y (used by the Sweep and Prune algorithm, as well as 
*       collision detection in general).  It registers and processes the colliders that belong to 
*       an object.  It handles the application of forces and torque as well as drag and angular drag.
*       It lets an object add colliders, and then exposes those colliders for narrow phase collision 
*       detection.  It handles some naive default collision responses such as a simple reflection
*       or 'stop on collision'.
*       
*   Important to note though that while this is called a PhysicsBody, no claims are made for the 
*       *accuracy* of the physics.  If you are planning to do anything that requires the physics
*       calculations to be remotely correct, you're going to have to extend the engine so it does 
*       that.  All I'm interested in here is showing you how it's *architected*. 
*       
*   This is also the subsystem which I am least confident about people relying on, because it is 
*       virtually untestable in any meaningful sense.  I spent three days trying to track down a 
*       bug that mean that an object would pass through another one at a rate of approximately
*       once every half hour...
*       
*   @author Michael Heron
*   @version 1.0
*   
*   Several substantial contributions to the code made by others:
*   @author Mårten Åsberg (see Changelog for 1.0.1)
*   
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Mathematics;

namespace Shard
{
    class PhysicsBody
    {
        List<Collider3D> myColliders;
        List<Collider3D> collisionCandidates;
        GameObject parent;
        CollisionHandler colh;
        Transform trans;
        private Vector3 angularDrag;
        private float drag;
        private Vector3 torque;
        private Vector3 force;
        private float mass;
        private double timeInterval;
        private float maxForce, maxTorque;
        private bool kinematic;
        private bool stopOnCollision;
        private bool reflectOnCollision;
        private bool impartForce;
        private bool passThrough;
        private bool usesGravity;
        private Color debugColor;
        public Color DebugColor { get => debugColor; set => debugColor = value; }

        private float[] minAndMaxX;
        private float[] minAndMaxY;
        private float[] minAndMaxZ;

        public void applyGravity(float modifier, Vector3 dir)
        {

            Vector3 gf = dir * modifier;

            addForce(gf);

        }

        public Vector3 AngularDrag { get => angularDrag; set => angularDrag = value; }
        public float Drag { get => drag; set => drag = value; }
        internal GameObject Parent { get => parent; set => parent = value; }
        internal Transform Trans { get => trans; set => trans = value; }
        public float Mass { get => mass; set => mass = value; }
        public float[] MinAndMaxX { get => minAndMaxX; set => minAndMaxX = value; }
        public float[] MinAndMaxY { get => minAndMaxY; set => minAndMaxY = value; }
        public float[] MinAndMaxZ { get => minAndMaxZ; set => minAndMaxZ = value; }
        public float MaxForce { get => maxForce; set => maxForce = value; }
        public float MaxTorque { get => maxTorque; set => maxTorque = value; }
        public bool Kinematic { get => kinematic; set => kinematic = value; }
        public bool PassThrough { get => passThrough; set => passThrough = value; }
        public bool UsesGravity { get => usesGravity; set => usesGravity = value; }
        public bool StopOnCollision { get => stopOnCollision; set => stopOnCollision = value; }
        public bool ReflectOnCollision { get => reflectOnCollision; set => reflectOnCollision = value; }
        public bool ImpartForce { get => this.impartForce; set => this.impartForce = value; }
        internal CollisionHandler Colh { get => colh; set => colh = value; }

        public void drawMe()
        {
            foreach (Collider3D col in myColliders)
            {
                col.drawMe(DebugColor);
            }
        }
        /**
         * @index should be the dimension you want to access
         * 0 = x
         * 1 = y
         * 2 = z
         **/
        public float[] getMinAndMax(int index)
        {
            float min = Int32.MaxValue;
            float max = -1 * min;
            float[] tmp;

            foreach (Collider3D col in myColliders)
            {
                min = Math.Min(col.minDimensions[index], min);
                max = Math.Max(col.maxDimensions[index], max);
            }
            return [min, max];
        }

        public PhysicsBody(GameObject p)
        {
            DebugColor = Color.Green;

            myColliders = new List<Collider3D>();
            collisionCandidates = new List<Collider3D>();

            Parent = p;
            Trans = p.Transform;
            Colh = (CollisionHandler)p;

            AngularDrag = new Vector3(0.01f);
            Drag = 0.01f;
            Drag = 0.01f;
            Mass = 1;
            MaxForce = 10;
            MaxTorque = 2;
            usesGravity = false;
            stopOnCollision = true;
            reflectOnCollision = false;

            MinAndMaxX = new float[2];
            MinAndMaxY = new float[2];
            MinAndMaxZ = new float[2];

            timeInterval = PhysicsManager.getInstance().TimeInterval;
            //            Debug.getInstance().log ("Setting physics enabled");

            PhysicsManager.getInstance().addPhysicsObject(this);
        }

        public void addTorque(Vector3 dir)
        {
            if (Kinematic)
            {
                return;
            }

            torque += dir / Mass;

            if (torque.Length > MaxTorque)
            {
                torque.Normalize(); 
                torque = torque * MaxTorque;
            }
        }

        public void reverseForces(float prop)
        {
            if (Kinematic)
            {
                return;
            }

            force *= -prop;
        }

        public void impartForces(PhysicsBody other, float massProp)
        {
            other.addForce(force * massProp);

            recalculateColliders();

        }

        public void stopForces()
        {
            force = Vector3.Zero;
        }

        public void reflectForces(Vector3 impulse)
        {
            Vector3 reflect = new Vector3(0, 0, 0);

            Debug.Log ("Reflecting " + impulse);

            // We're being pushed to the right, so we must have collided with the right.
            if (impulse.X > 0)
                reflect.X = -1;

            // We're being pushed to the left, so we must have collided with the left.
            if (impulse.X < 0)
                reflect.X = -1;

            // We're being pushed upwards, so we must have collided with the top.
            if (impulse.Y < 0)
                reflect.Y = -1;

            // We're being pushed downwards, so we must have collided with the bottom.
            if (impulse.Y > 0)
                reflect.Y = -1;

            // We're being pushed Backwards, so we must have collided with the background.
            if (impulse.Z < 0)
                reflect.Z = -1;

            // We're being pushed Forwards, so we must have collided with the screen.
            if (impulse.Z > 0)
                reflect.Z = -1;
            force *= reflect;

            Debug.Log("Reflect is " + reflect);

        }

        public void reduceForces(float prop) {
            force *= prop;
        }

        public void addForce(Vector3 dir, float force) {
            addForce(dir * force);
        }

        public void addForce(Vector3 dir)
        {
            if (Kinematic)
            {
                return;
            }

            dir /= Mass;

            // Set a lower bound.
            if (dir.Length * dir.Length < 0.0001)
            {
                return;
            }

            force += dir;

            // Set a higher bound.
            if (force.Length > MaxForce)
            {
                force = Vector3.Normalize(force) * MaxForce;
            }
        }

        public void recalculateColliders()
        {
            foreach (Collider3D col in getColliders())
            {
                col.calculateBoundingBox();
            }

            MinAndMaxX = getMinAndMax(0);
            MinAndMaxY = getMinAndMax(1);
            MinAndMaxZ = getMinAndMax(2);
        }

        public void physicsTick()
        {
            List<Vector3> toRemove;
            float force;
            float rot = 0;

            toRemove = new List<Vector3>();

            rot = torque.Length;

            torque.X = Math.Max(torque.X - AngularDrag.X, 0);
            torque.Y = Math.Max(torque.Y - AngularDrag.Y, 0);
            torque.Z = Math.Max(torque.Z - AngularDrag.Z, 0);
            Matrix3 rotMatrix = Matrix3.Identity;
            rotMatrix.Diagonal = torque;
            //should rotate along all axes
            //torque should also be calculated in a 3d vector
            //No clue if this works as intended, would be a wonder if it did
            trans.rotate(rotMatrix);

            force = this.force.Length;

			trans.translate(this.force);

            if (force < Drag)
            {
                stopForces();
            }
            else if (force > 0)
            {
                this.force = (this.force / force) * (force - Drag);
            }

        }

        public ColliderCube addColliderCube()
        {
            ColliderCube cCube = new ColliderCube((CollisionHandler)parent, parent.Transform);

            addCollider(cCube);

            return cCube;
        }
        public ColliderSphere addColliderSphere()
        {
            ColliderSphere cSphere = new ColliderSphere((CollisionHandler)parent, parent.Transform);
            
            addCollider(cSphere);

            return cSphere;
        }

        public void addCollider(Collider3D col)
        {
            myColliders.Add(col);
        }

        public List<Collider3D> getColliders()
        {
            return myColliders;
        }

        public Vector3? checkCollisions(Vector3 other)
        {
            Vector3? d;


            foreach (Collider3D c in myColliders)
            {
                d = c.checkCollision(other);

                if (d != null)
                {
                    return d;
                }
            }

            return null;
        }


        public Vector3? checkCollisions(Collider3D other)
        {
            Vector3? d;

//            Debug.Log("Checking collision with " + other);
            foreach (Collider3D c in myColliders)
            {
                d = c.checkCollision(other);

                if (d != null)
                {
                    return d;
                }
            }

            return null;
        }
    }
}