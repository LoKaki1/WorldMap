using Silk.NET.OpenGL;
using System.Numerics;
using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.OpenGL.Shaders
{
    public sealed unsafe class ShaderUniformGL<T> : IShaderUniform<T>
    {
        private static readonly IDictionary<Type, Action<int, T, GL>> m_Setters;
        private readonly Action<T> m_UniformSetter;
        static ShaderUniformGL()
        {
            m_Setters = new Dictionary<Type, Action<int, T, GL>>()
            {
                { typeof(int), (location, value, Gl) => Gl.Uniform1(location, (value is int p) ? p : 0) },
                { typeof(double), (location, value, Gl) => Gl.Uniform1(location, (value is double p) ? p : 0) },
                { typeof(float), (location, value, Gl) => Gl.Uniform1(location, (value is float p) ? p : 0) },
                { typeof(short), (location, value, Gl) => Gl.Uniform1(location, (value is short p) ? p : 0) },
                // etc
                {
                    typeof(Vector2),
                    (location, value, Gl) =>
                        {
                            if (value is Vector2 v2)
                            {
                                Gl.Uniform2(location, ref v2);
                            }
                        }
                },
                {
                    typeof(Vector3),
                    (location, value, Gl) =>
                        {
                            if (value is Vector3 v3)
                            {
                                Gl.Uniform3(location, ref v3);
                            }
                        }
                },
                { typeof(bool), (location, value, Gl) => Gl.Uniform1(location, (value is bool p) ? p ? 1 : 0 : 0) },
                {
                    typeof(System.Numerics.Matrix4x4),
                    (location, value, Gl) =>
                        {
                            #pragma warning disable CS8500
                            float* fixedData = (float*)&value;
                            #pragma warning restore CS8500
                            Gl.UniformMatrix4(location, 1, false, fixedData);
                        }
                }
            };
        }
        public IShaderProgram Program { get; private set; }
        public string Uniform { get; private set; }
        public int Location { get; private set; }

        public ShaderUniformGL(GL gl, IShaderProgram program, string uniform)
        {
            Program = program;
            Uniform = uniform;
            Location = Program.GetUniformLocation(Uniform);

            var isImplemented = m_Setters.TryGetValue(typeof(T), out var value);

            if (!isImplemented)
            {
                throw new NotImplementedException($"No implmentation of uniform for type {typeof(T)}");
            }
#pragma warning disable CS8604
            m_UniformSetter = (unfiromValue) => value?.Invoke(Location, unfiromValue, gl);
#pragma warning restore CS8604
        }


        public bool Set(T value)
        {
            m_UniformSetter.Invoke(value);

            return true;
        }
    }
}
