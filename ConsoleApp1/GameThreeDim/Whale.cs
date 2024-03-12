using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{

    class Whale : GameObject
    {
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);
        Vector3 moveDirection;

        public Whale(Vector3 startLocation, Vector3 direction)
        {
            this.Transform.SpritePath = "whale.jpg";
            this.Transform.initRenderer("whale.obj");
            this.Transform.scale(0.001f);
            this.Transform.Translation = startLocation;
            this.moveDirection = direction;
        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            this.Transform.rotate(persistentRotationMatrix3);
            this.Transform.translate(moveDirection);
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(Transform.getRenderer());
        }

        public override void onCollisionEnter(PhysicsBody x)
        {
            //base.onCollisionEnter(x);
            Console.WriteLine("Rat Attack!!!");
        }
    }
}
