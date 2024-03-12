using OpenTK.Mathematics;

namespace Shard
{
    public class Camera
    {
        private float screenwidth;
        private float screenheight;

        public Vector3 position;

        Vector3 up = Vector3.UnitY;
        Vector3 front = -Vector3.UnitZ;


        public Vector2 lastPos;

        public Camera(float width, float height, Vector3 position)
        {
            screenwidth = width;
            screenheight = height;
            this.position = position;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(position, position + front, up);
        }
        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), screenwidth / screenheight, 0.1f, 100.0f);
        }

        public void setPosition(Vector3 pos)
        {
            position = pos;
        }

        public void setVectors(Vector3 up, Vector3 front)
        {
            this.up = up;
            this.front = front;
        }

        public Vector3 getPosition() { return position; }

    }
}