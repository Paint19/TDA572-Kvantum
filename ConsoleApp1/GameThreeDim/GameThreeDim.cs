using OpenTK.Mathematics;

namespace Shard
{
    class GameThreeDim : Game, InputListener
    {
        Vector3[] verticesUnprocessed;
        float[] textureCoordinates;
        float[] vertices;
        Rat rat;
        Rat rat1;
        Cube cube;
        Teapot teapot;

        public void handleInput(InputEvent inp, string eventType)
        {
            // throw new NotImplementedException();
        }

        public override void initialize()
        {
            rat = new Rat(0.0f);
            //rat1 = new Rat(0.001f);
            //cube = new Cube();
            //teapot = new Teapot(0.0001f);
        }


        public override void update()
        {
            // throw new NotImplementedException();
        }
    }
}
