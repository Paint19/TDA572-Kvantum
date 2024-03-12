/*
*
*   Anything that is going to be an interactable object in your game should extend from GameObject.  
*       It handles the life-cycle of the objects, some useful general features (such as tags), and serves 
*       as the convenient facade to making the object work with the physics system.  It's a good class, Bront.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Shard.Shard.Animation;
using System;
using System.Collections.Generic;

namespace Shard
{
    class GameObject : CollisionHandler
    {
        private Transform transform;
        private bool transient;
        private bool toBeDestroyed;
        private bool visible;
        private PhysicsBody myBody;
        private Animation animation;
        private List<string> tags;

        public void addTag(string str)
        {
            if (tags.Contains(str))
            {
                return;
            }

            tags.Add(str);
        }

        public void removeTag(string str)
        {
            tags.Remove(str);
        }

        public bool checkTag(string tag)
        {
            return tags.Contains(tag);
        }

        public String getTags()
        {
            string str = "";

            foreach (string s in tags)
            {
                str += s;
                str += ";";
            }

            return str;
        }

        public void setPhysicsEnabled()
        {
            MyBody = new PhysicsBody(this);
        }


        public bool queryPhysicsEnabled()
        {
            if (MyBody == null)
            {
                return false;
            }
            return true;
        }

        public Transform Transform
        {
            get => transform;
        }
        internal Animation Animation
        {
            get => animation;
            set => animation = value;
        }

        public bool Visible
        {
            get => visible;
            set => visible = value;
        }
        public bool Transient { get => transient; set => transient = value; }
        public bool ToBeDestroyed { get => toBeDestroyed; set => toBeDestroyed = value; }
        internal PhysicsBody MyBody { get => myBody; set => myBody = value; }




        public virtual void initialize()
        {
        }

        public virtual void update()
        {
            if (myBody is null)
                return;
            foreach (Collider3D c in myBody.getColliders())
            {
                if (c != null)
                    c.update();
            }
        }

        public virtual void physicsUpdate()
        {
        }

        public virtual void prePhysicsUpdate()
        {
        }

        public GameObject()
        {
            GameObjectManager.getInstance().addGameObject(this);

            transform = new Transform();
            visible = false;

            ToBeDestroyed = false;
            tags = new List<string>();

            this.initialize();

        }

        public void checkDestroyMe()
        {

            if (!transient)
            {
                return;
            }

            //Should do a kinda advanced collision box check with the collider of the object and the viewspace to see if it is in the frame


            ToBeDestroyed = true;

        }

        public virtual void killMe()
        {
            PhysicsManager.getInstance().removePhysicsObject(myBody);

            myBody = null;
            transform = null;
        }

        public virtual void onCollisionEnter(PhysicsBody x)
        {
            throw new NotImplementedException();
        }

        public virtual void onCollisionExit(PhysicsBody x)
        {
            Console.WriteLine("Finally free from " + x.ToString());
        }

        public virtual void onCollisionStay(PhysicsBody x)
        {
            Console.WriteLine("still colliding with " + x.ToString());
        }
    }
}
