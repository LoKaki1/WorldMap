using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.Models.Shaders
{
    public sealed class ShaderProgramParameters
    {
        public IShader[] Shaders { get; set; }

        public ShaderProgramParameters(IShader[] shaders)
        {
            Shaders = shaders;
        }
    }
}
