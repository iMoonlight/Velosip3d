using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Velosip3d
{
    static class Render
    {
        static GameWindow renderWindow;

        static int frames = 0;

        public static void Init()
        {
                renderWindow = new GameWindow(Configs.Render.windowSize[0], Configs.Render.windowSize[1]);

                RenderFactory.Init();
                InitEvents();

                renderWindow.Run();
        }

        static void InitEvents()
        {
            renderWindow.Load += (es, e) => OnLoad();
            renderWindow.Resize += (es, e) => OnResize();
            renderWindow.UpdateFrame += (es, e) => OnFrameUpdate();
            renderWindow.RenderFrame += (es, e) => OnFrameRender();
        }

        static void UpdateViewport()
        {
            int _width = renderWindow.Width;
            int _height = renderWindow.Height;

            if ((_width * _height) == 0) return;
            if (_width < _height) return;

            float _aspect = _width / _height;
            float _fov = (float)((Math.PI / 180) * Configs.Render.FOV);

            GL.Viewport(renderWindow.ClientRectangle.X, renderWindow.ClientRectangle.Y, renderWindow.ClientRectangle.Width, renderWindow.ClientRectangle.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(_fov, _aspect, 1.0f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            Console.WriteLine("Viewport updated!");
        }

        static void OnLoad()
        {
            renderWindow.VSync = VSyncMode.Off;
            if (Configs.Render.VSync) renderWindow.VSync = VSyncMode.On;
        }

        static void OnResize()
        {
            UpdateViewport();
        }

        static void OnFrameUpdate()
        {
            renderWindow.Title = Configs.Render.windowTitle;
            if (Configs.Render.showFPS) renderWindow.Title += ", FPS: " + Math.Round(renderWindow.RenderFrequency).ToString();

            //Test
            GL.BindBuffer(BufferTarget.ArrayBuffer, RenderFactory.vboPosition);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(RenderFactory.dataVertex.Length * Vector3.SizeInBytes), RenderFactory.dataVertex, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(RenderFactory.shaderInVPos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, RenderFactory.vboColor);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(RenderFactory.dataColor.Length * Vector3.SizeInBytes), RenderFactory.dataColor, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(RenderFactory.shaderInVCol, 3, VertexAttribPointerType.Float, true, 0, 0);

            //Rotation by frames
            RenderFactory.dataModelview[0] = Matrix4.CreateRotationY(0.001f * frames) * Matrix4.CreateRotationX(0.15f * frames) * Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f) * Matrix4.CreatePerspectiveFieldOfView(1.3f, renderWindow.ClientSize.Width / (float)renderWindow.ClientSize.Height, 1.0f, 40.0f);

            GL.UniformMatrix4(RenderFactory.shaderModelview, false, ref RenderFactory.dataModelview[0]);

            GL.UseProgram(RenderFactory.glProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, RenderFactory.iboElements);

            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(RenderFactory.dataVertexIndex.Length * sizeof(int)), RenderFactory.dataVertexIndex, BufferUsageHint.StaticDraw);
        }

        static void OnFrameRender()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Aqua);

            GL.Enable(EnableCap.DepthTest);

            //Draw from bufers
            GL.EnableVertexAttribArray(RenderFactory.shaderInVPos);
            GL.EnableVertexAttribArray(RenderFactory.shaderInVCol);

            GL.DrawElements(BeginMode.Triangles, RenderFactory.dataVertexIndex.Length, DrawElementsType.UnsignedInt, 0);

            GL.DisableVertexAttribArray(RenderFactory.shaderInVPos);
            GL.DisableVertexAttribArray(RenderFactory.shaderInVCol);

            GL.Flush();
            renderWindow.SwapBuffers();

            frames += 1;
        }
    }
}
