using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Cube : GameObject
    {
        Matrix3 skewMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, 0.0f, 0.250f);
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);
        Vector3 movementDirection = new Vector3(0,0,0);

        public Cube()
        {
            this.Transform.initRenderer("cube.obj");
            this.Transform.scale(0.5f);
            this.Transform.rotate(skewMatrix);

        }
        public Cube(Matrix3 rotation)
        {
            this.Transform.initRenderer("cube.obj");
            this.Transform.scale(0.5f);
            this.Transform.rotate(skewMatrix);
            this.persistentRotationMatrix3 = rotation;
        }
        public Cube(Vector3 movementDirection)
        {
            this.Transform.initRenderer("cube.obj");
            this.Transform.scale(0.5f);
            this.Transform.rotate(skewMatrix);
            this.movementDirection = movementDirection;
        }
        public Cube(Vector3 movementDirection, Matrix3 rotation)
        {
            this.Transform.initRenderer("cube.obj");
            this.Transform.scale(0.5f);
            this.Transform.rotate(skewMatrix);
            this.persistentRotationMatrix3 = rotation;
            this.movementDirection = movementDirection;
        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            this.Transform.rotate(persistentRotationMatrix3);
            this.Transform.translate(movementDirection);
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
