using Silk.NET.OpenGL;
using WorldMap.Common.Buffers.Abstracts;
using WorldMap.Common.Models.Enums;
using WorldMap.Common.OpenGL.Buffers.Converters;

namespace WorldMap.Common.OpenGL.Buffers
{
    public unsafe class VertexBufferGL<T> : VertexBufferBase<T> where T : unmanaged
    {
        private readonly BufferTargetARB m_BufferTargetARB;
        private readonly BufferUsageARB m_BufferUsageARB;
        private readonly VertexAttribPointerType m_VertexAttribPointerType;

        public bool IsBufferd { get; protected set; }
        public GL Gl { get; set; }
        public PrimitiveType PrimitiveType { get; set; }

        public VertexBufferGL(GL gl,
                              Primitives primitiveType,
                              BufferTargetARB bufferTargetARB,
                              BufferUsageARB bufferUsageARB,
                              VertexAttribPointerType vertexAttribPointerType)
            : base(primitiveType)
        {
            m_BufferTargetARB = bufferTargetARB;
            m_BufferUsageARB = bufferUsageARB;
            m_VertexAttribPointerType = vertexAttribPointerType;
            Gl = gl;
            PrimitiveType = PrimtivesToOpenGLPrimitives.Convert(primitiveType);
            // Create a VAO
            VaoHandle = Gl.GenVertexArray();
            Gl.BindVertexArray(VaoHandle);

            // Create a VBO
            VboHandle = Gl.GenBuffer();
            Gl.BindBuffer(m_BufferTargetARB, VboHandle);

            // Set up attribs
            SetupVAO();
            // Clean up
            UnbindVAO();
            UnbindVBO();
        }

        public override void BindAndDraw()
        {
            BindVAO();
            Gl.DrawArrays(PrimitiveType, 0, (uint)VertexNumber);
            UnbindVAO();
        }

        public override unsafe void BufferData(int size, T* data)
        {
            VertexNumber = size;

            BindVBO();
            Gl.BufferData(m_BufferTargetARB,
                           (uint)(size * VertexNumber),
                           data,
                           m_BufferUsageARB);
            UnbindVBO();

            IsBufferd = true;
        }

        protected virtual void SetupVAO()
        {
            Gl.EnableVertexAttribArray(0);

            Gl.VertexAttribPointer(0,
                                   1,
                                   m_VertexAttribPointerType,
                                   false,
                                   (uint) VertexSize,
                                   null);
        }
        // Shortcut functions
        void BindVAO() => Gl.BindVertexArray(VaoHandle);
        void BindVBO() => Gl.BindBuffer(m_BufferTargetARB, VboHandle);
        void UnbindVAO() => Gl.BindVertexArray(0);
        void UnbindVBO() => Gl.BindBuffer(m_BufferTargetARB, 0);
    }
}

