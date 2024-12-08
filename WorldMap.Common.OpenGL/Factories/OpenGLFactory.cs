﻿using Silk.NET.OpenGL;
using WorldMap.Common.Buffers.Interfaces;
using WorldMap.Common.Factories.Interfaces;
using WorldMap.Common.Models.Buffers;
using WorldMap.Common.Models.Shaders;
using WorldMap.Common.OpenGL.Buffers;
using WorldMap.Common.OpenGL.Shaders;
using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.OpenGL.Factories
{
    public sealed class OpenGLFactory : IBufferFactory, IShaderFactory
    {
        public GL Gl { get; set; }

        public IShader CreateShader(ShaderParameters parameters)
        {
            var shader = new ShaderGL(Gl, parameters.Type, parameters.ShaderCode);

            return shader;
        }

        public IShaderProgram CreateShaderProgram(ShaderProgramParameters parameters)
        {
            return new ShaderProgramGL(Gl, 
                                       parameters.Shaders.OfType<ShaderGL>().ToArray());
        }

        public IShaderUniform<T> CreateShaderUnfiorm<T>(ShaderUniformParameters shaderUniformParameters)
        {
            return new ShaderUniformGL<T>(Gl,
                                          shaderUniformParameters.ShaderProgram,
                                          shaderUniformParameters.Name);
        }

        public IVertexBuffer<T> CreateVertexBuffer<T>(VertexBufferParameters vertexBufferParameters) where T : unmanaged
        {
            return new VertexBufferGL<T>(Gl,
                                         vertexBufferParameters.Primitive,
                                         BufferTargetARB.ArrayBuffer,
                                         BufferUsageARB.StreamDraw,
                                         VertexAttribPointerType.Float);
        }
    }
}