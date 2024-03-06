using OpenTK.Mathematics;
using Shard.Shard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Rat : GameObject
    {
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);
        Vector3 moveDirection;

        public Rat(float dir) 
        {
            this.Transform.SpritePath = "test.png";
            this.Transform.initRenderer("rat.obj");
            this.Transform.scale(0.001f);
            this.moveDirection = new Vector3(0.01f,0.01f,0.01f);
        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
