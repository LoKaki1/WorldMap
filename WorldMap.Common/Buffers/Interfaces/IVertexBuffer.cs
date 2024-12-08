using System;

namespace WorldMap.Common.Buffers.Interfaces;

/// <summary>
/// Buffer to hold and kind of data to buffer the shader with,
/// </summary>
public interface IVertexBuffer<T> where T : unmanaged
{
    /// <summary>
    /// Handle for the vertex buffer object
    /// </summary>
    public uint VboHandle { get; }
    /// <summary>
    /// Handle for the vertex array object
    /// </summary>
    public uint VaoHandle { get; }
    /// <summary>
    /// Stores the size of a single vertex object in bytes
    /// 
    /// Example, in the `HeightMapVertex` we only store the height of the vertex in a single float, 
    /// which means the size would be 4 bytes
    /// </summary>
    public int VertexSize { get; }
}
