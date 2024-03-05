/*
*
*   Our game engine functions in 2D, but all its features except for graphics can mostly be extended
*       from existing data structures.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/

using OpenTK.Mathematics;
using System;

namespace Shard
{
    class Transform3D
    {
        private GameObject owner;
        private Vector3 forward, right, up, centre, lastCentre, scale;

        public Transform3D(GameObject owner)
        {
            this.owner = owner;
        }

        public Vector3 getLastDirection()
        {
            return lastCentre - centre;
        }

        public void rotate(float pitch, float yaw, float roll)
        {
            Matrix3 matrix = Matrices.getInstance().getRotationMatrix3(pitch,yaw,roll);
            timesAllCurrentLocation(matrix);
        }

        public void translate(float x, float y, float z)
        {
            translate(new Vector3(x,y,z));
        }
        public void translate(Vector3 offset)
        {
            plusAllCurrentLocation(offset);
        }

        public void reScale(Vector3 scale)
        {
            try {
                Vector3 newScale = scale / this.scale;
                timesAllCurrentLocation(newScale);
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine(e);
                timesAllCurrentLocation(scale);
            }
        }

        private void timesAllCurrentLocation(Vector3 vector) {
            forward = vector * forward;
            right = vector * right;
            up = vector * up;
            centre = vector * centre;
        }
        private void timesAllCurrentLocation(Matrix3 matrix) {
            forward = matrix * forward;
            right = matrix * right;
            up = matrix * up;
            centre = matrix * centre;
        }
        private void plusAllCurrentLocation(Vector3 vector) {
            forward = vector + forward;
            right = vector + right;
            up = vector + up;
            centre = vector + centre;
        }

        public Vector3 Centre{ get => new(centre); }
        public Vector3 Forward { get => new(forward); }
        public Vector3 Right { get => new(right); }
        public Vector3 Up { get => new(up); }
        public Vector3 Scale { get => scale; set => reScale(value); }

        public Transform toTransform()
        {
            throw new NotImplementedException();
        }

    }
}
