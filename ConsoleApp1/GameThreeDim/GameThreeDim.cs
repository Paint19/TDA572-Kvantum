using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shard
{
    class GameThreeDim : Game
    {
        SpriteTest spriteTest;
        
        MainCamera mainCamera;
        Player player;
        Penguin penguin;
        List<Cheese> cheeses = new List<Cheese>();
        Sphere lightSource;

        private bool gameCleared = false;

        public override void initialize()
        {
            mainCamera = new MainCamera();
            lightSource = new Sphere(new Vector3(0.5f, 2.0f, 0.0f), new Vector3(0));
            lightSource.activateLight();

            cheeses.Add(new Cheese(new Vector3(0.5f, 0, 0)));
            cheeses.Add(new Cheese(new Vector3(0.5f, 0f, 1f)));
            player = new Player(new Vector3(-0.5f, 0, 0));
            penguin = new Penguin(new Vector3(-1f, 0, 0));
        }

        public override void update()
        {
            if(!gameCleared)
                mainCamera.update();

            if (cheeses.Count() == 0 && !gameCleared)
            {
                gameWon();
            }
        }

        public void cheeseGotEaten(Cheese ch)
        {
            cheeses.Remove(ch);
            Console.WriteLine(cheeses.Count() + " cheeses left!");
        }

        public void playerGotCaught()
        {
            Console.WriteLine("Player got Caught!");
            gameLost();
        }

        private void gameWon()
        {
            gameCleared = true;
            Console.WriteLine("You won!!!!!");
            Bootstrap.getInput().removeListener(mainCamera);
            mainCamera.getCamera().setVectors(Vector3.UnitY, -Vector3.UnitZ);
            spriteTest = new SpriteTest(1, 1, 26, "rat_spritesheet.png");
        }

        private void gameLost()
        {
            gameCleared = true;
            Console.WriteLine("You Lost!!!");
            Bootstrap.getInput().removeListener(mainCamera);
            mainCamera.getCamera().setVectors(Vector3.UnitY, -Vector3.UnitZ);
            spriteTest = new SpriteTest(0.8f, 1, 19, "ded_spritesheet.png");
        }
    }
}
