using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Teapot : GameObject
    {
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);
        float moveDirection;

        public Teapot(float dir)
        {
            this.Transform.initRenderer("teapot.obj");
            this.Transform.tmpChangeSize(0.1f);
            this.moveDirection = dir;
        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            this.Transform.rotate(persistentRotationMatrix3);
            this.Transform.tmpMove(moveDirection);
            Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
