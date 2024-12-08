using System;
using WorldMap.Common.Buffers.Interfaces;

namespace WorldMap.Common.Buffers.Abstracts;

public abstract unsafe class VertexBufferBase<T> : IVertexBuffer<T> where T : unmanaged
{
    public uint VboHandle { get; }

    public uint VaoHandle { get; }

    public int VertexSize { get; }

    public VertexBufferBase()
    {
        VertexSize = sizeof(T);
    }
}
