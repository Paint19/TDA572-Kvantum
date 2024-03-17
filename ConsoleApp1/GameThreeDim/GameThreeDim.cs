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
        List<Cube> cubes = new List<Cube>();
        Sphere lightSource;

        private bool gameCleared = false;

        public override void initialize()
        {
            mainCamera = new MainCamera();

            // Objekt mellan ca -3 och 3 i både X och Z-led syns inom kamerans startposition
            lightSource = new Sphere(new Vector3(0.5f, 2.0f, 0.0f), new Vector3(0));
            lightSource.activateLight();
            player = new Player(new Vector3(-0.5f, 0, 0));
            penguin = new Penguin(new Vector3(-1f, 0, 0));

            int nrCheeses = 5;
            for (int i = 0; i < nrCheeses; i++)
                cheeses.Add(new Cheese(getRandomPosition(-3, 3)));

            int nrCubes = 3;
            for (int i = 0; i < nrCubes; i++) { 
                cubes.Add(new Cube());
                cubes[i].Transform.translate(new Vector3(getRandomPosition(-3, 3)));
            }

            // Hårdkodat: Pingvinen åker runt i en cirkel
            penguin.GoForward = true;
            penguin.GoRight = true;
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

        private Vector3 getRandomPosition(float min,  float max)
        {
            Random rand = new Random();
            int maxCoord = (int)(max * 100);
            int minCoord = (int)(min * 100);
            float xPos = rand.Next(minCoord, maxCoord) * 0.01f;
            float zPos = rand.Next(minCoord, maxCoord) * 0.01f;
            return new Vector3(xPos, 0.0f, zPos);
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
