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
            aube = new Cube(new Vector3(0.001f,0,0));
            aube.setPhysicsEnabled();
            aube.MyBody.addColliderCube();
            //bube = new Cube();
            //cube = new Cube(new Vector3(0.0005f, 0.0005f, 0.001f));
            //dube = new Cube(new Vector3(-0.002f), Matrices.getInstance().getRotationMatrix3(0.5f, 0.5f, 0.5f));
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
            }


        }
    }
}
