using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace Shard.Shard.Animation
{
    internal class AnimationSystem
    {
        private List<GameObject> _toAnimate;

        public void initialize()
        {
            _toAnimate = new List<GameObject>();
        }

        public void addToAnimate(GameObject gob)
        {
            _toAnimate.Add(gob);
        }

        public void update()
        {
            foreach (GameObject gob in _toAnimate)
            {
                Transform t = gob.Transform;
                Animation animation = gob.Animation;

                if (animation.numFrames <= 0)
                    continue;
                
                // if we are not looped and the current from == num frames, skip
                if (!animation.looping && animation.currentFrame >= animation.numFrames - 1)
                    continue;

                // Get the current frame
                animation.currentFrame = (int)(SDL2.SDL.SDL_GetTicks64() - animation.startTime) * animation.frameRate / 1000 % animation.numFrames;

                if (animation.vertical)
                {
                    t.CropX = animation.currentFrame * t.Wid;
                    t.CropY = animation.frameOffset * t.Ht;
                }
                else
                {
                    t.CropX = animation.currentFrame * t.Wid + animation.frameOffset * t.Wid;
                }
                
            }
        }
    }
}
