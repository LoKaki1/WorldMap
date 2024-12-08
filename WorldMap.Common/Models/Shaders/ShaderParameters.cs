using WorldMap.Common.Models.Enums;

namespace WorldMap.Common.Models.Shaders
{
    public struct ShaderParameters
    {
        public string ShaderCode { get; set; }
        public ShadersTypes Type { get; set; }

        public ShaderParameters(string shaderProgram, ShadersTypes type)
        {
            ShaderCode = shaderProgram;
            Type = type;
        }
    }
}
