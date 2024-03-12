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
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Shard
{
    class Transform3D : Transform
    {
        private double z;
        private double rotx, roty;
        private int scalez;

        private ObjectFileParser objParser;
        private ObjectRenderer renderer;

        public ObjectFileParser getObjParser() { return  objParser; }

        public ObjectRenderer getRenderer() { return renderer; }

        public void initRenderer(string fileName) { 
            ObjectFileParser parser = new ObjectFileParser(fileName);
            uint[] indices = parser.getIndices();
            Vector3[] verts = parser.getVertices();

            // Sort the vertices into an array based on the indices (This is because OpenGl can't take multiple indices)
            List<Vector3> vert = new List<Vector3>();
            foreach (var ind in indices)
                vert.Add(verts[ind]);
            float[] vertices = vert
                    .SelectMany(nVec => new float[] { nVec[0], nVec[1], nVec[2] }).ToArray();

            // Do the same with the texture coordinates
            uint[] textIndices = parser.getTextureIndices();
            Vector3[] textCoords = parser.getTextureCoordinates();
            float[] textureCoordinates;
            if (textIndices.Length > 0)
            {
                List<Vector3> text = new List<Vector3>();
                foreach (var ind in textIndices)
                    text.Add(textCoords[ind]);
            
                textureCoordinates = text.SelectMany(nVec => new float[] { nVec[0], nVec[1] }).ToArray();
            }
            else
                textureCoordinates = textCoords.SelectMany(nVec => new float[] { nVec[0], nVec[1] }).ToArray();

            renderer = new ObjectRenderer(vertices, textureCoordinates, SpritePath);
        }

        public void initRenderer(float[] vertices, float[] textCoords, string spritePath)
        {
            renderer = new ObjectRenderer(vertices, textCoords, spritePath);
        }

        public Transform3D(GameObject o) : base(o)
        {
        }

        public double Z
        {
            get => z;
            set => z = value;
        }



        public int Scalez
        {
            get => scalez;
            set => scalez = value;
        }
        public double Rotx { get => rotx; set => rotx = value; }
        public double Roty { get => roty; set => roty = value; }

        public void rotateVertices(Matrix3 rotMatrix)
        {
            renderer.setVertices(
                renderer.getVertices()
                .Chunk(3)
                .Select(vec => rotMatrix * new Vector3(vec[0], vec[1], vec[2]))
                .SelectMany(nVec => new float[] { nVec[0], nVec[1], nVec[2] }).ToArray()
                );
        }

        public void tmpMove(float scalar)
        {
            renderer.setVertices(renderer.getVertices().Select(n => scalar + n ).ToArray());
        }

        public void tmpChangeSize(float scalar)
        {
            renderer.setVertices(renderer.getVertices().Select(n => scalar * n).ToArray());
        }
    }
}
