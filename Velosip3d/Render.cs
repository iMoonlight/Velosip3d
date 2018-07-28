using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Velosip3d
{
    internal static class Render
    {
        private static int frames = 0;
        private static int glProgramId;
        private static int shaderBaseVertex;
        private static int shaderBaseFragment;
        private static int shaderInVPos;
        private static int shaderInVCol;
        private static int shaderModelview;
        private static int vboPosition;
        private static int vboColor;
        private static int vboModelview;
        private static int iboElements;
        private static Vector3[] dataVertex;
        private static Vector3[] dataColor;
        private static int[] dataVertexIndec;

        internal static RenderCamera MainCamera { get; } = new RenderCamera();
        public static GameWindow RenderWindow { get; } = new GameWindow(Configs.Render.windowSize[0], Configs.Render.windowSize[1]);

        public static void Init()
        {
            InitEvents();

            Console.WriteLine("Init.");

            RenderWindow.Run();
        }

        private static void InitEvents()
        {
            Console.WriteLine("Events.");

            RenderWindow.Load += (es, e) => OnLoad();
            RenderWindow.Resize += (es, e) => OnResize();
            RenderWindow.UpdateFrame += (es, e) => OnFrameUpdate(e);
            RenderWindow.RenderFrame += (es, e) => OnFrameRender();
            RenderWindow.FocusedChanged += (es, e) => OnFocusedChanged(e);

            RenderWindow.KeyDown += (es, e) => OnKeyboard(e);
        }

        private static void SetUp()
        {
            Console.WriteLine("SetUp.");

            MainCamera.Position = new Vector3(2.0f, 2.5f, 6.5f);

            glProgramId = GL.CreateProgram();

            shaderBaseVertex = ShaderLoadFromFile("baseVertex", ShaderType.VertexShader, glProgramId);
            shaderBaseFragment = ShaderLoadFromFile("baseFragment", ShaderType.FragmentShader, glProgramId);

            GL.LinkProgram(glProgramId);

            shaderInVPos = GL.GetAttribLocation(glProgramId, "vPosition");
            shaderInVCol = GL.GetAttribLocation(glProgramId, "vColor");
            shaderModelview = GL.GetUniformLocation(glProgramId, "modelview");

            GL.GenBuffers(1, out vboPosition);
            GL.GenBuffers(1, out vboColor);
            GL.GenBuffers(1, out vboModelview);
            GL.GenBuffers(1, out iboElements);
        }

        private static void UpdateViewport()
        {
            GL.Viewport(0, 0, RenderWindow.ClientRectangle.Width, RenderWindow.ClientRectangle.Height);
        }

        private static void OnLoad()
        {
            Console.WriteLine("OnLoad.");

            RenderWindow.VSync = VSyncMode.Off;
            if (Configs.Render.VSync) RenderWindow.VSync = VSyncMode.On;

            SetUp();

            RenderWindow.Title = Configs.Render.windowTitle;
            GL.ClearColor(Color.DarkBlue);
            GL.PointSize(5f);
        }

        private static void OnResize()
        {
            Console.WriteLine("OnResize.");

            UpdateViewport();
        }

        private static void OnFrameUpdate(FrameEventArgs e)
        {
            if (Configs.Render.showFPS)
            {
                var _title = Configs.Render.windowTitle + ", FPS: " + Math.Round(RenderWindow.RenderFrequency).ToString();
                _title += ", Objects: " + renderObjectsList.Count.ToString();
                _title += ", Frames: " + frames.ToString();

                RenderWindow.Title = _title;
            }

            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();

            int vertcount = 0;
            foreach (RenderObjects.Object v in renderObjectsList)
            {
                verts.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                vertcount += v.countVertex;
            }

            dataVertex = verts.ToArray();
            dataVertexIndec = inds.ToArray();
            dataColor = colors.ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboPosition);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(dataVertex.Length * Vector3.SizeInBytes), dataVertex, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaderInVPos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboColor);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(dataColor.Length * Vector3.SizeInBytes), dataColor, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaderInVCol, 3, VertexAttribPointerType.Float, true, 0, 0);
            
            foreach (RenderObjects.Object v in renderObjectsList)
            {
                v.CalculateModelMatrix();
                v.matrixProjectionView = MainCamera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, (float)(RenderWindow.ClientSize.Width / RenderWindow.ClientSize.Height), 1.0f, 40.0f);
                v.matrixProjectionModelview = v.matrixModel * v.matrixProjectionView;
            }

            GL.UseProgram(glProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboElements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(dataVertexIndec.Length * sizeof(int)), dataVertexIndec, BufferUsageHint.StaticDraw);

            if (RenderWindow.Focused)
            {
                Vector2 delta = mousePosLast - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);

                MainCamera.AddRotation(delta.X, delta.Y);
                controlCursorReset();
            }
        }

        private static void OnFrameRender()
        {
            UpdateViewport();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.EnableVertexAttribArray(shaderInVPos);
            GL.EnableVertexAttribArray(shaderInVCol);

            int indiceat = 0;

            foreach (RenderObjects.Object v in renderObjectsList)
            {
                GL.UniformMatrix4(shaderModelview, false, ref v.matrixProjectionModelview);
                GL.DrawElements(BeginMode.Triangles, v.countIndec, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.countIndec;
            }

            GL.DisableVertexAttribArray(shaderInVPos);
            GL.DisableVertexAttribArray(shaderInVPos);

            GL.Flush();
            RenderWindow.SwapBuffers();

            frames++;
        }
    }
}
