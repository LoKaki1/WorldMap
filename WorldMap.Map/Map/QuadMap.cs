using System.Numerics;
using System.Runtime.InteropServices;
using WorldMap.Common.Allocators;
using WorldMap.Common.Buffers.Interfaces;
using WorldMap.Common.Camera.Interfaces;
using WorldMap.Common.Factories.Interfaces;
using WorldMap.Common.Models.Buffers;
using WorldMap.Common.Models.Enums;
using WorldMap.Map.Map.Interfaces;
using WorldMap.Map.QuadTrees;
using WorldMap.Map.Shaders;
using WorldMap.Map.Vertices;
using WorldMap.Map.WorldContants;

namespace WorldMap.Map.Map
{
    public unsafe class QuadMap : IMap
    {
        private readonly ICameraContoller m_CameraController;
        private readonly QuadTree m_QuadTree;
        private readonly IVertexBuffer<MapVertex> m_VertexBuffer;
        private readonly MapShader m_Shader;
        private readonly IBufferFactory m_Factory;
        private readonly IShaderStorageBuffer<QuadsShaderIndirectUniformStorage> m_SSBO;
        private readonly int m_Size = Constants.WORLD_SIZE ;
        private readonly int m_MaxZoom = Constants.WORLD_ZOOM_LEVELS;
        private readonly int m_MaxQuadsInMemory = 32;
        public bool IsUpdating { get; private set; }
        public QuadMap(ICameraContoller cameraController,
                        IShaderFactory shaderFactory,
                       IBufferFactory bufferFactory)
        {
            m_CameraController = cameraController;
            m_QuadTree = new(m_Size, m_MaxZoom, m_MaxQuadsInMemory);
            m_VertexBuffer = bufferFactory.CreateVertexBuffer<MapVertex>(new VertexBufferParameters(Primitives.TrianglesStrips));
            m_Shader = new MapShader(shaderFactory);
            m_Factory = bufferFactory;
        }

        public void Render()
        {

        }

        public void Update()
        {
            if (IsUpdating) return;

            IsUpdating = true;

            m_QuadTree.Update(m_CameraController.Position, 512);
            var node = m_QuadTree.Root;
            var numberOfChildren = node.GetNumberOfChildren();
            int size = numberOfChildren * Constants.VERTICES_PER_CHUNK;
            int sizeBytes = Marshal.SizeOf<MapVertex>() * size;
            var uniforms = node.GetQuadsShaderIndirectUniform();
            var sizeOfUniforms = numberOfChildren * sizeof(Vector2) + numberOfChildren * sizeof(uint);
            var aaa = Allocator.Alloc<QuadsShaderIndirectUniformStorage>(sizeBytes);
            m_SSBO = m_Factory.CreateShaderStorageBuffer(new ShaderStorageBufferParameters<QuadsShaderIndirectUniformStorage>(SSBOTypes.UniformBuffer,m_Shader.ShaderProgram, ))
            var offset = Allocator.Alloc<MapVertex>(sizeBytes);

            m_VertexBuffer.BufferVertexData(size, offset, () =>
            {
                Allocator.Free(ref offset, ref sizeBytes);
                IsUpdating = false;
            });

        }
    }
}
