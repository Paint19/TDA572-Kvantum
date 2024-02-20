using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class GameThreeDim : Game, InputListener
    {
        float[] verticesRect = { // TODO: Change to .obj file input
             0.5f,  0.5f, 0.0f,  // top right
             0.5f,  0.4f, 0.0f,  // bottom right
             0.4f,  0.4f, 0.0f,  // bottom left
             0.4f,  0.5f, 0.0f,   // top left
             0.1f,  0.1f, 0.0f,  // top right
             0.1f,  -0.1f, 0.0f,  // bottom right
             -0.1f,  -0.1f, 0.0f,  // bottom left
             -0.1f,  0.1f, 0.0f   // top leftq
        };

        uint[] indices = {  // TODO: change to .obj file input
            0, 1, 3,   // first triangle
            1, 2, 3,    // second triangle
            4, 5, 7,
            5, 6, 2
        };

        public void handleInput(InputEvent inp, string eventType)
        {
            // throw new NotImplementedException();
        }

        public override void initialize()
        {
            // throw new NotImplementedException();
        }

        public override void update()
        {
            Bootstrap.getDisplay().drawShape(verticesRect, indices);
        }
    }
}
