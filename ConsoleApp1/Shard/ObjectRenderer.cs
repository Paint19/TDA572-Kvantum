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
        private int textureVBO;

        private bool initialized = false;
        private float[] vertices, originalVertices;
        private float[] textureCoordinates;

        Texture texture;

        public ObjectRenderer(float[] vertices, float[] textCoords, string texturePath)
        {
            originalVertices = vertices.Select(it => it).ToArray();
            this.vertices = vertices;
            this.textureCoordinates = textCoords;
            if(texturePath is not null)
                texture = new Texture(Bootstrap.getAssetManager().getAssetPath(texturePath));

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            // Bind Vertex Array Object:
             GL.BindVertexArray(VertexArrayObject);

            // Copy our vertices array in a buffer for OpenGL to use:
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // stuff about indices

            // Set our vertex attributes pointers
            // Takes data from the latest bound VBO (memory buffer) bound to ArrayBuffer.
            // The first parameter is the location of the vertex attribute. Defined in shader.vert.
            // Dynamically retrieving shader layout would require some changes.
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            
            // -- Texture -- 

            // Generate a vertice object buffer for the texture coordinates
            textureVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, textureCoordinates.Length * sizeof(float), textureCoordinates, BufferUsageHint.StaticDraw);


            // Put the texture Coordinates in slot 1 of the VAO
            int texCoordLocation = 1;
            //GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexArrayAttrib(VertexArrayObject, texCoordLocation);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            initialized = true;
        }

        public void Bind()
        {
            // Re-binding things:
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            if(texture is not null)
                texture.Use();

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
        }

        public void Render()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);

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
        
        public void setTextCoords(float[] textCoords) 
        { 
            textureCoordinates = textCoords;
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, textureCoordinates.Length * sizeof(float), textureCoordinates);
        }
    }

}
