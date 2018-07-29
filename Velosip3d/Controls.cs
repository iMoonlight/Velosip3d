using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Velosip3d
{
    internal static class Controls
    {
        private static Vector2 mousePosLast = new Vector2();
        private static RenderCamera cam;
        private static GameWindow window;

        public static void Init()
        {
            cam = Render.MainCamera;
            window = Render.RenderWindow;

            //window.KeyDown += (es, e) => OnKeyboard(e);
        }

        private static void OnKeyboard(KeyboardKeyEventArgs e)
        {
            Tools.LogC("Keyboard", e.Key.ToString());

            if (e.Key == Key.Escape)
            {
                Render.RenderWindow.Exit();
            }

            switch (e.Key)
            {
                case Key.W:
                    cam.Move(0f, 0.1f, 0f);
                    break;
                case Key.A:
                    cam.Move(-0.1f, 0f, 0f);
                    break;
                case Key.S:
                    cam.Move(0f, -0.1f, 0f);
                    break;
                case Key.D:
                    cam.Move(0.1f, 0f, 0f);
                    break;
                case Key.Q:
                    cam.Move(0f, 0f, 0.1f);
                    break;
                case Key.E:
                    cam.Move(0f, 0f, -0.1f);
                    break;
            }
        }

        private static void controlCursorReset()
        {
            Mouse.SetPosition(window.Bounds.Left + window.Bounds.Width / 2, window.Bounds.Top + window.Bounds.Height / 2);
            mousePosLast = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        private static void TEMP()
        {
            //if (RenderWindow.Focused)
            //{
            //    Vector2 delta = mousePosLast - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);

            //    Render.MainCamera.AddRotation(delta.X, delta.Y);
            //    controlCursorReset();
            //}
        }
    }
}
