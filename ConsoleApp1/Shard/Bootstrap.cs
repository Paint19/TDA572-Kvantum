/*
*
*   The Bootstrap - this loads the config file, processes it and then starts the game loop
*   @author Michael Heron
*   @version 1.0
*   
*/

/*
When using OpenTK, the GameWindow has a function Run() which starts the game loop.
And there are functions for regulating the update frequency, and functions which are called when updating/rendering.
 */

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Shard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Shard
{
    class Bootstrap : GameWindow
    {

        public Bootstrap(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws){ }

        public static string DEFAULT_CONFIG = "config.cfg";

        private static Game runningGame;
        private static Display displayEngine;
        private static Sound soundEngine;
        private static InputSystem input;
        private static PhysicsManager phys;
        private static AssetManagerBase asset;

        private static int targetFrameRate;
        private static int millisPerFrame;
        private static double deltaTime;
        private static double timeElapsed;
        private static int frames;
        private static List<long> frameTimes;
        private static long startTime;
        private static string baseDir;
        private static Dictionary<string,string> enVars;

        // I haven't considered these yet, but a lot of this should be redundant since we no longer have to create the timing/fps functionality
        private static long timeInMillisecondsStart, lastTick;
        private static long interval;
        private static int sleep;
        private static int tfro = 1;
        private static bool physUpdate = false;
        private static bool physDebug = false;
        private static long timeInMillisecondsEnd;

        public static bool checkEnvironmentalVariable (string id) {
            return enVars.ContainsKey (id);
        }

        
        public static string getEnvironmentalVariable (string id) {
            if (checkEnvironmentalVariable (id)) {
                return enVars[id];
            }

            return null;
        }


        public static double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }

        public static string getBaseDir() {
            return baseDir;
        }

        public static void setup()
        {
            string workDir = Environment.CurrentDirectory;
            baseDir = Directory.GetParent(workDir).Parent.Parent.Parent.FullName;;

            setupEnvironmentalVariables(baseDir + "\\" + "envar.cfg");
            setup(baseDir + "\\" + DEFAULT_CONFIG);

        }

        public static void setupEnvironmentalVariables (String path) {
                Console.WriteLine("Path is " + path);

                Dictionary<string, string> config = BaseFunctionality.getInstance().readConfigFile(path);

                enVars = new Dictionary<string,string>();

                foreach (KeyValuePair<string, string> kvp in config)
                {
                    enVars[kvp.Key] = kvp.Value;
                }
        }
        public static double getDeltaTime()
        {

            return deltaTime;
        }

        public static Display getDisplay()
        {
            return displayEngine;
        }

        public static Sound getSound()
        {
            return soundEngine;
        }

        public static InputSystem getInput()
        {
            return input;
        }

        public static AssetManagerBase getAssetManager() {
            return asset;
        }

        public static Game getRunningGame()
        {
            return runningGame;
        }

        public static void setup(string path)
        {
            Console.WriteLine ("Path is " + path);

            Dictionary<string, string> config = BaseFunctionality.getInstance().readConfigFile(path);
            Type t;
            object ob;
            bool bailOut = false;

            phys = PhysicsManager.getInstance();

            foreach (KeyValuePair<string, string> kvp in config)
            {
                t = Type.GetType("Shard." + kvp.Value);

                if (t == null)
                {
                    Debug.getInstance().log("Missing Class Definition: " + kvp.Value + " in " + kvp.Key, Debug.DEBUG_LEVEL_ERROR);
                    Environment.Exit(0);
                }

                ob = Activator.CreateInstance(t);


                switch (kvp.Key)
                {
                    case "display":
                        displayEngine = (Display)ob;
                        displayEngine.initialize();
                        break;
                    case "sound":
                        soundEngine = (Sound)ob;
                        break;
                    case "asset":
                        asset = (AssetManagerBase)ob;
                        asset.registerAssets();
                        break;
                    case "game":
                        runningGame = (Game)ob;
                        targetFrameRate = runningGame.getTargetFrameRate();
                        millisPerFrame = 1000 / targetFrameRate;
                        break;
                    case "input":
                        input = (InputSystem)ob;
                        input.initialize();
                        break;

                }

                Debug.getInstance().log("Config file... setting " + kvp.Key + " to " + kvp.Value);
            }

            if (runningGame == null)
            {
                Debug.getInstance().log("No game set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (displayEngine == null)
            {
                Debug.getInstance().log("No display engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (soundEngine == null)
            {
                Debug.getInstance().log("No sound engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (bailOut)
            {
                Environment.Exit(0);
            }
        }

        public static long getCurrentMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static int getFPS()
        {
            int fps;
            double seconds;

            seconds = (getCurrentMillis() - startTime) / 1000.0;

            fps = (int)(frames / seconds);

            return fps;
        }

        public static int getSecondFPS()
        {
            int count = 0;
            long now = getCurrentMillis();
            int lastEntry;



            Debug.Log ("Frametimes is " + frameTimes.Count);

            if (frameTimes.Count == 0) {
                return -1;
            }

            lastEntry = frameTimes.Count - 1;

            while (frameTimes[lastEntry] > (now - 1000) && lastEntry > 0) {
                lastEntry -= 1;
                count += 1;
            }

            if (lastEntry > 0) {
                frameTimes.RemoveRange (0, lastEntry);
            }

            return count;
        }

        public static int getCurrentFrame()
        {
            return frames;
        }

        protected override void OnUpdateFrame(FrameEventArgs args) // Runs when GameWindow updates frame
        {
            base.OnUpdateFrame(args);


            if (KeyboardState.IsKeyDown(Keys.Escape)) // Esc - Closes window
            {
                Close();
            }

            /* ------- Everything below is from the original code, used to be in Main method. Much should be redundant ------- */

            long timeInMillisecondsStart = getCurrentMillis();

            frames += 1; 

            // Clear the screen.
            // Bootstrap.getDisplay().clearDisplay(); // TODO: Fix Display class

            // Update 
            // runningGame.update(); // TODO: Fix game class
            
            // Input // TODO: Fix game class

            if (runningGame.isRunning() == true)
            {

                // Get input, which works at 50 FPS to make sure it doesn't interfere with the 
                // variable frame rates.
                input.getInput();

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
                physUpdate = phys.update();

                if (physUpdate)
                {
                    // If it did tick, give every object an update
                    // that is pinned to the timing of the physics system.
                    GameObjectManager.getInstance().physicsUpdate();
                }

                if (physDebug)
                {
                    phys.drawDebugColliders();
                }

            }

            // Render the screen.
            // Bootstrap.getDisplay().display(); // TODO: Fix display class

            timeInMillisecondsEnd = getCurrentMillis();

            frameTimes.Add(timeInMillisecondsEnd);

            interval = timeInMillisecondsEnd - timeInMillisecondsStart;

            sleep = (int)(millisPerFrame - interval);


            TimeElapsed += deltaTime;

            if (sleep >= 0)
            {
                // Frame rate regulator.  Bear in mind since this is millisecond precision, and we 
                // only get whole numbers from our interval, it will only rarely match a target 
                // FPS.  Milliseconds just aren't precise enough.
                //
                //  (I'm hinting if this bothers you, you might have found an engine modification to make...)
                Thread.Sleep(sleep);
            }

            timeInMillisecondsEnd = getCurrentMillis();
            deltaTime = (timeInMillisecondsEnd - timeInMillisecondsStart) / 1000.0f;

            millisPerFrame = 1000 / targetFrameRate;

            lastTick = timeInMillisecondsStart;

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit); // Clear screen before re-rendering

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers(); // Display what has been rendering. Must be last. Double-buffering avoids screen tearing.
        }

        int VertexBufferObject;
        int VertexArrayObject;
        float[] vertices = {    // TODO: Remove. This is a temporary test object to be displayed.
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f,  //Bottom-right vertex
            0.0f,  0.5f, 0.0f   //Top vertex
        };
        Shader shader;
        protected override void OnLoad() // Runs when GameWindow loads the window (only runs once)
        {
            base.OnLoad();


            shader = new Shader("../../../Shard/shader.vert", "../../../Shard/shader.frag");

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw); // Move this

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f); // Sets the color of the window "between frames"

            VertexArrayObject = GL.GenVertexArray();

            // ..:: Initialization code (done once (unless your object frequently changes)) :: ..
            // 1. bind Vertex Array Object
            GL.BindVertexArray(VertexArrayObject);
            // 2. copy our vertices array in a buffer for OpenGL to use
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            // 3. then set our vertex attributes pointers
            // Takes data from the latest bound VBO (memory buffer) bound to ArrayBuffer.
            // The first parameter is the location of the vertex attribute. Dynamically retrieving shader layout would require some changes.
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Setup the engine.
            setup();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // Redundant?
            GL.DeleteBuffer(VertexBufferObject);        // Redundant?
            shader.Dispose();
        }

        protected override void OnResize(ResizeEventArgs e) // TODO: ? Remove or implement further? Doesn't seem to make a difference as it is now.
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        static void Main(string[] args)
        {
            frames = 0;
            frameTimes = new List<long>();
            // time for when we start the program running.
            startTime = getCurrentMillis();

            // -------  New stuff for OpenTK, to start the game loop etc:

            GameWindowSettings gws = GameWindowSettings.Default;
            NativeWindowSettings nws = NativeWindowSettings.Default;

            gws.UpdateFrequency = 60;

            nws.Size = new Vector2i(1280, 720);
            nws.Title = "Hello wOrld!";

            Bootstrap window = new Bootstrap(gws, nws);

            window.Run();


            // --- old stuff:
            
            // Start the game running.
            runningGame.initialize();

            // timeInMillisecondsStart = startTime; Should be OK to remove
            lastTick = startTime;

            phys.GravityModifier = 0.1f;
            // This is our game loop.

            if (getEnvironmentalVariable("physics_debug") == "1")
            {
                physDebug = true;
            }

        }
    }
}
