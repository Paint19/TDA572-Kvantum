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
            objParser = new ObjectFileParser(fileName);
            renderer = new ObjectRenderer(objParser);
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
