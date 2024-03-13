using OpenTK.Mathematics;
using Shard.Shard;
using Shard.Shard.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class SpriteTest : Sprite
    {

        public SpriteTest(float h, float w, String spritePath) : base(h, w, 0,0,1,1, spritePath)
        {
            //Bootstrap.getDisplay().addToDraw(this.Transform.getRenderer());
            Animation = new Animation(26, 10, 0, false);
            //crop(0, 0, 0.5f, 0.5f);
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
