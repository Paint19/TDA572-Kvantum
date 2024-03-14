using OpenTK.Mathematics;

namespace Shard
{
    class Penguin : GameObject
    {
        private bool goRight = false;
        private bool goLeft = false;
        private bool goForward = false;
        private bool goBack = false;
        private Vector3 front = Vector3.UnitZ;
        private float speed = 2f;
        private int dir = 1;

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
            float time = Bootstrap.getWindow().getEventArgsTime();
            base.update();

            Vector3 pos = this.Transform.Translation;


            if (goForward)
            {
                dir = 1;
                pos += front * speed * time;
            }
            if (goBack)
            {
                dir = -1;
                pos -= front * speed * time;
            }
            if (goLeft)
            {
                changeDirection(dir * speed * time);
            }
            if (goRight)
            {
                changeDirection(dir * -speed * time);
            }
            dir = 1;

            this.Transform.Translation = pos;
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
        private void changeDirection(float diff)
        {
            Matrix3 rotMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, 2 * diff, 0.0f);
            this.Transform.rotate(rotMatrix);
            front *= Matrix3.Invert(rotMatrix);
        }

        public bool GoForward { get => goForward; set => goForward = value; }
        public bool GoBack { get => goBack; set => goBack = value; }
        public bool GoRight { get => goRight; set => goRight = value; }
        public bool GoLeft { get => goLeft; set => goLeft = value; }

    }
}
