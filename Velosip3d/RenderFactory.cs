using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace Velosip3d
{
    static class RenderFactory
    {
        public static int glProgramId;

        public static int shaderBaseVertex;
        public static int shaderBaseFragment;

        public static int shaderInVPos;
        public static int shaderInVCol;
        public static int shaderModelview;

        public static int vboPosition;
        public static int vboColor;
        public static int vboModelview;
        public static int iboElements;

        public static Vector3[] dataVertex;
        public static Vector3[] dataColor;
        public static Matrix4[] dataModelview;
        public static int[] dataVertexIndex;

        public static void Init()
        {
            glProgramId = GL.CreateProgram();
            shaderBaseVertex = ShaderLoadFromFile("baseVertex", ShaderType.VertexShader, glProgramId);
            shaderBaseFragment = ShaderLoadFromFile("baseFragment", ShaderType.FragmentShader, glProgramId);

            GL.LinkProgram(glProgramId);

            shaderInVPos = GL.GetAttribLocation(glProgramId, "vPosition");
            shaderInVCol = GL.GetAttribLocation(glProgramId, "vColor");
            shaderModelview = GL.GetUniformLocation(glProgramId, "modelview");

            if (shaderInVPos == -1 || shaderInVCol == -1 || shaderModelview == -1)
            {
                Console.WriteLine("Error binding shader inputs!");
            }

            GL.GenBuffers(1, out vboPosition);
            GL.GenBuffers(1, out vboColor);
            GL.GenBuffers(1, out vboModelview);
            GL.GenBuffers(1, out iboElements);

            Console.WriteLine("Render factory init!");

            //testObjectsCreate();
            //DataLoadFromObject(RenderFactory.objectList[0]);

            dataVertex = new Vector3[] {
                new Vector3(-0.8f, -0.8f,  -0.8f),
                new Vector3(0.8f, -0.8f,  -0.8f),
                new Vector3(0.8f, 0.8f,  -0.8f),
                new Vector3(-0.8f, 0.8f,  -0.8f),
                new Vector3(-0.8f, -0.8f,  0.8f),
                new Vector3(0.8f, -0.8f,  0.8f),
                new Vector3(0.8f, 0.8f,  0.8f),
                new Vector3(-0.8f, 0.8f,  0.8f),
            };

            dataVertexIndex = new int[]{
                //front
                0, 7, 3,
                0, 4, 7,
                //back
                1, 2, 6,
                6, 5, 1,
                //left
                0, 2, 1,
                0, 3, 2,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            };

            dataColor = new Vector3[] { new Vector3(1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f,  1f, 0f),new Vector3(1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f,  1f, 0f),new Vector3(1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f)};

            dataModelview = new Matrix4[]{
                Matrix4.Identity
            };
        }

        public static int ShaderLoadFromString(string source, ShaderType type, int programId)
        {
            int address = GL.CreateShader(type);

            GL.ShaderSource(address, source);
            GL.CompileShader(address);
            GL.AttachShader(programId, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));

            return address;
        }

        public static int ShaderLoadFromFile(string fileName, ShaderType type, int programId)
        {
            var _fullPath = Configs.Dirs.dirShaders + fileName + ".glsl";

            Console.Write(_fullPath + " Error: ");

            using (StreamReader sr = new StreamReader(_fullPath))
            {
                return ShaderLoadFromString(sr.ReadToEnd(), type, programId);
            }
        }
    }
}
