using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Penguin : GameObject
    {
        Matrix3 skewMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, 0.0f, 0.250f);
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);
        Vector3 movementDirection = new Vector3(0, 0, 0);
        public Penguin() {
            this.Transform.SpritePath = "penguin.png";
            this.Transform.initRenderer("penguin.obj");
            this.Transform.scale(0.5f);
            this.Transform.rotate(skewMatrix);
        }
        public override void update()
        {
            base.update();
            this.Transform.rotate(persistentRotationMatrix3);
            this.Transform.translate(movementDirection);
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(Transform.getRenderer());
        }
    }
}
