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

        public override void drawShape(float[] vertices, uint[] indices, float[] textureCoords)
        {
            List<float> verts = new List<float>();
            for (int i = 0; i < vertices.Length/3; i++)
            {
                verts.Add(vertices[i*3]);
                verts.Add(vertices[i * 3 + 1]);
                verts.Add(vertices[i * 3 + 2]);
                if (textureCoords.Length > 2*i)
                {
                verts.Add(textureCoords[i * 2]);
                verts.Add(textureCoords[i * 2 + 1]);
                }
                else
                {
                    verts.Add(0);
                    verts.Add(0);
                }
            }
            vertices = verts.ToArray();

            Bootstrap.getWindow().setIndicesLength(indices.Length); // sus
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.DynamicDraw);
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
