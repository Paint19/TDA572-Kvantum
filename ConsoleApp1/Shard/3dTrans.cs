using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class _3dTrans
    {
        static public void translate(float[] tVec, float[] vCoords) {
            vCoords[0] = vCoords[0] + tVec[0];
            vCoords[1] = vCoords[1] + tVec[1];
            vCoords[2] = vCoords[2] + tVec[2];

        }
    }
}
