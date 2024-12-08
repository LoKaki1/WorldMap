using System;
using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.Shaders.Abstracts;

public abstract class ShaderProgramBase : IShaderProgram
{
    public IShader[] Shaders { get; protected set; }

    public bool IsActive { get; protected set; }

    public uint ProgramHandle { get; protected set; }
    public ShaderProgramBase(IShader[] shaders)
    {
        Shaders = shaders;
    }


    public virtual void Dispose()
    {
        IsActive = false;
        foreach (var shader in Shaders)
        {
            shader.Dispose();
        }
    }
    public abstract int GetUniformLocation(string uniform);
    public abstract bool Initialize(out string[] errors);
    public virtual void UseProgram()
    {
        IsActive = true;
    }
}
