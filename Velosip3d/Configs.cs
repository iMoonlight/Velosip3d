using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Velosip3d
{
    internal static class Configs
    {
        public struct Dirs
        {
            public static readonly string dirGame = AppDomain.CurrentDomain.BaseDirectory;
            public static readonly string dirContent = dirGame + "content\\";
            public static readonly string dirVisuals = dirContent + "visuals\\";
            public static readonly string dirShaders = dirVisuals + "shaders\\";
            public static readonly string dirSounds = dirContent + "sounds\\";
        }

        public struct Render
        {
            public static readonly int[] windowSize = { 1280, 720 };
            public static readonly string windowTitle = "Render output";
            public static readonly bool VSync = true;

            public static readonly float FOV = 90.0f;

            public static readonly bool showDebugInfo = true;
        }

        public struct Controls
        {
            public static readonly float mouseSens = 0.01f;
        }

        public static void Init()
        {
            
        }
    }
}
