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
        ObjectFileParser objReader = new ObjectFileParser();

        float[] vertices;
        uint[] indices;

        public void handleInput(InputEvent inp, string eventType)
        {
            // throw new NotImplementedException();
        }

        public override void initialize()
        {
            Tuple<float[], uint[]> parsedFile = objReader.parseFile("teapot.obj");
            vertices = parsedFile.Item1;
            indices = parsedFile.Item2;
            // throw new NotImplementedException();
        }

        public override void update()
        {
            List<float[]> chunks = vertices.Chunk(3).ToList();
            foreach (float[] vert in chunks)
            {
                _3dRot.rotateAll(0.0f, 0.0f, 0.01f, vert);
            }
            vertices = chunks.SelectMany(vert => vert).ToArray();
            Bootstrap.getDisplay().drawShape(vertices, indices);
        }
    }
}
