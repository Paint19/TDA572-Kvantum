using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Shard;
using System;
using System.Linq;

namespace Shard
{
    class GameThreeDim : Game, InputListener
    {
        private Vector3[] verticesUnprocessed;
        private float[] vertices;
        private uint[] indices;
        private Matrix3 persistentRotationMatrix3;

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
            ObjectFileParser parser = new ObjectFileParser("rat.obj");
            verticesUnprocessed = parser.getVertices();
            camera = new Camera(Bootstrap.getDisplay().getWidth(), Bootstrap.getDisplay().getHeight(), new Vector3(0,0,5));

            indices = parser.getIndices();
            Matrix3 rotMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, 0.0f, 0.250f);
            persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);

            this.vertices = verticesUnprocessed
                    .SelectMany(nVec => new float[] { nVec[0], nVec[1], nVec[2] }).ToArray();

            Bootstrap.getWindow().setActiveCamera(camera);
        }

        public override void update()
        {
            rotateVertices(persistentRotationMatrix3); // TODO: refactor'
            Bootstrap.getDisplay().drawShape(vertices, indices);

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

        void rotateVertices(Matrix3 rotMatrix)
        {
            if (verticesUnprocessed != null)
            {
                this.vertices = verticesUnprocessed
                    .Select(vec => rotMatrix * vec)
                    .SelectMany(nVec => new float[] { nVec[0], nVec[1], nVec[2] }).ToArray();

                verticesUnprocessed = null;
            }
            else
            {
                this.vertices = vertices
                .Chunk(3)
                .Select(vec=> rotMatrix * new Vector3(vec[0], vec[1], vec[2]))
                .SelectMany(nVec => new float[] { nVec[0], nVec[1], nVec[2] }).ToArray();
            }
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
