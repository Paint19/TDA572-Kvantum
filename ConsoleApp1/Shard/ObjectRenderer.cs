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
        private float[] vertices;
        private uint[] indices;
        private float[] textureCoordinates;

        Texture texture;

        public ObjectRenderer(ObjectFileParser parser, string texturePath)
        {
            indices = parser.getIndices();
            Vector3[] verts = parser.getVertices();
            vertices = verts
                    .SelectMany(nVec => new float[] { nVec[0], nVec[1], nVec[2] }).ToArray();
            textureCoordinates = parser.getTextureCoordinates().SelectMany(nVec => new float[] { nVec[0], nVec[1] }).ToArray();
            if(texturePath is not null)
                texture = new Texture(Bootstrap.getAssetManager().getAssetPath(texturePath));

            mergeVerticesWithTextCoord();

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
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0); 

            int texCoordLocation = 1;
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(0);

            initialized = true;
        }

        public void Bind()
        {
            // Re-binding things:
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            if(texture is not null)
                texture.Use();

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
        public void setVertices(float[] verts) { vertices = verts; }
        private void mergeVerticesWithTextCoord()
        {
            List<float> verts = new List<float>();
            for (int i = 0; i < vertices.Length / 3; i++)
            {
                verts.Add(vertices[i * 3]);
                verts.Add(vertices[i * 3 + 1]);
                verts.Add(vertices[i * 3 + 2]);
                if (textureCoordinates.Length > 2 * i)
                {
                    verts.Add(textureCoordinates[i * 2]);
                    verts.Add(textureCoordinates[i * 2 + 1]);
                }
                else
                {
                    verts.Add(0);
                    verts.Add(0);
                }
            }
            vertices = verts.ToArray();

        }
    }

}
