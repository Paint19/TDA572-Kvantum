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
        float moveDirection;

        public Rat(float dir) 
        {
            this.Transform.SpritePath = "rat_map.jpg";
            this.Transform.initRenderer("IKEA_rat.obj");
            Bootstrap.getDisplay().addToDraw(this);
            this.Transform.tmpChangeSize(2.0f);
            Matrix3 skewMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, 0.0f, 0.250f);
            this.Transform.rotateVertices(skewMatrix);
            this.moveDirection = dir;
        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            //this.Transform.rotateVertices(persistentRotationMatrix3);
            this.Transform.tmpMove(moveDirection);
        }
    }
}
