using WorldMap.Common.Models.Shaders;
using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.Factories.Interfaces
{
    public interface IShaderFactory
    {
        IShader CreateShader(ShaderParameters parameters);
        IShaderProgram CreateShaderProgram(ShaderProgramParameters parameters);
        IShaderUniform<T> CreateShaderUnfiorm<T>(ShaderUniformParameters shaderUniformParameters);
    }
}
