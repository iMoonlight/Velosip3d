using System;

namespace Velosip3d
{
    internal class Velosip3d
    {
        private static void Main(string[] args)
        {
            Tools.LogM();

            //Alot of cheks mb
            Configs.Init();
            Render.Init();
            Controls.Init();

            Console.ReadKey();
        }
    }
}
