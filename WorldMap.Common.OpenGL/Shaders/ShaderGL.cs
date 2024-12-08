using Silk.NET.OpenGL;
using WorldMap.Common.Models.Enums;
using WorldMap.Common.OpenGL.Shaders.Converters;
using WorldMap.Common.Shaders.Abstracts;

namespace WorldMap.Common.OpenGL.Shaders
{
    public sealed class ShaderGL : ShaderBase
    {
        public GL Gl { get; set; }
        public ShaderType ShaderTypeGL { get; set; }    
        public override uint ShaderHandle { get; protected set ; }
        
        public ShaderGL(GL gl,
                        ShadersTypes shaderType,
                        string shader) : base(shaderType, shader)
        {
            Gl = gl;
            ShaderTypeGL = ShaderTypeToOpenGL.ConvertShader(shaderType);
        }


        public override bool Compile(out string error)
        {
            ShaderHandle = Gl.CreateShader(ShaderTypeGL);
            Gl.ShaderSource(ShaderHandle, m_ShaderCode);
            Gl.CompileShader(ShaderHandle);

            // Check the log
            Gl.GetShader(ShaderHandle, ShaderParameterName.InfoLogLength, out var logLen);

            if (logLen > 0)
            {
                Gl.GetShaderInfoLog(ShaderHandle, (uint)logLen, out _, out string log);

                error = log;
            }
            else
            {
                error = string.Empty;
            }


            // Check it compiled successfully
            Gl.GetShader(ShaderHandle, ShaderParameterName.CompileStatus, out var status);

            if (status == (int)GLEnum.True)
            {
                return true;
            }
            // Delete it
            Gl.DeleteShader(ShaderHandle);
            ShaderHandle = 0;

            return false;
        }

        public override void Dispose()
        {
            // Already disposed
            if (ShaderHandle == 0)
            {
                return;
            }

            Gl.DeleteShader(ShaderHandle);
            ShaderHandle = 0;
        }
    }
}
