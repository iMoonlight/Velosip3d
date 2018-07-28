using System;

namespace Velosip3d
{
    internal class Velosip3d
    {
        private static void Main(string[] args)
        {
            Begin();

            Console.ReadKey();
        }

        private static void Begin()
        {
            //Alot of cheks mb
            Configs.Init();
            Render.Init();
            Controls.Init();
        }
    }
}
