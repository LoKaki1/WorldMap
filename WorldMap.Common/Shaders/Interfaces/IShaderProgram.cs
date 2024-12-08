using System;

namespace WorldMap.Common.Shaders.Interfaces;

/// <summary>
/// Wrapper for shader pipline
/// Activates the program should activate all the shader pipline stores in here
/// </summary>
public interface IShaderProgram : IDisposable
{
    /// <summary>
    /// All the valid shaders in this program.
    /// </summary>
    IShader[] Shaders { get; }

    /// <summary>
    /// Whether this progrram is currently running
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Handle of the program
    /// </summary>
    uint ProgramHandle { get; }

    /// <summary>
    /// Initalzie all the shaders in the program 
    /// </summary>
    /// <param name="errors">The errors of each shader if fails</param>
    /// <returns>Whether succeeded or not</returns>
    bool Initialize(out string[] errors);
    /// <summary>
    /// Gets the uniform 
    /// </summary>
    /// <param name="uniform">Uniform name</param>
    /// <returns>The uniform location</returns>
    int GetUniformLocation(string uniform);
    /// <summary>
    /// Set the current context to execute the program
    /// </summary>
    void UseProgram();
}
