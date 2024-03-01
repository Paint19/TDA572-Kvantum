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
        Texture texture;

        public WindowOTK(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        public void setIndicesLength(int length) { indicesLength = length; }

        protected override void OnUpdateFrame(FrameEventArgs args) // Runs when GameWindow updates frame
        {

            base.OnUpdateFrame(args);

            long timeInMillisecondsStart, timeInMillisecondsEnd;
            timeInMillisecondsStart = Bootstrap.getCurrentMillis();

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

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            // Bind Vertex Array Object:
            GL.BindVertexArray(VertexArrayObject);

            // Copy our vertices array in a buffer for OpenGL to use:
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw); TODO: Remove

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw); TODO: Remove

            // Set our vertex attributes pointers
            // Takes data from the latest bound VBO (memory buffer) bound to ArrayBuffer.
            // The first parameter is the location of the vertex attribute. Defined in shader.vert. Dynamically retrieving shader layout would require some changes.
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0); // TODO: Fix
            
            int texCoordLocation = 1;
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(0);

            // GL.BufferData(..) should be called in update() in a game, to add data to be buffered
            // ... Seems OK to not update buffer data in a game, but I might want to look into that
            Bootstrap.getRunningGame().update(); // Q: Is this circular? 

            

            timeInMillisecondsEnd = Bootstrap.getCurrentMillis();
            Bootstrap.setDeltaTime((timeInMillisecondsEnd - timeInMillisecondsStart) / 1000.0f); // Dunno if this is right. Not sure what deltaTime should be

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // Clear screen before re-rendering
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Magic OpenGL rendering stuff
            texture.Use();
            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3); // TODO: Remove
            GL.DrawElements(PrimitiveType.Triangles, indicesLength, DrawElementsType.UnsignedInt, 0); // For DrawShape rather than drawtriangle

            // Display what has been rendering. Must be last. Double-buffering avoids screen tearing.
            SwapBuffers(); 
        }

        // Runs (only once) when GameWindow loads the window
        protected override void OnLoad() 
        {
            base.OnLoad();

            // Sets the color of the window "between frames"
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);      // Redundant?

            shader = new Shader("../../../../Shard/shader.vert", "../../../../Shard/shader.frag");
            texture = new Texture(Bootstrap.getAssetManager().getAssetPath("spaceship.png"));
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // Redundant?
            GL.DeleteBuffer(VertexBufferObject);        // Redundant?
            shader.Dispose();
        }

    }
}
