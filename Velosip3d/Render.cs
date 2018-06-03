using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using OpenTK.Input;

namespace Velosip3d
{
    static class Render
    {
        static GameWindow renderWindow;

        static float time = 0;

        static int glProgramId;

        static RenderCamera cam = new RenderCamera();

        static Vector2 mousePosLast = new Vector2();

        static int shaderBaseVertex;
        static int shaderBaseFragment;

        static int shaderInVPos;
        static int shaderInVCol;
        static int shaderModelview;

        static int vboPosition;
        static int vboColor;
        static int vboModelview;
        static int iboElements;

        static Vector3[] dataVertex;
        static Vector3[] dataColor;
        static int[] dataVertexIndec;

        static List<RenderObjects.ObjectBase> renderObjectsList = new List<RenderObjects.ObjectBase>();

        public static void Init()
        {
            renderWindow = new GameWindow(Configs.Render.windowSize[0], Configs.Render.windowSize[1], new GraphicsMode(32, 24, 0, 4));

            InitEvents();

            Console.WriteLine("Render.Init()");

            renderWindow.Run();
        }

        static void InitEvents()
        {
            Console.WriteLine("Events.");

            renderWindow.Load += (es, e) => OnLoad();
            renderWindow.Resize += (es, e) => OnResize();
            renderWindow.UpdateFrame += (es, e) => OnFrameUpdate(e);
            renderWindow.RenderFrame += (es, e) => OnFrameRender();
            renderWindow.FocusedChanged += (es, e) => OnFocusedChanged(e);

            renderWindow.KeyDown += (es, e) => OnKeyboard(e);
        }

        static void SetUp()
        {
            Console.WriteLine("SetUp.");

            renderObjectsList.Add(new RenderObjects.ObjectCube());
            renderObjectsList.Add(new RenderObjects.ObjectCube());
            renderObjectsList.Add(new RenderObjects.ObjectParallelepiped());
            renderObjectsList.Add(new RenderObjects.ObjectCube());

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

        static void UpdateViewport()
        {
            GL.Viewport(0, 0, renderWindow.ClientRectangle.Width, renderWindow.ClientRectangle.Height);
        }

        static int ShaderLoadFromString(string source, ShaderType type, int programId)
        {
            int address = GL.CreateShader(type);

            GL.ShaderSource(address, source);
            GL.CompileShader(address);
            GL.AttachShader(programId, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));

            return address;
        }

        static int ShaderLoadFromFile(string fileName, ShaderType type, int programId)
        {
            var _fullPath = Configs.Dirs.dirShaders + fileName + ".glsl";

            Console.Write(_fullPath + " Error: ");

            using (StreamReader sr = new StreamReader(_fullPath))
            {
                return ShaderLoadFromString(sr.ReadToEnd(), type, programId);
            }
        }

        static void OnLoad()
        {
            Console.WriteLine("OnLoad.");

            renderWindow.VSync = VSyncMode.Off;
            if (Configs.Render.VSync) renderWindow.VSync = VSyncMode.On;

            SetUp();

            renderWindow.Title = Configs.Render.windowTitle;
            GL.ClearColor(Color.DarkBlue);
            GL.PointSize(5f);
        }

        static void OnResize()
        {
            Console.WriteLine("OnResize.");

            UpdateViewport();
        }

        static void OnFrameUpdate(FrameEventArgs e)
        {
            if (Configs.Render.showFPS)
            {
                var _title = Configs.Render.windowTitle + ", FPS: " + Math.Round(renderWindow.RenderFrequency).ToString();
                _title += ", Objects: " + renderObjectsList.Count.ToString();
                _title += ", Time: " + Math.Round(time).ToString() + "s";

                renderWindow.Title = _title;
            }

            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();

            int vertcount = 0;
            foreach (RenderObjects.ObjectBase v in renderObjectsList)
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

            time += (float)e.Time;

            renderObjectsList[0].Position = new Vector3(1.0f, 0.0f, 0.0f);
            renderObjectsList[0].Scale = new Vector3(1.0f, 1.0f, 1.0f);

            renderObjectsList[1].Position = new Vector3(3.0f, 0.0f, 0.0f);
            renderObjectsList[1].Scale = new Vector3(1.0f, 1.0f, 1.0f);

            renderObjectsList[2].Position = new Vector3(2.0f, 0.0f, 0.0f);
            renderObjectsList[2].Scale = new Vector3(1.0f, 1.0f, 1.0f);

            renderObjectsList[3].Position = new Vector3(2.0f, 2.7f, 0.0f);
            renderObjectsList[3].Scale = new Vector3(1.2f, 1.2f, 1.2f);

            foreach (RenderObjects.ObjectBase v in renderObjectsList)
            {
                v.CalculateModelMatrix();
                v.matrixProjectionView = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, (float)(renderWindow.ClientSize.Width / renderWindow.ClientSize.Height), 1.0f, 40.0f);
                v.matrixProjectionModelview = v.matrixModel * v.matrixProjectionView;
            }

            GL.UseProgram(glProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboElements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(dataVertexIndec.Length * sizeof(int)), dataVertexIndec, BufferUsageHint.StaticDraw);

            if (renderWindow.Focused)
            {
                Vector2 delta = mousePosLast - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);

                cam.AddRotation(delta.X, delta.Y);
                controlCursorReset();
            }
        }

        static void OnFrameRender()
        {
            UpdateViewport();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.EnableVertexAttribArray(shaderInVPos);
            GL.EnableVertexAttribArray(shaderInVCol);

            int indiceat = 0;

            foreach (RenderObjects.ObjectBase v in renderObjectsList)
            {
                GL.UniformMatrix4(shaderModelview, false, ref v.matrixProjectionModelview);
                GL.DrawElements(BeginMode.Triangles, v.countIndec, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.countIndec;
            }

            GL.DisableVertexAttribArray(shaderInVPos);
            GL.DisableVertexAttribArray(shaderInVPos);

            GL.Flush();
            renderWindow.SwapBuffers();
        }

        static void OnFocusedChanged(EventArgs e)
        {
            if (renderWindow.Focused)
            {
                controlCursorReset();
            }
        }

        static void OnKeyboard(KeyboardKeyEventArgs e)
        {
            Console.WriteLine("OnKeyboard: " + e.Key.ToString());

            if (e.Key == Key.Escape)
            {
                renderWindow.Exit();
            }

            switch (e.Key)
            {
                case Key.W:
                    cam.Move(0f, 0.1f, 0f);
                    break;
                case Key.A:
                    cam.Move(-0.1f, 0f, 0f);
                    break;
                case Key.S:
                    cam.Move(0f, -0.1f, 0f);
                    break;
                case Key.D:
                    cam.Move(0.1f, 0f, 0f);
                    break;
                case Key.Q:
                    cam.Move(0f, 0f, 0.1f);
                    break;
                case Key.E:
                    cam.Move(0f, 0f, -0.1f);
                    break;
            }
        }

        static void controlCursorReset()
        {
            OpenTK.Input.Mouse.SetPosition(renderWindow.Bounds.Left + renderWindow.Bounds.Width / 2, renderWindow.Bounds.Top + renderWindow.Bounds.Height / 2);
            mousePosLast = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }
    }
}
