using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shard.Shard
{
    class ObjectFileParser
    {
        /*
         *  parseFile takes a file name (file must be placed in Asset-folder) and 
         *  parses it as an object file.
         */

        private Vector3[] vertices;
        private uint[] indices;
        private uint[] textureIndices;
        private uint[] normalIndices;
        private float[] verticeColor;
        private Vector3[] textureCoordinates;
        private Vector3[] vertexNormals;
        private Vector3[] parameterSpaceCoordinates;
        private uint[] lineElements;

        List<Vector3> verts = new List<Vector3>();
        List<uint> vertInds = new List<uint>();
        List<uint> texInds = new List<uint>();
        List<uint> normInds = new List<uint>();
        List<float> vertCol = new List<float>();
        List<Vector3> texCoords = new List<Vector3>();
        List<Vector3> vertNorms = new List<Vector3>();
        List<Vector3> paramSpaceCoords = new List<Vector3>();
        List<uint> lineElems = new List<uint>();

        public ObjectFileParser(string fileName)
        {
            string filePath = Bootstrap.getAssetManager().getAssetPath(fileName);

            IEnumerable<string> allLines;

            if (File.Exists(filePath))
            {
                //Read all content of the files and store it to the list split with new line 
                allLines = File.ReadLines(filePath);

                foreach (string line in allLines)
                {
                    string[] words = line.Split(' ');

                    switch (words[0])
                    {
                        case "v": // Geometric vertices. x, y, z, w. w defaults to 1 and defines a color value ranging 0 to 1.
                            verts.Add(
                            new Vector3(
                                float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat),
                                float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat),
                                float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat))); 

                            if (words.Length == 5) // w
                                vertCol.Add(float.Parse(words[4], CultureInfo.InvariantCulture.NumberFormat));
                            break;

                        case "vt": // Texture coordinates. u, v, w coordinates, these will vary between 0 and 1. v, w are optional and default to 0.
                            float first = float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat);
                            float second = 0;
                            float third = 0;
                            if (words.Length > 2)
                                second = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
                            if (words.Length > 3)
                                third = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
                            texCoords.Add(new Vector3(first, second, third));
                            break;

                        case "vn": // Vertex normals in (x,y,z) form. Normals might not be unit vectors.
                            vertNorms.Add(
                            new Vector3(
                                float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat),
                                float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat),
                                float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat)));
                            break;

                        case "vp": // Parameter space vertice. In u, v, w form; free form geometry statement 
                            paramSpaceCoords.Add(
                            new Vector3(
                                float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat),
                                float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat),
                                float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat)));
                            break;

                        // Face element. In vertex_index/texture_index/normal_index form. Texture and normal indices are optional
                        // Currently only supports parsing for triangles and quadrilaterals. 
                        case "f":
                            if (words.Length == 4) // Triangle
                            { 
                                for (int i = 1; i < words.Length; i++)
                                {
                                    parseFaceElement(words[i]);
                                }
                            }
                            else if (words.Length == 5) // Quadrilateral
                            {
                                for (int i = 1; i < 4; i++) // Triangle 1
                                {
                                    parseFaceElement(words[i]);
                                }
                                for (int i = 1; i < 5 && i != 2; i++) // Triangle 2
                                {
                                    parseFaceElement(words[i]);
                                }
                            }
                            break;

                        case "l": // Line element, these specify the order of the vertices which build a polyline. 
                            for (int i = 1; i < words.Length; i++)
                                lineElems.Add(uint.Parse(words[i], CultureInfo.InvariantCulture.NumberFormat));
                            break;
                    }

                    // Set all values here
                    vertices = verts.ToArray();
                    indices = vertInds.ToArray();
                    textureIndices = texInds.ToArray();
                    normalIndices = normInds.ToArray();
                    verticeColor = vertCol.ToArray();
                    textureCoordinates = texCoords.ToArray();
                    vertexNormals = vertNorms.ToArray();
                    parameterSpaceCoordinates = paramSpaceCoords.ToArray();
                    lineElements = lineElems.ToArray();
                }
            }
        }
        
        private void parseFaceElement(string word)
        {
            string[] inds = word.Split(new char[] { '/', '/' }, StringSplitOptions.RemoveEmptyEntries);

            vertInds.Add(parseInd(inds[0]));

            if (word.Contains("//"))
                normInds.Add(parseInd(inds[1]));
            else if (inds.Length > 1)
                texInds.Add(parseInd(inds[1]));
            if (inds.Length > 2)
                normInds.Add(parseInd(inds[2]));
        }
        private uint parseInd(string i) { return uint.Parse(i, CultureInfo.InvariantCulture.NumberFormat) - 1; }
        public Vector3[] getVertices() { return vertices; }
        public uint[] getIndices() { return indices; }
        public uint[] getTextureIndices() { return textureIndices; }
        public uint[] getNormalIndices() {  return normalIndices; }
        public float[] getVerticeColor() { return verticeColor; }
        public Vector3[] getTextureCoordinates() { return textureCoordinates; }
        public Vector3[] getVertexNormals() { return vertexNormals; }
        public Vector3[] getParameterSpaceCoordinates() { return parameterSpaceCoordinates; }   
        public uint[] getLineElements() { return lineElements; }    

    }
}
