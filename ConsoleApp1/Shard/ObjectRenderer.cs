using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Shard.Shard;

/*
 * 
 * This class holds the latest updated vertices for an object.
 * It's probably not great that the vertices are in GameObject.Transform.renderer.
 * However, at this point it's only the renderer which actually uses the vertices.
 * 
 */

namespace Shard
{
    class ObjectRenderer : IDisposable
    {
        private int VertexBufferObject;
        private int ElementBufferObject;
        private int VertexArrayObject;

        private bool initialized = false;
        private float[] vertices, originalVertices;
        private uint[] indices;

        public ObjectRenderer(ObjectFileParser parser)
        {
            indices = parser.getIndices();
            Vector3[] verts = parser.getVertices();
            vertices = verts
                    .SelectMany(nVec => new float[] { nVec[0], nVec[1], nVec[2] }).ToArray();
            originalVertices = vertices.Select(it => it).ToArray();

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            // Bind Vertex Array Object:
             GL.BindVertexArray(VertexArrayObject);

            // Copy our vertices array in a buffer for OpenGL to use:
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // stuff about indices
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw); 

            // Set our vertex attributes pointers
            // Takes data from the latest bound VBO (memory buffer) bound to ArrayBuffer.
            // The first parameter is the location of the vertex attribute. Defined in shader.vert.
            // Dynamically retrieving shader layout would require some changes.
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.EnableVertexAttribArray(0);

            initialized = true;
        }

        public void Bind()
        {
            // Re-binding things:
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
        }

        public void Render()
        {
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0); // For DrawShape rather than drawtriangle

            // Unbinding buffers
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (initialized)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    GL.DeleteBuffer(VertexBufferObject);

                    initialized = false;
                }
            }
        }

        public float[] getVertices() { return vertices; }

        public float[] Vertices { get { return vertices; } }

        public float[] OriginalVertices { get { return originalVertices; } }
        public void setVertices(float[] verts) { vertices = verts; }
    }
}
