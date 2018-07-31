using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Velosip3d
{
    internal class Tools
    {
        public static void LogC(string action, string text) //Custom log text
        {
            string timeCode = DateTime.Now.ToString("HH:mm:ss");

            Console.WriteLine("[" + timeCode + "] " + action + " > " + text);
        }

        public static void LogM() //Full log of method call
        {
            StackFrame frame = new StackFrame(1, true);

            string method = frame.GetMethod().Name;
            string fileName = frame.GetFileName().Split('\\').Last();
            string lineNumber = frame.GetFileLineNumber().ToString();
            string timeCode = DateTime.Now.ToString("HH:mm:ss");

            LogC(fileName + ":" + lineNumber, method);
        }

        public static void Lag(int ms)
        {
            Thread.Sleep(ms);
        }
    }
}
