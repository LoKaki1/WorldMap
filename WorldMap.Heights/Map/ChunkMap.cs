﻿using System.Numerics;
using System.Runtime.InteropServices;
using WorldMap.Common.Allocators;
using WorldMap.Common.Buffers.Interfaces;
using WorldMap.Common.Camera.Interfaces;
using WorldMap.Common.Factories.Interfaces;
using WorldMap.Common.Models.Buffers;
using WorldMap.Common.Models.Enums;
using WorldMap.Common.Shaders.Interfaces;
using WorldMap.Heights.Map.Interfaces;
using WorldMap.Heights.Shaders;
using WorldMap.Heights.TempCauseImLazy;
using WorldMap.Heights.Vertex;

namespace WorldMap.Heights.Map
{
    public unsafe sealed class ChunkMap : IMap
    {
        private readonly IVertexBuffer<HeightVertex> m_VertexBuffer;
        private readonly ICameraContoller m_CameraController;

        public bool IsRendering { get; private set; }
        public bool IsUpdating { get; private set; }
        private bool m_IsReady;

        public ChunkMap(IBufferFactory factory,
                        ICameraContoller cameraContoller)
        {
            var vertexParams = new VertexBufferParameters(Primitives.TrianglesStrips);
            m_VertexBuffer = factory.CreateVertexBuffer<HeightVertex>(vertexParams);
            m_CameraController = cameraContoller;
        }

        public void Render()
        {
            IsRendering = true;
            //// First let opengl that we want to use this shader now
            //m_MapShader.UseShader();
            //// creation the projection
            //var projection = m_CameraController.GetViewProjection();
            //// Set the projection in the uniform
            //m_MapShader.ModelViewProjection.Set(projection);

            //// Bind the current shader to the buffer and draw
            //m_VertexBuffer.Draw();
            //IsRendering = false;
        }

        public void Update()
        {
            if (m_IsReady || IsUpdating) return;

            IsUpdating = true;
            GenerateMap();
        }

        unsafe void GenerateMap()
        {
            var bytes_vertexData = Marshal.SizeOf<HeightVertex>() * Constants.VERTICES_PER_CHUNK;
            var offset = Allocator.Alloc<HeightVertex>(bytes_vertexData);
            var write = offset;

            for (int z = 0; z < Constants.HEIGHTMAP_SIZE; z++)
            {
                int x = 0;

                var altitude0 = 0;
                var altitude1 = 0;
                var altitude2 = 0;


                // First vertex is a degenerate
                write++->Y = altitude0;


                // Create the first triangle
                write++->Y = altitude0;
                write++->Y = altitude1;
                write++->Y = altitude2;
                // Rest of the strip
                x += 1;
                var altitude = 0;
                write++->Y = altitude;

                x += 1;

                for (; x <= Constants.HEIGHTMAP_SIZE; x++)
                {
                    altitude = 0;
                    write++->Y = altitude;

                    altitude = 0;
                    write++->Y = altitude;

                }

                // Degenerate
                altitude = 0;
                write++->Y = altitude;
            }

            m_VertexBuffer.BufferVertexData(Constants.VERTICES_PER_CHUNK, offset, () =>
            {
                Allocator.Free(ref offset, ref bytes_vertexData);
                IsUpdating = false;
                m_IsReady = true;
            });
        }
    }
}
