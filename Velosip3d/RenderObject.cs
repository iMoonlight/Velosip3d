using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Velosip3d
{
    class RenderObject
    {
        public int vboPosition;
        public int vboColor;
        public int vboModelview;
        public int iboElements;

        public Vector3[] dataVertex;
        public Vector3[] dataColor;
        public Matrix4[] dataModelview;
        public int[] dataVertexIndex;
    }
}
