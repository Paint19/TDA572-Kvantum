using OpenTK.Mathematics;
using System;

namespace Shard
{
    // This sphere is used as a light source
    class Sphere : GameObject
    {
        Vector3 moveDirection;

        public Sphere(Vector3 startLocation, Vector3 direction)
        {
            this.Transform.SpritePath = "white.png";
            this.Transform.InitialColor = new Vector3(0.0f, 0.0f, 1.0f);
            this.Transform.initRenderer("sphere.obj");
            this.Transform.scale(0.1f);
            this.Transform.Translation = startLocation;
            this.moveDirection = direction;
        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            this.Transform.translate(moveDirection);
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();

            // Perhaps addToDraw and addLight should be in GameObject?
            if (IsLightSource)
                Bootstrap.getDisplay().addLightToDraw(Transform.getRenderer());
            else
                Bootstrap.getDisplay().addToDraw(Transform.getRenderer());
        }

        public void activateLight() // This is not great
        {
            IsLightSource = true;
            Bootstrap.getWindow().addLight(this.Transform);
        }

        public void deactivateLight() // This is not great
        {
            IsLightSource = false;
            Bootstrap.getWindow().removeLight();
        }
    }
}
