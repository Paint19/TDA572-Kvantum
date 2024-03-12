using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Animation
{
    internal class Animation
    {
        public int numFrames = 1, frameRate = 1, frameOffset = 0, currentFrame = 1;
        public float startTime;
        public bool looping = true;
        public bool vertical = false;

        public Animation(int numFrames, int fr, int foff, bool vert)
        {
            this.numFrames = numFrames;
            frameRate = fr;
            frameOffset = foff;
            vertical = vert;
            startTime = 0;
        }
    }
}
