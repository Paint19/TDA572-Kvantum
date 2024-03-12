/*
*
*   The Bootstrap - this loads the config file, processes it and then starts the game loop
*   @author Michael Heron
*   @version 1.0
*   
*/

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Shard.Shard.Animation;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shard
{
    class Bootstrap
    {

        public static string DEFAULT_CONFIG = "config.cfg";

        private static WindowOTK window;

        private static Game runningGame;
        private static Display displayEngine;
        private static Sound soundEngine;
        private static InputSystem input;
        private static bool physDebug = false;
        private static PhysicsManager phys;
        private static AssetManagerBase asset;
        private static AnimationSystem animation;

        private static int targetFrameRate;
        private static double deltaTime;
        private static string baseDir;
        private static Dictionary<string,string> enVars;

        private static long timeStarted;

        public static bool checkEnvironmentalVariable (string id) {
            return enVars.ContainsKey (id);
        }

        
        public static string getEnvironmentalVariable (string id) {
            if (checkEnvironmentalVariable (id)) {
                return enVars[id];
            }

            return null;
        }

        public static string getBaseDir() {
            return baseDir;
        }

        public static void setup()
        {
            string workDir = Environment.CurrentDirectory;
            baseDir = Directory.GetParent(workDir).Parent.Parent.Parent.FullName;

            setupEnvironmentalVariables(baseDir + "\\" + "envar.cfg");
            setup(baseDir + "\\" + DEFAULT_CONFIG);
            timeStarted = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

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

        public static void setDeltaTime(double time)
        {
            deltaTime = time;
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

        public static AnimationSystem getAnimationSystem()
        {
            return animation;
        }

        public static Game getRunningGame()
        {
            return runningGame;
        }

        public static PhysicsManager getPhysicsManager() { return phys; }

        public static bool getPhysDebug() { return physDebug; }

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
                        break;
                    case "input":
                        input = (InputSystem)ob;
                        input.initialize();
                        break;
                    case "animation":
                        animation = (AnimationSystem)ob;
                        animation.initialize();
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

            if (animation == null)
            {
                Debug.getInstance().log("No animation system set", Debug.DEBUG_LEVEL_ERROR);
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

        public static long getTimeSinceSetup()
        {
            return (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - timeStarted;
        }
        public static WindowOTK getWindow(){ return window; }   // sus

        static void Main(string[] args)
        {
            // Setup the engine.
            setup();
            
            

            phys.GravityModifier = 0.1f;

            if (getEnvironmentalVariable("physics_debug") == "1")
            {
                physDebug = true;
            }

            // OpenTK, to start the game loop etc:

            GameWindowSettings gws = GameWindowSettings.Default;
            NativeWindowSettings nws = NativeWindowSettings.Default;

            gws.UpdateFrequency = 60;

            nws.Size = new Vector2i( displayEngine.getWidth(), displayEngine.getHeight());
            nws.Title = "Hello wOrld!";
            nws.Vsync = VSyncMode.On;

            window = new WindowOTK(gws, nws); // Starts the game loop
            window.Run();
        }
    }
}
