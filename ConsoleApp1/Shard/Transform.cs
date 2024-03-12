/*
*
*   Our game engine functions in 2D, but all its features except for graphics can mostly be extended
*       from existing data structures.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/

using OpenTK.Mathematics;
using Shard.Shard;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Security;

namespace Shard
{
    class Transform
    {
        private Matrix4 matrix;
        private float[] calculatedVertices;
        private Vector3 lastLocation;
        private ObjectFileParser objParser;
        private ObjectRenderer renderer;

        public ObjectFileParser getObjParser() { return  objParser; }

        public ObjectRenderer getRenderer() { return renderer; }

        public void initRenderer(string fileName) { 
            objParser = new ObjectFileParser(fileName);
            renderer = new ObjectRenderer(objParser);
            calculateVertices();
        }

        public void initRenderer(float[] vertices, uint[] indices)
        {
            this.calculatedVertices = vertices;
            renderer = new ObjectRenderer(vertices, indices);
            calculateVertices();
        }
        public Transform()
        {
            this.matrix = Matrix4.Identity;
            this.lastLocation = Vector3.Zero;

        }
        public Transform(Matrix4 translateAndRotate)
        {
            this.matrix = new Matrix4(
                translateAndRotate.Row0, 
                translateAndRotate.Row1, 
                translateAndRotate.Row2, 
                translateAndRotate.Row3);
            lastLocation = Vector3.Zero;
        }
        public Vector3 Right { get => new Vector3(matrix.Row0); }
        public Vector3 Up { get =>  new Vector3(matrix.Row1); }
        public Vector3 Forward { get =>  new Vector3(matrix.Row2); }
        public Vector3 Scale { get => matrix.ExtractScale();
            set 
            {
                Matrix4 diagonal = Matrix4.Identity;
                diagonal.Diagonal = new Vector4(value,1);
                matrix = diagonal * matrix;
            }
        }
        public Vector3 Translation {
            get => new Vector3(matrix.Column3);
            set => matrix.Column3 = new Vector4(value, 1);
        }

        public Matrix3 Orientation
        {
            get => new Matrix3(matrix);
            set 
            {
                Vector3 translation = Translation;
                matrix = new Matrix4(value);
                Translation = translation;
            }
        }
        public Vector3 getLastDirection()
        {
            return lastLocation - Translation;
        }

        public Matrix4 getConvenientMath()
        {
            return new Matrix4(
                matrix.Row0, 
                matrix.Row1, 
                matrix.Row2, 
                matrix.Row3);
        }

        public void calculateVertices()
        {
            calculatedVertices = renderer
                .OriginalVertices
                .Chunk(3)
                .Select(vert => matrix * new Vector4(vert[0], vert[1], vert[2], 1))
                .SelectMany(vec => new float[] { vec.X, vec.Y, vec.Z }).ToArray();
        }

        public IEnumerable<Vector3> getVerticesAsVectors()
        {
            return renderer
                .OriginalVertices
                .Chunk(3)
                .Select(arr => new Vector3(arr[0], arr[1], arr[2]));
        }

        public void setCalculatedVerticesToRender()
        {
            renderer.setVertices(calculatedVertices);
        }

        public void setCalculatedVerticesToRender(float[] vertices)
        {
            renderer.setVertices(vertices);
        }
        public void rotate(float pitch, float yaw, float roll)
        {
            Matrix3 matrix = Matrices.getInstance().getRotationMatrix3(pitch, yaw, roll);
            rotate(matrix);
        }
        public void rotate(Matrix3 rotMatrix)
        {
            Orientation = rotMatrix * Orientation;
        }
        public void translate(Vector3 translation)
        {
            lastLocation = Translation;
            Translation = lastLocation + translation;
        }

        public void translateAbsolute(Vector3 location)
        {
            Translation = location;
        }

        public void scale(Vector3 scale)
        {
            Scale = scale;
        }
        public void scale(float scalar)
        {
            scale(new Vector3(scalar));
        }

        public float[] Vertices { get => calculatedVertices; }
    }
}
