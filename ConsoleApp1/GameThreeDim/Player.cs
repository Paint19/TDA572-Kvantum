using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    internal class Player : GameObject
    {
        public Player(Vector3 startLocation)
        {
            this.Transform.SpritePath = "penguin.png";
            this.Transform.initRenderer("penguin.obj");
            this.Transform.scale(0.03f);
            this.Transform.Translation = startLocation;
            setPhysicsEnabled();
            MyBody.addColliderCube();
        }
        public override void initialize()
        {
            base.initialize();
        }
        public override void update()
        {
            base.update();
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(Transform.getRenderer());
        }

        public override void onCollisionEnter(PhysicsBody x)
        {
            Console.WriteLine("Rat Attack!!!");
        }
    }
}
