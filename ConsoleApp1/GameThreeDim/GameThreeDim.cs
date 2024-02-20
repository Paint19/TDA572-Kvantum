using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Shard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class GameThreeDim : Game, InputListener
    {
        float[] vertices;
        uint[] indices;

        public void handleInput(InputEvent inp, string eventType)
        {
            // throw new NotImplementedException();
        }

        public override void initialize()
        {
            Tuple<float[], uint[]> parsedFile = ObjectFileParser.parseFile("rat.obj");
            vertices = parsedFile.Item1;
            indices = parsedFile.Item2;
            rotateVertices(0.0f, 0.0f, 3.0f / (4.0f * (float)Math.PI));
            // throw new NotImplementedException();
        }

        public void rotateVertices(float a, float b, float c)
        {
            List<float[]> chunks = vertices.Chunk(3).ToList();
            foreach (float[] vert in chunks)
            {
                _3dRot.rotateAll(a, b, c, vert);
            }
            vertices = chunks.SelectMany(vert => vert).ToArray();
        }

        public override void update()
        {
            rotateVertices(0.0f, 0.01f, 0.0f);
            Bootstrap.getDisplay().drawShape(vertices, indices);
        }
    }
}
