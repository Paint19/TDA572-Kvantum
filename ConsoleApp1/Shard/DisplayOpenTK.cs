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
        
        public override void drawTriangle(float[] vertices)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
        }

        public override void clearDisplay()
        {
            // throw new NotImplementedException();
        }

        public override void display()
        {
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
