using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard
{
    internal class AnimationSystem
    {
        private List<Transform> _toAnimate;
        public void initialize()
        {
            _toAnimate = new List<Transform>();
        }

        public void addToAnimate(GameObject gob)
        {
            _toAnimate.Add(gob.Transform);
        }

        public void update()
        {

        }
    }
}
