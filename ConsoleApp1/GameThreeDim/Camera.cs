using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace Shard
{
    internal class Camera : InputListener
    {
        private float speed = 8f;
        private float screenwidth;
        private float screenheight;
        private float sensitivity = 10f;

        public Vector3 position;

        Vector3 up = Vector3.UnitY;
        Vector3 front = -Vector3.UnitZ;
        Vector3 right = Vector3.UnitX;

        private float pitch;
        private float yaw = -90.0f;

        private bool firstMove = true;
        public Vector2 lastPos;

        bool goRight = false;
        bool goLeft = false;
        public Camera(float width, float height, Vector3 position)
        {
            screenwidth = width;
            screenheight = height;
            this.position = position;
            Bootstrap.getInput().addListener(this);
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(position, position + front, up);
        }
        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), screenwidth / screenheight, 0.1f, 100.0f);
        }

        private void UpdateVectors()
        {
            if (pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            if (pitch < -89.0f)
            {
                pitch = -89.0f;
            }


            front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

            front = Vector3.Normalize(front);

            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {

            if (input.IsKeyDown(Keys.W))
            {
                position += front * speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                position -= right * speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                position -= front * speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                position += right * speed * (float)e.Time;
            }

            if (input.IsKeyDown(Keys.Space))
            {
                position.Y += speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                position.Y -= speed * (float)e.Time;
            }

            if (firstMove)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                yaw += deltaX * sensitivity * (float)e.Time;
                pitch -= deltaY * sensitivity * (float)e.Time;
            }
            UpdateVectors();
        }
        public void Update(FrameEventArgs e)
        {
            //InputController(input, mouse, e);
            if (goLeft)
            {
                position -= right * speed * (float)e.Time;
            }
            if (goRight)
            {
                position += right * speed * (float)e.Time;
            }
        }

        public void handleInput(InputEvent inp, string eventType)
        {

            if (eventType == "KeyDown")
            {

                if (inp.Key == (int)Keys.D)
                {
                    goRight = true;
                }

                if (inp.Key == (int)Keys.A)
                {
                    goLeft = true;
                }

            }
            else if (eventType == "KeyUp")
            {


                if (inp.Key == (int)Keys.D)
                {
                    goRight = false;
                }

                if (inp.Key == (int)Keys.A)
                {
                    goLeft = false;
                }


            }
        }
    }
}