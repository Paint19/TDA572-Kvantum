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
            Tuple<float[], uint[]> parsedFile = ObjectFileParser.parseFile("teapot.obj");
            vertices = parsedFile.Item1;
            indices = parsedFile.Item2;
            // throw new NotImplementedException();
        }

        public override void update()
        {
            Bootstrap.getDisplay().drawShape(vertices, indices);
        }
    }
}
