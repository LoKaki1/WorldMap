using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.Models.Shaders
{
    public class ShaderUniformParameters
    {
        public IShaderProgram  ShaderProgram { get; set; }
        public string Name { get; set; }

        public ShaderUniformParameters(IShaderProgram shaderProgram, string name)
        {
            ShaderProgram = shaderProgram;
            Name = name;
        }

    }
}
