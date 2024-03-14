/*
*
*   SDL provides an input layer, and we're using that.  This class tracks input, anchors it to the 
*       timing of the game loop, and converts the SDL events into one that is more abstract so games 
*       can be written more interchangeably.
*   @author Michael Heron
*   @version 1.0
*   
*/



using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;

namespace Shard
{

    // We'll be using OpenTK here to provide our underlying input system.
    class InputFramework : InputSystem
    {
        GameWindow window;

        double tick, timeInterval;

        List<MouseMoveEventArgs> mouseMoveEvents = new List<MouseMoveEventArgs>();
        List<MouseButtonEventArgs> mouseButtonEvents = new List<MouseButtonEventArgs>();
        List<MouseWheelEventArgs> wheelEvents = new List<MouseWheelEventArgs>();
        List<KeyboardKeyEventArgs> keyDownEvents = new List<KeyboardKeyEventArgs>();
        List<KeyboardKeyEventArgs> keyUpEvents = new List<KeyboardKeyEventArgs>();

        public override void getInput()
        {

            InputEvent ie;

            tick += Bootstrap.getDeltaTime();

            if (tick < timeInterval)
            {
                return;
            }

            while (tick >= timeInterval)
            {

                ie = new InputEvent();

                foreach(var mot in mouseMoveEvents)
                {
                    ie.X = (int)mot.X;
                    ie.Y = (int)mot.Y;

                    informListeners(ie, "MouseMotion");
                }
                mouseMoveEvents.Clear();

                foreach (var butt in mouseButtonEvents)
                {
                    ie.Button = (int)butt.Button;
                    if (butt.IsPressed)
                        informListeners(ie, "MouseDown");
                    else
                        informListeners(ie, "MouseUp");
                }
                mouseButtonEvents.Clear();

                foreach (var wh in wheelEvents)
                {
                    ie.X = (int)wh.OffsetX;
                    ie.Y = (int)wh.OffsetY;

                    informListeners(ie, "MouseWheel");
                }
                wheelEvents.Clear();


                foreach (var ev in keyDownEvents)
                {
                    if (!ev.IsRepeat)
                    {
                        ie.Key = (int)ev.Key;
                        informListeners(ie, "KeyDown");
                    }
                }
                keyDownEvents.Clear();

                foreach (var ev in keyUpEvents)
                {
                    ie.Key = (int)ev.Key;
                    informListeners(ie, "KeyUp");
                }
                keyUpEvents.Clear();


                tick -= timeInterval;
            }


        }

        public override void initialize()
        {
            tick = 0;
            timeInterval = 1.0 / 120000.0;
        }

        public void setWindow(GameWindow window)
        {
            this.window = window;
            window.KeyDown += window_KeyDown;
            window.KeyUp += window_KeyUp;
            window.MouseMove += window_MouseMove;
            window.MouseDown += window_MouseDown;
            window.MouseUp += window_MouseUp;
            window.MouseWheel += window_MouseWheel;
        }

        private void window_MouseWheel(MouseWheelEventArgs obj)
        {
            wheelEvents.Add(obj);
        }

        private void window_MouseUp(MouseButtonEventArgs obj)
        {
            mouseButtonEvents.Add(obj);
        }

        private void window_MouseDown(MouseButtonEventArgs obj)
        {
            mouseButtonEvents.Add(obj);
        }

        private void window_MouseMove(MouseMoveEventArgs obj)
        {
            mouseMoveEvents.Clear();
            mouseMoveEvents.Add(obj);
        }

        private void window_KeyUp(KeyboardKeyEventArgs obj)
        {
            keyUpEvents.Add(obj);
        }

        private void window_KeyDown(KeyboardKeyEventArgs obj)
        {
            keyDownEvents.Add(obj);
        }
    }
}