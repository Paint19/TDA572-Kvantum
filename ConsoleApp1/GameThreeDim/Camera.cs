using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Camera
    {
        Vector3 cameraPos, cameraTarget, cameraDirection, up, cameraRight, cameraUp;
        Matrix4 view;

        public Camera()
        {
            this.cameraPos = new Vector3(0,0,3.0f);
            this.cameraTarget = Vector3.Zero;
            this.cameraDirection = Vector3.Normalize(cameraPos - cameraTarget);
            this.up = Vector3.UnitZ;
            this.cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            this.cameraUp = Vector3.Cross(cameraDirection, cameraRight);
            this.view = Matrix4.LookAt(
            cameraPos,
            cameraTarget,
            up);

        }
        void fun()
        {
            view = Matrix4.LookAt(cameraPos, cameraTarget, up);
        }
    }
}
