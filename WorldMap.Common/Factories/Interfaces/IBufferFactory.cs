using WorldMap.Common.Buffers.Interfaces;
using WorldMap.Common.Models.Buffers;

namespace WorldMap.Common.Factories.Interfaces
{
    public interface IBufferFactory
    {
        IVertexBuffer<T> CreateVertexBuffer<T>(VertexBufferParameters vertexBufferParameters) 
            where T : unmanaged;
    }
}
