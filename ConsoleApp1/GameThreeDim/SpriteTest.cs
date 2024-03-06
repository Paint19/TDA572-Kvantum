using OpenTK.Mathematics;
using Shard.Shard;
using Shard.Shard.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class SpriteTest : Sprite
    {
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);
        float moveDirection;

        public SpriteTest(float h, float w, String spritePath) : base(h, w, 0,0,1,1, spritePath)
        {
            Bootstrap.getDisplay().addToDraw(this);
            //this.Transform.tmpChangeSize(0.1f);
            this.moveDirection = 1;
            Animation = new Animation(6, 10, 0, false);
            //crop(0, 0, 0.5f, 0.5f);
        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            Bootstrap.getAnimmationSystem().addToAnimate(this);
            //this.Transform.rotateVertices(persistentRotationMatrix3);
            //this.Transform.tmpMove(moveDirection);
            Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
