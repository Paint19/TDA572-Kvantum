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
        Rat rat;
        Rat rat1;
        Cube aube, bube, cube, dube;
        Teapot teapot;
        SpriteTest spriteTest;
        
        MainCamera mainCamera;
        Player player;
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

            cheeses.Add(new Cheese(new Vector3(0.5f, 0, 0), this));
            cheeses.Add(new Cheese(new Vector3(0.5f, 0f, 1f), this));
            player = new Player(new Vector3(-0.5f, 0, 0));
            

        }

        public override void update()
        {
            mainCamera.update();
            if (cheeses.Count() == 0 && gameCleared == false)
            {
                gameWon();
            }
        }

        public void cheeseGotEaten(Cheese ch)
        {
            cheeses.Remove(ch);
            Console.WriteLine(cheeses.Count() + " cheeses left!");
        }

        private void gameWon()
        {
            gameCleared = true;
            Console.WriteLine("You won!!!!!");
            spriteTest = new SpriteTest(1, 1, "rat_spritesheet.png");
        }
    }
}
