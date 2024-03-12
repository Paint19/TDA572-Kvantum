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
        Cube aube, bube, cube, dube;
        Teapot teapot;
        SpriteTest spriteTest;


        // CAMERA
        private bool goRight = false;
        private bool goLeft = false;
        private bool goUp = false;
        private bool goDown = false;
        private bool goForward = false;
        private bool goBack = false;
        private Vector3 up = Vector3.UnitY;
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 right = Vector3.UnitX;
        private float speed = 4f;
        private Vector3 camPos;

        private Camera camera;

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            camera = new Camera(Bootstrap.getDisplay().getWidth(), Bootstrap.getDisplay().getHeight(), new Vector3(0, 0, 5));

            Bootstrap.getWindow().setActiveCamera(camera);                                    
            
            // Game objects
            rat = new Rat(new Vector3(0.5f, 0,0), new Vector3(-0.001f, 0,0));
            rat.setPhysicsEnabled();
            rat.MyBody.addColliderCube();

            rat1 = new Rat(new Vector3(-0.5f,0,0), new Vector3(0.001f,0,0));
            rat1.setPhysicsEnabled();
            rat1.MyBody.addColliderCube();
            spriteTest = new SpriteTest(1,1, "spritesheet.png");

        }

        public override void update()
        {
            float time = Bootstrap.getWindow().getEventArgsTime();

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
                configMovement(inp, true);

            else if (eventType == "KeyUp")
                configMovement(inp, false);


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
    }
}
