using OpenTK.Mathematics;

namespace Shard
{
    class Penguin : GameObject
    {
        public Penguin(Vector3 startLocation) {
            this.Transform.SpritePath = "penguin.png";
            // Colors must be set to white for each vertex if only texture is to be shown
            this.Transform.InitialColor = new Vector3(1.0f, 1.0f, 1.0f);

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
