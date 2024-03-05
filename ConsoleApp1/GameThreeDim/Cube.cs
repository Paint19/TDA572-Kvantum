using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Cube : GameObject
    {
        Matrix3 skewMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, 0.0f, 0.250f);
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);

        public Cube()
        {
            this.Transform.SpritePath = "spaceship.png";
            this.Transform.initRenderer("cube.obj");
            this.Transform.tmpChangeSize(0.5f);
            this.Transform.rotateVertices(skewMatrix);

        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            this.Transform.rotateVertices(persistentRotationMatrix3);
            Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
