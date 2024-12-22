using Silk.NET.OpenGL;
using WorldMap.Common.Buffers.Interfaces;
using WorldMap.Common.Models.Buffers;
using WorldMap.Common.Models.Enums;
using WorldMap.Common.OpenGL.Pipline;
using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.OpenGL.Buffers
{
    public sealed unsafe class ShaderStorageBufferGL<T> : IShaderStorageBuffer<T>
         where T : unmanaged
    {

        #region Cross-platform Data

        // All of this can be nullable becase no need to use this when we use the ssbo
        
        private bool m_IsSSBOAvaliable;
        private string m_ShaderStorageBufferName;
        private IShaderProgram? m_Program;
        private uint m_UniformStorageLocation;

        #endregion
        
        private uint m_BindingPoint;
        private readonly ShaderStorageBufferParameters<T> m_Params;

        public BufferTargetARB BufferTargetARB { get; private set; }
        public uint SSBO { get; private set; }
        public SSBOTypes Type { get; private set; }
        public GL Gl { get; private set; }

        public ShaderStorageBufferGL(GL glContext, ShaderStorageBufferParameters<T> shaderStorageBufferParameters)
        {
            Gl = glContext;
            Type = shaderStorageBufferParameters.SSBOTypes;
            m_Params = shaderStorageBufferParameters;

            // Means the ssbo isn't an option here thefore we must use the uniform buffer
            //m_IsSSBOAvaliable = Type != SSBOTypes.UniformBuffer;
            m_IsSSBOAvaliable = false;
            m_ShaderStorageBufferName = shaderStorageBufferParameters.SSBOName;
            m_BindingPoint = shaderStorageBufferParameters.BindingPoint;
            
            PiplineGL.EnqueToPipline(() =>
            {
                if (!m_IsSSBOAvaliable)
                {
                    BufferTargetARB = BufferTargetARB.UniformBuffer;
                    m_Program = shaderStorageBufferParameters.ShaderProgram;
                    m_UniformStorageLocation = Gl.GetUniformBlockIndex(m_Program.ProgramHandle, m_ShaderStorageBufferName);
                    SSBO = Gl.GenBuffer();
                    Gl.BindBuffer(BufferTargetARB.UniformBuffer, SSBO);
                    Gl.BindBufferBase(BufferTargetARB.UniformBuffer, m_UniformStorageLocation, SSBO);
                    Gl.BufferData(BufferTargetARB.UniformBuffer,
                                  (nuint) shaderStorageBufferParameters.Size,
                                  shaderStorageBufferParameters.Data,
                                  BufferUsageARB.DynamicDraw);
                    Gl.UniformBlockBinding(m_Program.ProgramHandle, m_UniformStorageLocation, m_BindingPoint);
                    Gl.BindBuffer(BufferTargetARB.UniformBuffer, 0);

                }
                else
                {
                    BufferTargetARB = BufferTargetARB.ShaderStorageBuffer;
                    SSBO = Gl.GenBuffer();
                    Gl.BindBuffer(BufferTargetARB.ShaderStorageBuffer, SSBO);
                    Gl.BindBufferBase(BufferTargetARB.ShaderStorageBuffer, m_BindingPoint, SSBO);
                    Gl.BufferData(BufferTargetARB.ShaderStorageBuffer,
                                  (nuint)shaderStorageBufferParameters.Size,
                                  shaderStorageBufferParameters.Data,
                                  BufferUsageARB.DynamicDraw);
                    Gl.BindBuffer(BufferTargetARB.ShaderStorageBuffer, 0);
                }
            });
        }

        public unsafe void BufferObject(T* data,
                                        Action? action = null)
        {
            PiplineGL.EnqueToPipline(() =>
            {

                BindSSBO();
                Gl.BindBufferBase(BufferTargetARB, m_BindingPoint, SSBO);
                Gl.BufferSubData(BufferTargetARB,
                                0,
                                (nuint)m_Params.Size,
                                data);
                UnbindSSBO();

                action?.Invoke();
            });
        }

        void BindSSBO() => Gl.BindBuffer(BufferTargetARB, SSBO);
        void UnbindSSBO() => Gl.BindBuffer(BufferTargetARB, 0);
    }
}
