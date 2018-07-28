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

        public static void Init()
        {

        }

        public static void OnKeyboard(KeyboardKeyEventArgs e)
        {
            Tools.LogN("Keyboard", e.Key.ToString());

            if (e.Key == Key.Escape)
            {
                Render.RenderWindow.Exit();
            }

            switch (e.Key)
            {
                case Key.W:
                    Render.MainCamera.Move(0f, 0.1f, 0f);
                    break;
                case Key.A:
                    Render.MainCamera.Move(-0.1f, 0f, 0f);
                    break;
                case Key.S:
                    Render.MainCamera.Move(0f, -0.1f, 0f);
                    break;
                case Key.D:
                    Render.MainCamera.Move(0.1f, 0f, 0f);
                    break;
                case Key.Q:
                    Render.MainCamera.Move(0f, 0f, 0.1f);
                    break;
                case Key.E:
                    Render.MainCamera.Move(0f, 0f, -0.1f);
                    break;
            }
        }

        private static void controlCursorReset()
        {
            Mouse.SetPosition(Render.RenderWindow.Bounds.Left + Render.RenderWindow.Bounds.Width / 2, Render.RenderWindow.Bounds.Top + Render.RenderWindow.Bounds.Height / 2);
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
