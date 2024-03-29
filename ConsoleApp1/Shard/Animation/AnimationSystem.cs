﻿using System.Collections.Generic;

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
            foreach (Sprite sprite in _toAnimate)
            {

                Animation animation = sprite.Animation;

                if (animation.numFrames <= 0)
                    continue;

                // if we are not looped and the current from == num frames, skip
                if (!animation.looping && animation.currentFrame >= animation.numFrames - 1)
                    continue;

                // Get the current frame
                animation.currentFrame = (int)(Bootstrap.getTimeSinceSetup() - animation.startTime) * animation.frameRate / 1000 % animation.numFrames;


                float cropX, cropY = 0;
                float size = 1f / animation.numFrames;


                if (animation.vertical)
                {
                    cropX = animation.currentFrame;
                    cropY = animation.frameOffset * size;
                }
                else
                {
                    cropX = animation.currentFrame * size + animation.frameOffset * size;
                }

                sprite.crop(cropX, cropY, 1, size);
            }
        }
    }
}
