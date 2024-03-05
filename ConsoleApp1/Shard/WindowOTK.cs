using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Shard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

/*
 *  Here we have a lot of OpenTK magic. 
 *  GameWindow has a bunch of functions that are called when updating/rendering/loading/unloading the window (and more)
 */

namespace Shard
{
    public class WindowOTK : GameWindow
    {

        private static int VertexBufferObject;
        private static int ElementBufferObject;
        private static int VertexArrayObject;

        private static int indicesLength;

        Shader shader;

        public WindowOTK(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        public void setIndicesLength(int length) { indicesLength = length; }

        protected override void OnUpdateFrame(FrameEventArgs args) // Runs when GameWindow updates frame
        {

            base.OnUpdateFrame(args);

            long timeInMillisecondsStart, timeInMillisecondsEnd;
            timeInMillisecondsStart = Bootstrap.getCurrentMillis();

            Bootstrap.getDisplay().clearDisplay();

            PhysicsManager phys = Bootstrap.getPhysicsManager();

            if (Bootstrap.getRunningGame().isRunning() == true)
            {

                // Get input, which works at 50 FPS to make sure it doesn't interfere with the 
                // variable frame rates.
                Bootstrap.getInput().getInput(); // TODO: This probably needs fixing!

                // Update runs as fast as the system lets it.  Any kind of movement or counter 
                // increment should be based then on the deltaTime variable.
                GameObjectManager.getInstance().update();

                // This will update every 20 milliseconds or thereabouts.  Our physics system aims 
                // at a 50 FPS cycle.
                if (phys.willTick())
                {
                    GameObjectManager.getInstance().prePhysicsUpdate();
                }

                // Update the physics.  If it's too soon, it'll return false.   Otherwise 
                // it'll return true.
                bool physUpdate = phys.update();

                if (physUpdate)
                {
                    // If it did tick, give every object an update
                    // that is pinned to the timing of the physics system.
                    GameObjectManager.getInstance().physicsUpdate();
                }

                if (Bootstrap.getPhysDebug())
                {
                    phys.drawDebugColliders();
                }

            }

            Bootstrap.getRunningGame().update();

            timeInMillisecondsEnd = Bootstrap.getCurrentMillis();
            Bootstrap.setDeltaTime((timeInMillisecondsEnd - timeInMillisecondsStart) / 1000.0f); // Dunno if this is right. Not sure what deltaTime should be

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // Clear screen before re-rendering
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();

            // Display.display() renders all game objects
            Bootstrap.getDisplay().display();

            // Display what has been rendering. Must be last. Double-buffering avoids screen tearing.
            SwapBuffers(); 
        }

        // Runs (only once) when GameWindow loads the window
        protected override void OnLoad() 
        {
            base.OnLoad();

            // Sets the color of the window "between frames"
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);      // Redundant?

            // Start the game running. Must be done after starting the game loop
            Bootstrap.getRunningGame().initialize();

            // Initialize display
            Bootstrap.getDisplay().initialize();

            shader = new Shader("../../../Shard/shader.vert", "../../../Shard/shader.frag");
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Bootstrap.getDisplay().dispose();
            shader.Dispose();
        }

    }
}
