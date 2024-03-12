using System.Collections.Generic;

namespace Shard
{
    class DisplayOpenTK : Display
    {

        List<ObjectRenderer> toDraw = new List<ObjectRenderer>();
        int vao;

        public override void addToDraw(ObjectRenderer gob)
        {
            toDraw.Add(gob);
        }

        public override void clearDisplay()
        {
            toDraw.Clear();
        }

        public override void dispose()
        {
            toDraw.RemoveAll(it => it == null);
            toDraw.ForEach(it => it.Dispose());
            toDraw.Clear();
        }



        public override void display()
        {
            toDraw.RemoveAll(it => it == null);
            toDraw.ForEach(it =>
            {
                it.Bind();
                it.Render();
            });
        }

        public override void initialize()
        {
            // throw new NotImplementedException();
        }

        public override void showText(string text, double x, double y, int size, int r, int g, int b)
        {
            // throw new NotImplementedException();
        }

        public override void showText(char[,] text, double x, double y, int size, int r, int g, int b)
        {
            // throw new NotImplementedException();
        }
    }
}
