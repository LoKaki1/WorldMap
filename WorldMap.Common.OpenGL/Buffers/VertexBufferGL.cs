using Silk.NET.OpenGL;
using WorldMap.Common.Buffers.Abstracts;
using WorldMap.Common.Models.Buffers;
using WorldMap.Common.Models.Enums;
using WorldMap.Common.OpenGL.Buffers.Converters;
using WorldMap.Common.OpenGL.Pipline;

namespace WorldMap.Common.OpenGL.Buffers
{
    public unsafe class VertexBufferGL<T> : VertexBufferBase<T> where T : unmanaged
    {
        private readonly VertexAttribPointerType m_VertexAttribPointerType;

        public bool IsVerticesBufferd { get; protected set; }
        public bool IsSSBOBuffered { get; protected set; }
        public GL Gl { get; set; }
        public PrimitiveType PrimitiveType { get; set; }
        public uint SSBOHandle { get; }

        public VertexBufferGL(GL gl,
                              VertexBufferParameters parameters,
                              VertexAttribPointerType vertexAttribPointerType)
            : base(parameters)
        {
            // m_BufferTargetARB = bufferTargetARB;
            // m_BufferUsageARB = bufferUsageARB;
            m_VertexAttribPointerType = vertexAttribPointerType;
            Gl = gl;
            PrimitiveType = PrimtivesToOpenGLPrimitives.Convert(parameters.Primitive);
            // Create a VAO
            VaoHandle = Gl.GenVertexArray();
            Gl.BindVertexArray(VaoHandle);

            // Create a VBO
            VboHandle = Gl.GenBuffer();
            Gl.BindBuffer(BufferTargetARB.ArrayBuffer, VboHandle);
            // Set up attribs
  
            // uint bindingPoint = 0; // Choose a binding point
            // Gl.BindBufferBase(GLEnum.UniformBuffer, bindingPoint, SSBOHandle);
            SetupVAO();
            // Clean up
            UnbindVAO();
            UnbindVBO();
            //UnbindSSBO();
        }

        public override void Draw()
        {
            BindVAO();
            Gl.FrontFace(FrontFaceDirection.Ccw);
            Gl.DrawArrays(PrimitiveType, 0, (uint)VertexNumber);
            UnbindVAO();
        }

        /// <summary>
        /// On open gl this function can take a lot of time and must happend on the main thread therefore we must enque it to the next render call to not cause a freeze 
        /// Also we will create an engine which divide the buffer to little parts with the function `GL.BufferSubData`
        /// </summary>
        /// <param name="size"></param>
        /// <param name="data"></param>
        public override unsafe void BufferVertexData(int size, T* data, Action? onBuffer = null)
        {
            var actualOnBuffer = onBuffer ?? OnBuffered;

            PiplineGL.EnqueToPipline(() =>
            {
                VertexNumber = size;
                BindVBO();
                Gl.BufferData(BufferTargetARB.ArrayBuffer,
                               (uint)(size * VertexSize),
                               data,
                               BufferUsageARB.StaticDraw);
                UnbindVBO();

                IsVerticesBufferd = true;
                actualOnBuffer?.Invoke();
            });
        }

        protected virtual void SetupVAO()
        {
            Gl.EnableVertexAttribArray(0);
            Gl.VertexAttribPointer(0,
                                   1,
                                   m_VertexAttribPointerType,
                                   false,
                                   (uint)VertexSize,
                                   null);
        }
        //public override unsafe void BufferSSBO<SSBO>(int size,
        //                                             SSBO* data,
        //                                             Action? onBuffer = null)
        //{
        //    PiplineGL.EnqueToPipline(() =>
        //     {
        //         BindSSBO();
        //         Gl.BindBufferBase(BufferTargetARB.UniformBuffer, 0u, SSBOHandle);
        //         Gl.BufferData(BufferTargetARB.UniformBuffer,
        //                          (uint)size,
        //                          data,
        //                          BufferUsageARB.StaticDraw);
        //         UnbindSSBO();

        //         IsVerticesBufferd = true;
        //         onBuffer?.Invoke();
        //     });
        //}
        // Shortcut functions
        void BindVAO() => Gl.BindVertexArray(VaoHandle);
        void BindVBO() => Gl.BindBuffer(BufferTargetARB.ArrayBuffer, VboHandle);
        //void BindSSBO() => Gl.BindBuffer(BufferTargetARB.UniformBuffer, SSBOHandle);
        //void UnbindSSBO() => Gl.BindBuffer(BufferTargetARB.UniformBuffer, 0);
        void UnbindVAO() => Gl.BindVertexArray(0);
        void UnbindVBO() => Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
    }
}

