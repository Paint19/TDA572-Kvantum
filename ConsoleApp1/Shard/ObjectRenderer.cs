using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Shard.Shard;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * 
 * This class holds the latest updated vertices for an object.
 * It's probably not great that the vertices are in GameObject.Transform.renderer.
 * However, at this point it's only the renderer which actually uses the vertices.
 * 
 */

namespace Shard
{
    public class ObjectRenderer : IDisposable
    {
        private int VertexBufferObject;
        private int VertexArrayObject;
        private int textureVBO;
        private int colorVBO;
        private int normalVBO;

        private float[] color;
        private float[] normals;

        private bool initialized = false;
        private float[] vertices, originalVertices;
        private float[] textureCoordinates;
        private string spritePath;

        Texture texture;

        public ObjectRenderer(float[] vertices, float[] textCoords, string texturePath, Vector3 colors)
        {
            originalVertices = vertices.Select(it => it).ToArray();
            this.vertices = vertices;
            this.textureCoordinates = textCoords;
            if (texturePath is not null)
                texture = new Texture(Bootstrap.getAssetManager().getAssetPath(texturePath));
            setSolidColor(colors);

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            // Bind Vertex Array Object:
            GL.BindVertexArray(VertexArrayObject);

            // Copy our vertices array in a buffer for OpenGL to use:
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // Set our vertex attributes pointers
            // Takes data from the latest bound VBO (memory buffer) bound to ArrayBuffer.
            // The first parameter is the location of the vertex attribute. Defined in shader.vert.
            // Dynamically retrieving shader layout would require some changes.
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw;

            // -- Texture -- 

            // Generate a vertice object buffer for the texture coordinates
            textureVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, textureCoordinates.Length * sizeof(float), textureCoordinates, bufferUsageHint);


            // Put the texture Coordinates in slot 1 of the VAO
            int texCoordLocation = 1;
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexArrayAttrib(VertexArrayObject, texCoordLocation);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // -- Color -- 
            colorVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorVBO);

            GL.BufferData(BufferTarget.ArrayBuffer, color.Length * sizeof(float), color, bufferUsageHint);
            int colorLocation = 2;
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexArrayAttrib(VertexArrayObject, colorLocation);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // -- Normals -- 
            calculateNormals();
            normalVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVBO);

            GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * sizeof(float), normals, bufferUsageHint);
            int normalLocation = 3;
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexArrayAttrib(VertexArrayObject, normalLocation);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


            initialized = true;
        }

        public void Bind()
        {
            // Re-binding things:
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            if (texture is not null)
                texture.Use();

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVBO);
            // For the normals. Might be wrong :)
            calculateNormals(); 
            GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * sizeof(float), normals, BufferUsageHint.DynamicDraw);
        }

        public void Render()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length / 3);

            // Unbinding buffers
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
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
                    GL.DeleteBuffer(VertexBufferObject);
                    GL.DeleteBuffer(textureVBO);
                    GL.DeleteBuffer(colorVBO);
                    GL.DeleteBuffer(normalVBO);

                    initialized = false;
                }
            }
        }

        private void calculateNormals()
        {
            Vector3[] verts = 
                vertices
                .Chunk(3)
                .Select(vec => new Vector3(vec[0], vec[1], vec[2])).ToArray();

            Vector3[] normalList = new Vector3[verts.Length];

            // Compute normals for each face.
            // Assumes vertices are in the order of their indices.
            // Every 3 consecutive vertices make one triangle in verts.
            for (int i = 0; i < verts.Length; i += 3)
            {
                Vector3 v1 = verts[i];
                Vector3 v2 = verts[i + 1];
                Vector3 v3 = verts[i + 2];

                // The normal is the cross product of two sides of the triangle
                normalList[i] += Vector3.Cross(v2 - v1, v3 - v1);
                normalList[i + 1] += Vector3.Cross(v2 - v1, v3 - v1);
                normalList[i + 2] += Vector3.Cross(v2 - v1, v3 - v1);
            }

            for (int i = 0; i < normalList.Length; i++)
            {
                normalList[i] = normalList[i].Normalized();
            }

            normals = normalList.SelectMany(vec => new float[] { vec[0], vec[1], vec[2] }).ToArray();
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
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void setSpritePath(String path)
        {
            this.spritePath = path;
            texture = new Texture(Bootstrap.getAssetManager().getAssetPath(path));
        }
        public void setColor(float[] col) // Requires an array with a float value for each vertice of the object
        {
            color = col;
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorVBO);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, color.Length * sizeof(float), color);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void setSolidColor(Vector3 solidColor)
        {
            Console.WriteLine("Vertices Length: " + vertices.Length);
            float[] tmpColor = 
                vertices
                .Chunk(3)
                .SelectMany(vec => new float[] { solidColor.X, solidColor.Y, solidColor.Z }).ToArray();
            setColor(tmpColor);
        }

        public float[] getColor() { return color; }
    }

}
