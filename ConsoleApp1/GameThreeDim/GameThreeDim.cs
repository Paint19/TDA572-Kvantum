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
        float[] vertices = {    // TODO: This is hard-coded. Make dynamic.
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f,  //Bottom-right vertex
            0.0f,  0.5f, 0.0f   //Top vertex
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
            Bootstrap.getDisplay().drawTriangle(vertices);
        }
    }
}
