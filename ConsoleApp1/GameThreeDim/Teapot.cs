using OpenTK.Mathematics;

namespace Shard
{
    class Teapot : GameObject
    {
        Matrix3 persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);

        public Teapot(Vector3 pos)
        {
            this.Transform.initRenderer("teapot.obj");
            this.Transform.scale(0.1f);
            Transform.translate(pos);
        }
        public override void initialize()
        {
            base.initialize();
        }

        public override void update()
        {
            base.update();
            this.Transform.rotate(persistentRotationMatrix3);
            this.Transform.calculateVertices();
            this.Transform.setCalculatedVerticesToRender();
            Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
