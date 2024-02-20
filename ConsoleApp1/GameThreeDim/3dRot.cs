using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    internal class _3dRot
    {

        static void rotateX(float deg, float[] vec) {
            double x = Math.Cos(deg) * vec[0] - Math.Sin(deg) * vec[1];
            double y = Math.Sin(deg) * vec[0] + Math.Cos(deg) * vec[1];
            vec[0] = (float)x;
            vec[1] = (float)y;
        }
        static void rotateY(float deg, float[] vec) {
            double x = Math.Cos(deg) * vec[0] + Math.Sin(deg) * vec[2];
            double z = -Math.Sin(deg) * vec[0] + Math.Cos(deg) * vec[2];
            vec[0] = (float)x;
            vec[2] = (float)z;
        }
        static void rotateZ(float deg, float[] vec) {
            double y = Math.Cos(deg) * vec[1] - Math.Sin(deg) * vec[2];
            double z = Math.Sin(deg) * vec[1] + Math.Cos(deg) * vec[2];
            vec[1] = (float)y;
            vec[2] = (float)z;
        }

        static void rotateAll(float alpha, float beta, float gamma, float[] vec)
        {
            double x = Math.Cos(alpha) * Math.Cos(beta) * vec[0] +
                (Math.Cos(alpha) * Math.Sin(beta) * Math.Sin(gamma) - Math.Sin(alpha) * Math.Cos(gamma)) * vec[1] +
                (Math.Cos(alpha) * Math.Sin(beta) * Math.Cos(gamma) + Math.Sin(alpha) * Math.Sin(gamma)) * vec[2];
            double y = Math.Sin(alpha) * Math.Cos(beta) * vec[0] +
                (Math.Sin(alpha) * Math.Sin(beta) * Math.Sin(gamma) + Math.Cos(alpha) * Math.Cos(gamma)) * vec[1] +
                (Math.Sin(alpha) * Math.Sin(beta) * Math.Cos(gamma) - Math.Cos(alpha) * Math.Sin(gamma)) * vec[2];
            double z = -Math.Sin(beta) * vec[0] + 
                Math.Cos(beta) * Math.Sin(gamma) * vec[1] +
                Math.Cos(beta) * Math.Cos(gamma) * vec[2];
            vec[0] = (float)x;
            vec[1] = (float)y;
            vec[2] = (float)z;
        }
    }
    
}
