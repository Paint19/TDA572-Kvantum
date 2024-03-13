using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Penguin : GameObject
    {
        public Penguin(Vector3 startLocation) {
            this.Transform.SpritePath = "penguin.png";
            this.Transform.initRenderer("penguin.obj");
            this.Transform.scale(0.5f);
            this.Transform.Translation = startLocation;
            setPhysicsEnabled();
            MyBody.addColliderCube();
            addTag("enemy");
        }
        public override void update()
        {
            base.update();
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(Transform.getRenderer());
        }

        public override void onCollisionEnter(PhysicsBody x)
        {
            if (x.checkTag("player"))
            {
                ((GameThreeDim)Bootstrap.getRunningGame()).playerGotCaught();
            }
        }
    }
}
