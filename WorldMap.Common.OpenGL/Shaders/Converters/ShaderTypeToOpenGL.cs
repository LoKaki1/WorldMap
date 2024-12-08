using Silk.NET.OpenGL;
using WorldMap.Common.Models.Enums;

namespace WorldMap.Common.OpenGL.Shaders.Converters
{
    internal static class ShaderTypeToOpenGL
    {
        static readonly IDictionary<ShadersTypes, ShaderType> Converter = new Dictionary<ShadersTypes, ShaderType>()
        {
            {ShadersTypes.Vertex, ShaderType.VertexShader },
            {ShadersTypes.TesselationEvalution, ShaderType.TessEvaluationShader},
            {ShadersTypes.TesselationControl, ShaderType.TessControlShader},
            {ShadersTypes.Geometry, ShaderType.GeometryShader},
            {ShadersTypes.Fragmenet, ShaderType.FragmentShader}
        };
        public static ShaderType ConvertShader(ShadersTypes shaderType)
        {
            return Converter.TryGetValue(shaderType, out var shader) ? shader : ShaderType.VertexShader;
        }
    }
}
