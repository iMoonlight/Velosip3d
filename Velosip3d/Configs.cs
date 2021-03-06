﻿using System;
using System.Collections.Generic;
using OpenTK.Input;

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

            public static Dictionary<string, Key> Keys = new Dictionary<string, Key>()
            {
                { "control", Key.LControl },
                { "cam.forward", Key.W },
                { "cam.back", Key.S },
                { "cam.left", Key.A },
                { "cam.right", Key.D }
            };
        }

        public struct Engine
        {
            public static readonly int tickRate = 500; // Do not increase over 500
        }

        public static void Init()
        {
            Tools.LogM();
        }
    }
}
