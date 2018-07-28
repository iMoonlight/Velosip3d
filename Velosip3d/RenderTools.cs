using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Velosip3d
{
    internal class RenderTools
    {
        private static int ShaderLoadFromString(string source, ShaderType type, int programId)
        {
            int address = GL.CreateShader(type);

            GL.ShaderSource(address, source);
            GL.CompileShader(address);
            GL.AttachShader(programId, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));

            return address;
        }

        private static int ShaderLoadFromFile(string fileName, ShaderType type, int programId)
        {
            var _fullPath = Configs.Dirs.dirShaders + fileName + ".glsl";

            Console.Write(_fullPath + " Log: ");

            using (StreamReader sr = new StreamReader(_fullPath))
            {
                return ShaderLoadFromString(sr.ReadToEnd(), type, programId);
            }
        }
    }
}
