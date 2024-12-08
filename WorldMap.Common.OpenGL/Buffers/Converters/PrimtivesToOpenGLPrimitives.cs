using Silk.NET.OpenGL;
using WorldMap.Common.Models.Enums;

namespace WorldMap.Common.OpenGL.Buffers.Converters
{
    public static class PrimtivesToOpenGLPrimitives
    {
        static readonly Dictionary<Primitives, PrimitiveType> Converter =
             new()
             {
                 {Primitives.Quads, PrimitiveType.Quads},
                 {Primitives.Trinagles, PrimitiveType.Triangles},
                 {Primitives.TrianglesStrips, PrimitiveType.TriangleStrip},
                 {Primitives.TriangleFan, PrimitiveType.TriangleFan},
                 {Primitives.Lines, PrimitiveType.Lines},
                 {Primitives.LineStrip, PrimitiveType.LineStrip},
                 {Primitives.Patches, PrimitiveType.Patches},
                 {Primitives.Custom, PrimitiveType.Patches},
             };

        public static PrimitiveType Convert(Primitives primitives)
        {
            return Converter.TryGetValue(primitives, out PrimitiveType primitiveType) ? primitiveType : PrimitiveType.Triangles;
        }
    }
}
