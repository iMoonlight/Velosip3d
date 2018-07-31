using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Velosip3d
{
    internal static class Controls
    {
        private static Vector2 mousePosLast = new Vector2();
        private static RenderCamera cam;
        private static GameWindow window;

        private static List<Key> keyBuffer = new List<Key>();

        private static Dictionary<List<Key>, Action> keyShortcuts = new Dictionary<List<Key>, Action>();

        public static void Init()
        {
            Tools.LogM();

            cam = Render.MainCamera;

            while (Render.RenderWindow == null){ //Quick workaround
                Thread.Sleep(10);
            }

            window = Render.RenderWindow;

            window.KeyDown += (es, e) => OnKeyboard(true , e);
            window.KeyUp += (es, e) => OnKeyboard(false, e);
            window.MouseMove += (es, e) => OnMouseMove();

            keyShortcuts.Add(new List<Key> { Key.W }, () => cam.Move(0f, 0.1f, 0f)); //LOL
            //keyShortcuts.Add(new List<Key> { }, () => window.Exit()); //LOL
        }

        private static void OnKeyboard(bool state, KeyboardKeyEventArgs e)
        {
            if (!window.Focused) return; //I am making game engine, not keylogger ;)

            if (state) //Spagethi?
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

            foreach (Key _key in keyBuffer)
            {
                Console.Write(_key.ToString() + " ");
            }
            Console.WriteLine();

            //foreach (Key key in keyBuffer)
            //{
            //    if (key == Key.Escape)
            //    {
            //        window.Exit();
            //    }

            //    switch (key)
            //    {
            //        //case Key.W:
            //        //    cam.Move(0f, 0.1f, 0f);
            //        //    break;
            //        case Key.A:
            //            cam.Move(-0.1f, 0f, 0f);
            //            break;
            //        case Key.S:
            //            cam.Move(0f, -0.1f, 0f);
            //            break;
            //        case Key.D:
            //            cam.Move(0.1f, 0f, 0f);
            //            break;
            //        case Key.Q:
            //            cam.Move(0f, 0f, 0.1f);
            //            break;
            //        case Key.E:
            //            cam.Move(0f, 0f, -0.1f);
            //            break;
            //    }
            //}
        }

        private static void KeyPresseed(Key key)
        {
            Tools.LogC("Keypressed", key.ToString());
        }

        private static void KeyReleased(Key key)
        {
            Tools.LogC("Keyreleased", key.ToString());
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
