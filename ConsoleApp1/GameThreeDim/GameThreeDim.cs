using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Shard;
using System;
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
        Cheese cheese;
        Player player;

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
            
            spriteTest = new SpriteTest(1,1, "spritesheet.png");
            */

            cheese = new Cheese(new Vector3(0.5f, 0, 0));
            player = new Player(new Vector3(-0.5f, 0, 0));
            

        }

        public override void update()
        {
            mainCamera.update();
        }
    }
}
