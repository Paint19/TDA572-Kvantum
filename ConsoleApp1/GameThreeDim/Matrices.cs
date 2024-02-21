using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Matrices
    {
        private static Matrices instance;

        private Matrices() { }

        public static Matrices getInstance()
        {
            if (instance == null)
                instance = new Matrices();
            return instance;
        }

        public Matrix3 getRotationMatrix3 (float alpha, float beta, float gamma)
        {
            return new Matrix3(
                new Vector3((float) (Math.Cos(alpha) * Math.Cos(beta)),
                (float)(Math.Cos(alpha) * Math.Sin(beta) * Math.Sin(gamma) - Math.Sin(alpha) * Math.Cos(gamma)),
                (float)(Math.Cos(alpha) * Math.Sin(beta) * Math.Cos(gamma) + Math.Sin(alpha) * Math.Sin(gamma))),
                new Vector3((float)(Math.Sin(alpha) * Math.Cos(beta)),
                (float)(Math.Sin(alpha) * Math.Sin(beta) * Math.Sin(gamma) + Math.Cos(alpha) * Math.Cos(gamma)),
                (float)(Math.Sin(alpha) * Math.Sin(beta) * Math.Cos(gamma) - Math.Cos(alpha) * Math.Sin(gamma))),
                new Vector3(-(float)Math.Sin(beta),
                (float)(Math.Cos(beta) * Math.Sin(gamma)),
                (float)(Math.Cos(beta) * Math.Cos(gamma)))
                );
        }
    }
}
