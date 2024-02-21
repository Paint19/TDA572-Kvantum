using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Shard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class GameThreeDim : Game, InputListener
    {
        float[] vertices;
        uint[] indices;
        Matrix3 persistentRotationMatrix3;
        public void handleInput(InputEvent inp, string eventType)
        {
            // throw new NotImplementedException();
        }

        public override void initialize()
        {
            Tuple<float[], uint[]> parsedFile = ObjectFileParser.parseFile("rat.obj");
            vertices = parsedFile.Item1;
            indices = parsedFile.Item2;
            Matrix3 rotMatrix = Matrices.getInstance().getRotationMatrix3(0.0f, 0.0f, 0.250f);
            persistentRotationMatrix3 = Matrices.getInstance().getRotationMatrix3(0.0f, 0.01f, 0.0f);
            rotateVertices(rotMatrix);
            // throw new NotImplementedException();
        }
 
        void rotateVertices(Matrix3 rotMatrix)
        {
            this.vertices = vertices
                .Chunk(3)
                .Select(vec=> rotMatrix * new Vector3(vec[0], vec[1], vec[2]))
                .SelectMany(nVec => new float[] { nVec[0], nVec[1], nVec[2] }).ToArray();
        }

        public override void update()
        {
            rotateVertices(persistentRotationMatrix3);
            Bootstrap.getDisplay().drawShape(vertices, indices);
        }
    }
}
