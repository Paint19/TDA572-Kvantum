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
        SpriteTest spriteTest;


        // CAMERA
        private bool goRight = false;
        private bool goLeft = false;
        private bool goUp = false;
        private bool goDown = false;
        private Vector3 up = Vector3.UnitY;
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 right = Vector3.UnitX;
        private float speed = 4f;
        private Vector3 position;

        private Camera camera;

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            camera = new Camera(Bootstrap.getDisplay().getWidth(), Bootstrap.getDisplay().getHeight(), new Vector3(0, 0, 5));

            Bootstrap.getWindow().setActiveCamera(camera);                                    
            
            // Game objects
            rat = new Rat(-0.001f);
            //rat1 = new Rat(0.001f);
            cube = new Cube();
            //teapot = new Teapot(0.0001f);
            spriteTest = new SpriteTest(1,1, "spritesheet.png");
        }

        public override void update()
        {
            float time = Bootstrap.getWindow().getEventArgsTime();

            position = camera.getPosition();

            if (goLeft)
            {
                position -= right * speed * time;
            }
            if (goRight)
            {
                position += right * speed * time;
            }
            if (goUp)
            {
                position += front * speed * time;
            }
            if (goDown)
            {
                position -= front * speed * time;
            }

            camera.setPosition(position);
        }

        public void handleInput(InputEvent inp, string eventType)
        {

            if (eventType == "KeyDown")
            {

                if (inp.Key == (int)Keys.D)
                {
                    goRight = true;
                }

                if (inp.Key == (int)Keys.A)
                {
                    goLeft = true;
                }
                if (inp.Key == (int)Keys.S)
                {
                    goDown = true;
                }

            }
            else if (eventType == "KeyUp")
            {


                if (inp.Key == (int)Keys.D)
                {
                    goRight = false;
                }
                if (inp.Key == (int)Keys.A)
                {
                    goLeft = false;
                }
                if (inp.Key == (int)Keys.S)
                {
                    goDown = false;
                }
            }


        }
    }
}
