using System;
using WorldMap.Common.Models.Enums;

namespace WorldMap.Common.Shaders.Interfaces;

/// <summary>
/// Wrapper for all the shaders -> [Vertex, Tesselation, Geometry, Fragment]
/// </summary>
public interface IShader : IDisposable
{
    /// <summary>
    /// Type of shader 
    /// </summary>
    ShadersTypes ShaderType { get; }
    /// <summary>
    /// Handle to the shader program
    /// </summary>
    uint ShaderHandle { get; }
    /// <summary>
    /// Compiles the shader and the result put in string error
    /// </summary>
    /// <returns>Whether it succeded or not</returns>
    bool Compile(out string error);
}
