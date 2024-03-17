using OpenTK.Mathematics;
using System;

namespace Shard
{
    class Penguin : GameObject
    {
        private Vector3 targetPosition;
        private bool goRight = false;
        private bool goLeft = false;
        private bool goForward = false;
        private bool goBack = false;
        private Vector3 front = Vector3.UnitZ;
        private Vector3 up = Vector3.UnitY;
        private float speed = 0.1f;
        private int dir = 1;

        public Penguin(Vector3 startLocation, Vector3 playerPosition) {
            this.Transform.SpritePath = "penguin.png";
            // Colors must be set to white for each vertex if only texture is to be shown
            this.Transform.InitialColor = new Vector3(1.0f, 1.0f, 1.0f);

            targetPosition = playerPosition;
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
            Vector3 direction = Vector3.Normalize(targetPosition - pos);


            Vector3 rightDirection = Vector3.Cross(front, up);

            float dot = Vector3.Dot(rightDirection, direction); // dot < 0: rat left of penguin, dot > 0: rat right of penguin

            float angle = Vector3.CalculateAngle(front, direction); // In front of penguin: 0 rad, right behind: pi rad. 

            if (dot < 0) // going left
                changeDirection(angle);
            else // going right
                changeDirection(-angle);

            pos += front * speed;

            this.Transform.Translation = pos;
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(Transform.getRenderer());
        }

        public void setTarget(Vector3 playerPosition)
        {
            targetPosition = playerPosition;
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
            Matrix3 rotMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, diff, 0.0f);
            this.Transform.rotate(rotMatrix);
            front *= Matrix3.Invert(rotMatrix);
        }

        public bool GoForward { get => goForward; set => goForward = value; }
        public bool GoBack { get => goBack; set => goBack = value; }
        public bool GoRight { get => goRight; set => goRight = value; }
        public bool GoLeft { get => goLeft; set => goLeft = value; }

    }
}
