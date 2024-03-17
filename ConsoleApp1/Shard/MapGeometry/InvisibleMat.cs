using Shard.pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.MapGeometry
{
    class InvisibleMat : GameObject, GraphNode
    {
        List<PhysicsBody> containedBodies;
        HashSet<GameObject> occupants;
        GameObject above;
        public InvisibleMat(GameObject above): base()
        {
            this.above = above;
            containedBodies = new List<PhysicsBody>();
            occupants = new HashSet<GameObject>();
        }

        public string getId()
        {
            return Transform.Translation.ToString();
        }

        public override void onCollisionEnter(PhysicsBody x)
        {
            base.onCollisionEnter(x);
            containedBodies.Add(x);
            if (!occupants.Contains(x.Parent))
            {
                Console.WriteLine(x.Parent + " Finally entered the floor at: " + getId());
                occupants.Add(x.Parent);
            }
        }
        public override void onCollisionExit(PhysicsBody x)
        {
            base.onCollisionExit(x);
            containedBodies.Remove(x);
            if (!containedBodies.Any(body => body.Parent == x.Parent))
            {
                occupants.Remove(x.Parent);
                Console.WriteLine(x.Parent.ToString() + " Finally Left the floor at: " + getId());
            }
        }
    }
}
