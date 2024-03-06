using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard
{
    internal class Sprite : GameObject
    {
        
        private string spritePath;
        private float[] vertices;
        private float[] textCoords;
        private uint[] indices;
        
        float height = 1, width = 1;

        public Sprite(float h, float w, float cropX, float cropY, float cropH, float cropW, String spritePath)
        {
            this.spritePath = spritePath;
            
            float xOf = w / 2;
            float yOf = h / 2;
            vertices =
            [
                //Position          
                xOf, yOf, 0.0f,  // top right
                xOf, -yOf, 0.0f,  // bottom right
                -xOf, -yOf, 0.0f,  // bottom left
                -xOf, yOf, 0.0f  // top left
            ];
            textCoords =
            [
                1- cropX, 1- cropY,
                1- cropX, 1- cropH,
                1- cropW, 1- cropH,
                1- cropW, 1- cropY
            ];
            indices = [0, 1, 2, 2, 3, 0];
            Transform.initRenderer(vertices, indices, textCoords, spritePath);
        }

        public void crop(float cropX, float cropY, float cropH, float cropW)
        {
            //Console.WriteLine("Cropped!");
            textCoords =
            [
                1- cropX,           1- cropY,
                1- cropX,           1 - cropY - cropH,
                1- cropX- cropW,    1- cropY- cropH,
                1- cropX- cropW,    1- cropY
            ];
            Transform.getRenderer().setTextCoords(textCoords);
        }
        
    }
}
