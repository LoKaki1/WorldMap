using System;
using WorldMap.Common.Models.Enums;
using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.Shaders.Abstracts;

public abstract class ShaderBase : IShader
{
    protected string m_ShaderCode;
    public ShadersTypes ShaderType { get; }

    public abstract uint ShaderHandle { get; protected set; }

    public ShaderBase(ShadersTypes shaderType, string shader)
    {
        ShaderType = shaderType;
        m_ShaderCode = shader;
    }

    public abstract bool Compile(out string error);
    public abstract void Dispose();
}
