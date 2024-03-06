using OpenTK.Mathematics;
using Shard.Shard;
using Shard.Shard.Animation;
using System;

namespace Shard
{
    // Makes up the room walls, floor and ceiling in the demo
    class Plane : Sprite
    {
        Vector3 position;
        public Plane(float h, float w, String spritePath, Vector3 pos) : base(h, w, 0, 0, 1, 1, spritePath)
        {
            Bootstrap.getDisplay().addToDraw(this);
            position = pos;
            this.Transform.tmpMove(1.0f);
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
