using OpenTK.Mathematics;
using System;

namespace Shard
{
    class Cube : GameObject
    {
        Matrix3 skewMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, 0.0f, 0.250f);
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);
        Vector3 movementDirection = new Vector3(0,0,0);

        public Cube()
        {
            initCube();
        }
        public Cube(Matrix3 rotation)
        {
            initCube();
            this.persistentRotationMatrix3 = rotation;
        }
        public Cube(Vector3 movementDirection)
        {
            initCube();
            this.movementDirection = movementDirection;
        }
        public Cube(Vector3 movementDirection, Matrix3 rotation)
        {
            initCube();
            this.persistentRotationMatrix3 = rotation;
            this.movementDirection = movementDirection;
        }

        private void initCube()
        {
            Random rand = new Random();
            float r = rand.Next(0, 100) * 0.01f;
            float g = rand.Next(0, 100) * 0.01f;
            float b = rand.Next(0, 100) * 0.01f;
            this.Transform.SpritePath = "white.png";
            this.Transform.InitialColor = new Vector3(r, g, b);
            this.Transform.initRenderer("cube.obj");

            setPhysicsEnabled();
            MyBody.addColliderCube();
        }

        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            // this.Transform.rotate(persistentRotationMatrix3);
            this.Transform.translate(movementDirection);
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(Transform.getRenderer());
        }
    }
}
