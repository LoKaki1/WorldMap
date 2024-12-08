using System;

namespace WorldMap.Common.Shaders.Interfaces;

/// <summary>
/// Wrraper for all values we can put inside a shader uniform
/// </summary>
/// <typeparam name="T">The type of the uniform</typeparam>
public interface IShaderUniform<T>
{
    /// <summary>
    /// The program which is related to this shader uniform 
    /// </summary>
    IShaderProgram Program { get; }
    /// <summary>
    ///  The uniform name 
    /// </summary>
    string Uniform { get; }
    /// <summary>
    /// Set the value in the uniform
    /// </summary>
    /// <param name="value"></param>
    /// <returns>if the set succeded</returns>
    bool Set(T value);
}
