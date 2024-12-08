using WorldMap.Common.Buffers.Interfaces;
using WorldMap.Common.Factories.Interfaces;
using WorldMap.Common.Models.Buffers;
using WorldMap.Common.Models.Enums;
using WorldMap.Heights.Map.Interfaces;
using WorldMap.Heights.Vertex;

namespace WorldMap.Heights.Map
{
    public sealed class StaticMap : IMap
    {
        private readonly IVertexBuffer<HeightVertex> m_VertexBuffer;

        public bool IsRendering { get; private set; }

        public bool IsUpdaeting { get; private set; }

        public StaticMap(IBufferFactory factory,
                         IShaderFactory shaderFactory)
        {
            var vertexParams = new VertexBufferParameters(Primitives.TrianglesStrips);
            m_VertexBuffer = factory.CreateVertexBuffer<HeightVertex>(vertexParams);
        }

        public void Render()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
