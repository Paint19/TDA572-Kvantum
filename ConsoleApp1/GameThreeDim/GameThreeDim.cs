using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Shard;
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

        private bool gameCleared = false;

        public override void initialize()
        {
            mainCamera = new MainCamera();                     
            
            /*
            // Game objects
            rat = new Rat(new Vector3(0.5f, 0,0), new Vector3(-0.001f, 0,0));
            rat.setPhysicsEnabled();
            rat.MyBody.addColliderCube();

            rat1 = new Rat(new Vector3(-0.5f,0,0), new Vector3(0.001f,0,0));
            rat1.setPhysicsEnabled();
            rat1.MyBody.addColliderCube();
            
            */

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
