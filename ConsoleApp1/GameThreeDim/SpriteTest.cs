using OpenTK.Mathematics;
using Shard.Shard;
using Shard.Shard.Animation;
using System;

namespace Shard
{
    class SpriteTest : Sprite
    {
        public SpriteTest(float h, float w, int nFrames, String spritePath) : base(h, w, 0,0,1,1, spritePath)
        {
            Animation = new Animation(nFrames, 10, 0, false);
        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            Vector3 camPos = Bootstrap.getWindow().getActiveCamera().getPosition();
            this.Transform.Translation = camPos + new Vector3(0f, 0f, -2f);
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getAnimationSystem().addToAnimate(this);
            Bootstrap.getDisplay().addToDraw(this.Transform.getRenderer());
        }
    }
}
