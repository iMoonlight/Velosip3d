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
        public class Shaders
        {
            public static int ShaderLoadFromString(string source, ShaderType type, int programId)
            {
                int address = GL.CreateShader(type);

                GL.ShaderSource(address, source);
                GL.CompileShader(address);
                GL.AttachShader(programId, address);

                return address;
            }

            public static int ShaderLoadFromFile(string fileName, ShaderType type, int programId)
            {
                string _fullName = fileName + ".glsl";
                string _fullPath = Configs.Dirs.dirShaders + _fullName;

                using (StreamReader sr = new StreamReader(_fullPath))
                {
                    Tools.LogC("Shader loaded", _fullName);
                    return ShaderLoadFromString(sr.ReadToEnd(), type, programId);
                }
            }
        }
    }
}
