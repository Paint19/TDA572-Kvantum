using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Shard
{
    internal class Player : GameObject, InputListener
    {
        private bool goRight = false;
        private bool goLeft = false;
        private bool goForward = false;
        private bool goBack = false;
        private Vector3 up = Vector3.UnitY;
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 right = Vector3.UnitX;
        private float speed = 2f;
        private int dir = 1;
        private bool isCollidingForward = false;
        private bool isCollidingBackward = false;

        public Player(Vector3 startLocation)
        {
            this.Transform.SpritePath = "white.png";
            this.Transform.InitialColor = new Vector3(1.0f, 0.1f, 0.55f);
            this.Transform.initRenderer("rat.obj");
            this.Transform.scale(0.001f);
            this.Transform.Translation = startLocation;
            setPhysicsEnabled();
            MyBody.addColliderCube();
            addTag("player");
        }
        public override void initialize()
        {
            base.initialize();
            Bootstrap.getInput().addListener(this);
        }
        public override void update()
        {
            float time = Bootstrap.getWindow().getEventArgsTime();
            base.update();

            Vector3 pos = this.Transform.Translation;

            
            if (goForward)
            {
                dir = 1;
                if(!isCollidingForward)
                    pos += front * speed * time;
            }
            if (goBack)
            {
                dir = -1;
                if (!isCollidingBackward)
                    pos -= front * speed * time;
            }
            if (goLeft)
            {
                if (!isCollidingBackward && !isCollidingForward)
                    changeDirection(dir * speed * time);
            }
            if (goRight)
            {
                if (!isCollidingBackward && !isCollidingForward)
                    changeDirection(dir * -speed * time);
            }
            dir = 1;

            this.Transform.Translation = pos;
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(Transform.getRenderer());
        }

        // Player is too fast and ends up inside cube :)
        public override void onCollisionEnter(PhysicsBody x) 
        {
            if (dir == 1)
            {
                isCollidingForward = true;
                isCollidingBackward = false;
            }
            else if (dir == -1)
            {
                isCollidingForward = false;
                isCollidingBackward = true;
            }
        }

        public override void onCollisionExit(PhysicsBody x)
        {
            isCollidingForward = false;
            isCollidingBackward = false;
        }

        public void handleInput(InputEvent inp, string eventType)
        {

            if (eventType == "KeyDown")
                configMovement(inp, true);

            else if (eventType == "KeyUp")
                configMovement(inp, false);
        }

        private void configMovement(InputEvent inp, bool isTrue)
        {
            switch (inp.Key)
            {
                case (int)Keys.Right:
                    goRight = isTrue;
                    break;
                case (int)Keys.Left:
                    goLeft = isTrue;
                    break;
                case (int)Keys.Up:
                    goForward = isTrue;
                    break;
                case (int)Keys.Down:
                    goBack = isTrue;
                    break;
            }
        }

        private void changeDirection(float diff)
        {
            Matrix3 rotMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, 2*diff, 0.0f);
            this.Transform.rotate(rotMatrix);
            front *= Matrix3.Invert(rotMatrix);
        }
    }
}
