using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    internal class Cheese : GameObject
    {
        public Cheese(Vector3 startLocation)
        {
            this.Transform.SpritePath = "cheese.jpg";
            this.Transform.InitialColor = new Vector3(1.0f, 1.0f, 1.0f);
            this.Transform.initRenderer("cheese.obj");
            this.Transform.scale(0.03f);
            this.Transform.Translation = startLocation;
            setPhysicsEnabled();
            MyBody.addColliderCube();
            addTag("cheese");
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
            if (x.checkTag("player"))
            {
                Console.WriteLine("Rat Attack!!!");
                ((GameThreeDim)Bootstrap.getRunningGame()).cheeseGotEaten(this);
                ToBeDestroyed = true;
            }
        }
    }
}
