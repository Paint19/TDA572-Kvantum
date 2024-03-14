using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Shard.Shard;

/*
 *  Here we have a lot of OpenTK magic. 
 *  GameWindow has a bunch of functions that are called when updating/rendering/loading/unloading the window (and more)
 */

namespace Shard
{
    public class WindowOTK : GameWindow
    {

        private int VertexBufferObject;
        private int ElementBufferObject;
        private int VertexArrayObject;

        private int indicesLength;

        private Shader shader;
        private Shader shaderLightSource;

        private Camera activeCamera;

        private float eventArgsTime;

        // TODO: fundera på om detta är rimligt
        // Currently only supports one light source
        private Vector3 LIGHT_COLOR = new Vector3(1.0f, 1.0f, 1.0f);
        private Transform lightSource = new Transform(); 

        public WindowOTK(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        public void setIndicesLength(int length) { indicesLength = length; }
        public void setActiveCamera(Camera cam) { activeCamera = cam; }
        public void setShaderMVP(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);
        }

        public void setLightShaderMVP(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            shaderLightSource.SetMatrix4("model", model);
            shaderLightSource.SetMatrix4("view", view);
            shaderLightSource.SetMatrix4("projection", projection);
        }

        public void addLight(Transform trans) { lightSource = trans; }
        public void removeLight() { lightSource = null; }

        public float getEventArgsTime() { return eventArgsTime; }
        protected override void OnUpdateFrame(FrameEventArgs args) // Runs when GameWindow updates frame
        {

            base.OnUpdateFrame(args);

            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            eventArgsTime = (float)args.Time;


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

            Bootstrap.getAnimationSystem().update();
            Bootstrap.getRunningGame().update();

            timeInMillisecondsEnd = Bootstrap.getCurrentMillis();
            Bootstrap.setDeltaTime((timeInMillisecondsEnd - timeInMillisecondsStart) / 1000.0f); // Dunno if this is right. Not sure what deltaTime should be

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // Clear screen before re-rendering
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // ---- GAME OBJECTS
            
            shader.Use();

            // Renders all game objects
            Bootstrap.getDisplay().display();

            // transformation matrices
            if (activeCamera != null)
            {
                // Shader.vert
                Matrix4 model = Matrix4.Identity;
                Matrix4 view = activeCamera.GetViewMatrix();
                Matrix4 projection = activeCamera.GetProjectionMatrix();
                setShaderMVP(model, view, projection);

                // Shader.frag
                shader.SetVector3("viewPos", activeCamera.position);
            }

            if(lightSource is not null)
            {
                // Shader.frag
                shader.SetVector3("lightColor", LIGHT_COLOR);
                shader.SetVector3("lightPos", lightSource.Translation); 
            }

            //  ---- LIGHT

            shaderLightSource.Use();

            // Render lights:
            Bootstrap.getDisplay().displayLightSource();

            if (activeCamera != null)
            {
                // Shader.vert
                Matrix4 model = Matrix4.Identity;
                Matrix4 view = activeCamera.GetViewMatrix();
                Matrix4 projection = activeCamera.GetProjectionMatrix();
                setLightShaderMVP(model, view, projection);
            }

            lightSource = null; // reset lightSource

            // Display what has been rendering. Must be last. Double-buffering avoids screen tearing.
            SwapBuffers();

        }

        // Runs (only once) when GameWindow loads the window
        protected override void OnLoad()
        {
            base.OnLoad();

            InputFramework inp = (InputFramework)Bootstrap.getInput();
            inp.setWindow(this);

            GL.Enable(EnableCap.DepthTest);

            // Sets the color of the window "between frames"
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);      // Redundant?

            // Start the game running. Must be done after starting the game loop
            Bootstrap.getRunningGame().initialize();

            // Initialize display
            Bootstrap.getDisplay().initialize();

            shader = new Shader("../../../Shard/shader.vert", "../../../Shard/shader.frag");
            shaderLightSource = new Shader("../../../Shard/shader.vert", "../../../Shard/shaderLightSource.frag");


            CursorState = CursorState.Grabbed;

        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Bootstrap.getDisplay().dispose();
            shader.Dispose();
            shaderLightSource.Dispose();
        }

    }
}
