using Silk.NET.OpenGL;
using System.Xml.Linq;
using System;
using WorldMap.Common.OpenGL.Shaders.Converters;
using WorldMap.Common.Shaders.Interfaces;
using static Silk.NET.Core.Native.WinString;

namespace WorldMap.Common.OpenGL.Shaders
{
    public sealed class ShaderUniformGL<T> : IShaderUniform<T>
    {
        private readonly Action<T> m_UniformSetter;

        public IShaderProgram Program { get; private set; }
        public string Uniform { get; private set; }
        public int Location { get; private set; }   

        public ShaderUniformGL(GL gl, IShaderProgram program, string uniform)
        {
            Program = program;
            Uniform = uniform;
            Location = Program.GetUniformLocation(Uniform);
            
            var isImplemented = UniformConverter.Converter.TryGetValue(typeof(T), out var value);
            
            if (!isImplemented)
            {
                throw new NotImplementedException($"No implmentation of uniform for type {typeof(T)}");
            }

            m_UniformSetter = (unfiromValue) => value.Invoke(Location, unfiromValue, gl);
        }


        public bool Set(T value)
        {
            m_UniformSetter.Invoke(value);

            return true;
        }
    }
}
