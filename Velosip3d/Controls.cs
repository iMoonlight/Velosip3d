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

        private static void OnKeyboard(KeyboardKeyEventArgs e)
        {
            Console.WriteLine("OnKeyboard: " + e.Key.ToString());

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
            Mouse.SetPosition(renderWindow.Bounds.Left + renderWindow.Bounds.Width / 2, renderWindow.Bounds.Top + renderWindow.Bounds.Height / 2);
            mousePosLast = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
    }
}
