using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Velosip3d
{
    internal class Tools
    {
        public static void LogN(string action, string text)
        {
            string timeCode = DateTime.Now.ToString("HH:mm:ss");

            Console.WriteLine("[" + timeCode + "] " + action + ": " + text);
        }

        public static void Log(string action, string text)
        {
            string timeCode = DateTime.Now.ToString("HH:mm:ss");

            Console.Write("[" + timeCode + "]" + action + ": " + text);
        }
    }
}
