using System.Numerics;
using System.Runtime.InteropServices;

namespace WorldMap.Map.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct QuadsShaderIndirectUniformStorage
    {
        public  Vector2[] QuadPostion { get; set; }
        public  uint[] TriangleSize { get; set; }
        public QuadsShaderIndirectUniformStorage(Vector2[] quadPostion, uint[] triangleSize)
        {
            QuadPostion = quadPostion;
            TriangleSize = triangleSize;
        }

        public QuadsShaderIndirectUniformStorage()
        {
            QuadPostion = [];
            TriangleSize = [];
        }

    }
}
