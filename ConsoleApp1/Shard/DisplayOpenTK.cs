using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class DisplayOpenTK : Display
    {

        List<GameObject> toDraw = new List<GameObject>();
        int vao;

        public override void addToDraw(GameObject gob)
        {
            toDraw.Add(gob);
        }

        public override void clearDisplay()
        {
            toDraw.Clear();
        }

        public override void dispose()
        {
            foreach (GameObject go in toDraw)
            {
                ObjectRenderer renderer = go.Transform.getRenderer();
                if (renderer != null)
                {
                    renderer.Dispose();
                }
            }
            toDraw.Clear();
            // throw new NotImplementedException();
        }


        
        public override void display()
        {
            foreach (GameObject go in toDraw)
            {                
                ObjectRenderer renderer = go.Transform.getRenderer();
                if (renderer != null)
                {
                    renderer.Bind();
                    renderer.Render();
                }
            }
            // throw new NotImplementedException();
        }

        public override void initialize()
        {
            // throw new NotImplementedException();
        }

        public override void showText(string text, double x, double y, int size, int r, int g, int b)
        {
            // throw new NotImplementedException();
        }

        public override void showText(char[,] text, double x, double y, int size, int r, int g, int b)
        {
            // throw new NotImplementedException();
        }
    }
}
