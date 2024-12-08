using WorldMap.Common.Buffers.Interfaces;
using WorldMap.Common.Models.Enums;

namespace WorldMap.Common.Models.Buffers
{
    public class VertexBufferParameters
    {
        public Primitives Primitive { get; set; }
        public Action? OnBuffered { get; set; }
        public VertexBufferParameters(Primitives primitive,
                                      Action onBuffered = null)   
        {
            Primitive = primitive;
            OnBuffered = onBuffered;
        }
    }
}
