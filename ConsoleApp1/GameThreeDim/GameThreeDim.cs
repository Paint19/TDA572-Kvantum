using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Shard;
using System;
using System.Linq;

namespace Shard
{
    class GameThreeDim : Game, InputListener
    {
        Rat rat;
        Rat rat1;
        Cube cube;
        Teapot teapot;

        // Room
        Plane wall;

        private float time;

        // CAMERA
        private bool goRight = false;
        private bool goLeft = false;
        private bool goForward = false;
        private bool goBack = false;
        private bool goUp = false;
        private bool goDown = false;
        private Vector3 up = Vector3.UnitY;
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 right = Vector3.UnitX;
        private float speed = 4f;
        private Vector3 camPos;
        private Camera camera;
        // First person shooter style camera controls
        private float pitch;
        private float yaw = -90.0f;
        private bool firstMove = true;
        private float sensitivity = 10f;
        private Vector2 lastPos;
        private float deltaX;
        private float deltaY;

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            camera = new Camera(Bootstrap.getDisplay().getWidth(), Bootstrap.getDisplay().getHeight(), new Vector3(0, 0, 5));

            Bootstrap.getWindow().setActiveCamera(camera);                                    
            
            // Game objects
            rat = new Rat(-0.001f);
            rat1 = new Rat(0.001f);
            cube = new Cube();
            teapot = new Teapot(0.0001f);
            wall = new Plane(1f,1f, "wallpaper.png");
        }

        public override void update()
        {
            time = Bootstrap.getWindow().getEventArgsTime();

            camPos = camera.getPosition();

            if (goLeft)
            {
                camPos -= right * speed * time;
            }
            if (goRight)
            {
                camPos += right * speed * time;
            }
            if (goForward)
            {
                camPos += front * speed * time;
            }
            if (goBack)
            {
                camPos -= front * speed * time;
            }
            if (goUp)
            {
                camPos += up * speed * time;
            }
            if (goDown)
            {
                camPos -= up * speed * time;
            }

           

            camera.setPosition(camPos);
        }

        public void handleInput(InputEvent inp, string eventType)
        {

            if (eventType == "KeyDown")
            {
                configMovement(inp, true);
            }
            else if (eventType == "KeyUp")
            {
                configMovement(inp, false);
            }
            
            if (eventType == "MouseMotion") 
            {
                if (firstMove)
                {
                    lastPos = new Vector2(inp.X, inp.Y);
                    firstMove = false;
                }
                else
                {
                    deltaX = inp.X - lastPos.X;
                    deltaY = inp.Y - lastPos.Y;
                    lastPos = new Vector2(inp.X, inp.Y);

                    // Looking direction:
                    yaw += deltaX * sensitivity * time;
                    pitch -= deltaY * sensitivity * time;
                }
                UpdateVectors();
            }
        }

        private void configMovement(InputEvent inp, bool isTrue)
        {
            switch (inp.Key)
            {
                case (int)Keys.D:
                    goRight = isTrue;
                    break;
                case (int)Keys.A:
                    goLeft = isTrue;
                    break;
                case (int)Keys.W:
                    goForward = isTrue;
                    break;
                case (int)Keys.S:
                    goBack = isTrue;
                    break;
                case (int)Keys.E:
                    goUp = isTrue;
                    break;
                case (int)Keys.Q:
                    goDown = isTrue;
                    break;
            }
        }
        private void UpdateVectors()
        {
            if (pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            if (pitch < -89.0f)
            {
                pitch = -89.0f;
            }
            front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);
            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));

            camera.setVectors(up, front); // updates looking direction

        }

    }
}
