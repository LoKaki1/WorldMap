using System;
using WorldMap.Common.Models.Enums;

namespace WorldMap.Common.Buffers.Interfaces;

/// <summary>
/// Buffer to hold and kind of data to buffer the shader with,
/// </summary>
public unsafe interface IVertexBuffer<T> where T : unmanaged
{
    /// <summary>
    /// Handle for the vertex buffer object
    /// </summary>
    uint VboHandle { get; }
    /// <summary>
    /// Handle for the vertex array object
    /// </summary>
    uint VaoHandle { get; }
    /// <summary>
    /// Stores the size of a single vertex object in bytes
    /// 
    /// Example, in the `HeightMapVertex` we only store the height of the vertex in a single float, 
    /// which means the size would be 4 bytes
    /// </summary>
    int VertexSize { get; }
    /// <summary>
    /// Stores the number of vertex
    /// </summary>
    int VertexNumber { get; }
    /// <summary>
    /// The buffer primitives types we will draw like trinagles, trinagles strip etc,
    /// </summary>
    Primitives BufferPrimitive { get; set; }

    
    /// <summary>
    /// Bind the buffer to the current program and executes the program on the buffer
    /// 
    /// (IMPORTANT what I described is true for openGL if we move API move this documentation to the open gl implmenetation and update the documentation)
    /// </summary>
    void BindAndDraw();

    /// <summary>
    /// Buffer the data to the GPU context 
    /// </summary>
    void BufferData(int size, T* data);

}
