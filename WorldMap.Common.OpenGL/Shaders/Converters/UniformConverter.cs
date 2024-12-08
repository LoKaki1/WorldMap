using Silk.NET.OpenGL;
using System.Numerics;

namespace WorldMap.Common.OpenGL.Shaders.Converters
{
    public unsafe static class UniformConverter
    {
        public static  Dictionary<Type, Action<int, object, GL>> Converter = new Dictionary<Type, Action<int, object, GL>>()
        {
            {typeof(int), (location, value, gl) => gl.Uniform1(location, (value is int p) ? p : 0)},
            {typeof(double), (location, value, gl) => gl.Uniform1(location, (value is double p) ? p : 0)},
            { typeof(float), (location, value, gl) => gl.Uniform1(location, (value is float p) ? p : 0)},
            { typeof(short), (location, value, gl) => gl.Uniform1(location, (value is short p) ? p : 0)},
            // etc
            {
    typeof(Vector2),
                (location, value, gl) =>
                {
                    if (value is Vector2 v2)
                    {
                        gl.Uniform2(location, ref v2);
                    }
                }
            },
            {
    typeof(Vector3),
                (location, value, gl) =>
                {
                    if (value is Vector3 v3)
                    {
                        gl.Uniform3(location, ref v3);
                    }
                }
            },
            { typeof(bool), (location, value, gl) => gl.Uniform1(location, (value is bool p) ? p ? 1 : 0 : 0)},
            {
    typeof(Matrix4x4),
                (location, value, gl) =>
                {
                    float* fixedData = (float*)&value; ;
                    gl.UniformMatrix4(location, 1, false, fixedData);
                }
            }
        };
    }
}
