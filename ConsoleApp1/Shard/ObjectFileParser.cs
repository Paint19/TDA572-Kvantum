using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard
{
    class ObjectFileParser
    {
        /*
         *  parseFile takes a file name (file must be placed in Asset-folder) and 
         *  parses it as an object file. Right now it only parses the simplest 
         *  type of .obj file, which contains vertices coordinates and faces 
         *  (which three vertices form a triangle)
         */
        static public Tuple<float[], uint[]> parseFile(string fileName)
        {
            string filePath = Bootstrap.getAssetManager().getAssetPath(fileName);
            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();

            const float epsilon = 0.000001f;

            IEnumerable<string> allLines;
            if (File.Exists(filePath))
            {
                //Read all content of the files and store it to the list split with new line 
                allLines = File.ReadLines(filePath);

                foreach (string line in allLines)
                {
                    string[] vals = line.Split(' ');

                    if (vals[0].Equals("v")) {
                        for(int i = 1; i < vals.Length; i++)
                        {
                            float v = float.Parse(vals[i], CultureInfo.InvariantCulture.NumberFormat) * 0.1f;  //TODO: Remove *0.1f // Must be in "0.00..." format
                            if (Math.Abs(v) < epsilon) // TODO: think abuout
                                v = 0.0f;
                            vertices.Add(v);
                        }
                    }
                    else if (vals[0].Equals("f"))
                    {
                        for (int i = 1; i < vals.Length; i++)
                        {
                            indices.Add(uint.Parse(vals[i], CultureInfo.InvariantCulture.NumberFormat));
                        }
                    }
                }

            }

            float[] vs = vertices.ToArray();
            uint[] inds = indices.ToArray();

            return new Tuple<float[], uint[]>(vs, inds);
        }
    }
}
