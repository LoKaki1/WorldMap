using WorldMap.Common.Buffers.Interfaces;
using WorldMap.Common.Models.Buffers;
using WorldMap.Common.Models.Enums;

namespace WorldMap.Common.Buffers.Abstracts;

public abstract unsafe class VertexBufferBase<T> : IVertexBuffer<T> where T : unmanaged
{
    public uint VboHandle { get; protected set; }

    public uint VaoHandle { get; protected set; }

    public int VertexSize { get; protected set; }

    public int VertexNumber { get; protected set; }

    public Primitives BufferPrimitive { get; set; }
    public Action? OnBuffered { get; }

    public VertexBufferBase(VertexBufferParameters parameters)
    {
        VertexSize = sizeof(T);
        BufferPrimitive = parameters.Primitive;
        OnBuffered = parameters.OnBuffered;
    }

    public abstract void BindAndDraw();

    public abstract void BufferVertexData(int size, T* data, Action? onBuffer = null);
    public abstract void BufferSSBO<SSBO>(int size, SSBO* data, Action? onBuffer = null) where SSBO : unmanaged;
}
