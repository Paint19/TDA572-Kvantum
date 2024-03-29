using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Shard
{
    class Rat : GameObject
    {
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);
        Vector3 moveDirection;

        public Rat(Vector3 startLocation, Vector3 direction)
        {
            this.Transform.SpritePath = "white.png";
            this.Transform.InitialColor = new Vector3(0.0f, 0.0f, 1.0f);
            this.Transform.initRenderer("rat.obj");
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
