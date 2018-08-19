using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Velosip3d
{
    internal static class Controls
    {
        private static Vector2 mousePosLast = new Vector2();
        private static MainCamera cam;
        private static GameWindow window;

        private static List<Key> keyBuffer = new List<Key>();

        private static Dictionary<Key, Action> keyActions = new Dictionary<Key, Action>();

        public static void Init()
        {
            Tools.LogM();

            cam = Render.MainCamera;

            while (Render.RenderWindow == null){ //Quick workaround
                Tools.Lag(10);
            }

            window = Render.RenderWindow;

            window.KeyDown += (es, e) => OnKeyboard(true , e);
            window.KeyUp += (es, e) => OnKeyboard(false, e);
            window.MouseMove += (es, e) => OnMouseMove();

            keyActions.Add(Key.Escape, () => window.Exit()); //Hardcoded ESC = END ALL SH*T
            keyActions.Add(Configs.Controls.Keys["cam.forward"], () => cam.Move(0f, 0.1f, 0f));
            keyActions.Add(Configs.Controls.Keys["cam.back"], () => cam.Move(0f, -0.1f, 0f));
            keyActions.Add(Configs.Controls.Keys["cam.left"], () => cam.Move(-0.1f, 0f, 0f));
            keyActions.Add(Configs.Controls.Keys["cam.right"], () => cam.Move(0.1f, 0f, 0f));

            KeyBufferProcessor();
        }

        private static void OnKeyboard(bool state, KeyboardKeyEventArgs e)
        {
            if (!window.Focused) return; //I am making game engine, not keylogger ;)

            if (state)
            {
                if (!keyBuffer.Contains(e.Key))
                {
                    keyBuffer.Add(e.Key);
                    KeyPresseed(e.Key);
                }
            }
            else
            {
                keyBuffer.RemoveAll(key => key == e.Key);
                KeyReleased(e.Key);
            }
        }

        private static void KeyPresseed(Key key)
        {
            Tools.LogC("Keypressed", key.ToString());
        }

        private static void KeyReleased(Key key)
        {
            Tools.LogC("Keyreleased", key.ToString());
        }

        private static void KeyBufferProcessor() //RUN ONLY IN ANOTHER THREAD, ITS IMPORTANT FOR GAMES
        {
            new Thread(() => //Threaded key process
            {
                Thread.CurrentThread.IsBackground = true;

                while (true)
                {
                    if (keyBuffer.Count > 0)
                    {
                        foreach (Key key in keyBuffer.ToList())
                        {
                            if (keyActions.ContainsKey(key)) keyActions[key]();
                        }
                    }

                    Tools.Lag(1);
                }
            }).Start();
        }

        private static void OnMouseMove()
        {
            if (!window.Focused) return; //I am making game engine, not keylogger ;)

            if (keyBuffer.Contains(Configs.Controls.Keys["control"]))
            {
                Vector2 delta = mousePosLast - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                Render.MainCamera.AddRotation(delta.X, delta.Y);
                controlCursorReset();
            }
        }

        private static void controlCursorReset()
        {
            Mouse.SetPosition(window.Bounds.Left + window.Bounds.Width / 2, window.Bounds.Top + window.Bounds.Height / 2);
            mousePosLast = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
    }
}
