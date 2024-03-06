using OpenTK.Mathematics;
using Shard.Shard;
using Shard.Shard.Animation;
using System;

namespace Shard
{
    // Makes up the room walls, floor and ceiling in the demo
    class Plane : Sprite
    {
        public Plane(float h, float w, String spritePath) : base(h, w, 0, 0, 1, 1, spritePath)
        {
            Bootstrap.getDisplay().addToDraw(this);
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
