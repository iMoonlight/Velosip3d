using OpenTK;

namespace RenderObjects
{
    public abstract class BaseObject
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        public int countVertex;
        public int countIndec;
        public int countColorData;
        public Matrix4 matrixModel = Matrix4.Identity;
        public Matrix4 matrixProjectionView = Matrix4.Identity;
        public Matrix4 matrixProjectionModelview = Matrix4.Identity;

        public abstract Vector3[] GetVerts();
        public abstract int[] GetIndices(int offset = 0);
        public abstract Vector3[] GetColorData();
        public abstract void CalculateModelMatrix();
    }

    public abstract class BaseLight
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
    }
}
