using OpenTK;
using System;

namespace RenderObjects
{
    interface IRenderObject
    {
        Vector3 GetPos();
    }

    internal abstract class RObject: IRenderObject
    {
        Vector3 _pos;

        public Vector3 GetPos()
        {
            return _pos;
        }
    }
}
