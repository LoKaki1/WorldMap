using System;
using WorldMap.Common.Buffers.Interfaces;
using WorldMap.Common.Models.Enums;

namespace WorldMap.Common.Buffers.Abstracts;

public abstract unsafe class VertexBufferBase<T> : IVertexBuffer<T> where T : unmanaged
{
    public uint VboHandle { get; protected set; }

    public uint VaoHandle { get; protected set; }

    public int VertexSize { get; protected set; }

    public int VertexNumber { get; protected set; }

    public Primitives BufferPrimitive { get;  set; }

    public VertexBufferBase(Primitives primitives)
    {
        VertexSize = sizeof(T);
        BufferPrimitive = primitives;
    }

    public abstract void BindAndDraw();

    public abstract void BufferData(int size, T* data);
}
