using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

            if (goLeft)
            {
                pos -= right * speed * time;
            }
            if (goRight)
            {
                pos += right * speed * time;
            }
            if (goForward)
            {
                pos += front * speed * time;
            }
            if (goBack)
            {
                pos -= front * speed * time;
            }
            this.Transform.Translation = pos;
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(Transform.getRenderer());
        }

        public override void onCollisionEnter(PhysicsBody x)
        {
            Console.WriteLine("Rat Attack!!!");
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
    }
}
