using Silk.NET.OpenGL;
using System;
using WorldMap.Common.Shaders.Abstracts;
using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.OpenGL.Shaders
{
    public class ShaderProgramGL : ShaderProgramBase
    {
        /// <summary>
        /// In opengl only one program can execute at a time, therefore we must set the current handle to the program handle when excutes
        /// </summary>
        public static uint CurrentHandle { get; private set; }
        public GL Gl { get; set; }
        public ShaderProgramGL(GL gl, ShaderGL[] shaders) : base(shaders)
        {
            Gl = gl;
            var isValid = Initialize(out var data);
            Console.WriteLine(data);
            if (!isValid)
            {
                throw new Exception("AAAA");
            }
        }

        public override bool Initialize(out string[] errors)
        {
            // Create shader program.
            ProgramHandle = Gl.CreateProgram();

            // Attach shaders to the program
            foreach (var shader in Shaders)
            {
                var isValid = shader.Compile(out string s);

                if (!isValid)
                {
                    throw new Exception("AAAA");
                }

                Gl.AttachShader(ProgramHandle, shader.ShaderHandle);
            }

            Gl.LinkProgram(ProgramHandle);


            // Get the log
            Gl.GetProgram(ProgramHandle, ProgramPropertyARB.InfoLogLength, out int logLen);
            List<string> error = [];

            if (logLen > 0)
            {
                Gl.GetProgramInfoLog(ProgramHandle, (uint)logLen, out _, out string log);
                error.Add(log);
            }
            // Clean up
            // Attach shaders to the program
            foreach (var shader in Shaders)
            {
                Gl.DetachShader(ProgramHandle, shader.ShaderHandle);
            }
            errors = [.. error];

            // Ensure the shaders were linked correctly
            Gl.GetProgram(ProgramHandle, ProgramPropertyARB.LinkStatus, out int status);
            if (status == 0)
            {
                Console.WriteLine($"Shader link failed. Status: {status}");
                return false;
            }

            Unbind();

            return true;
        }
        public void Unbind()
        {
            Gl.UseProgram(0);
            CurrentHandle = 0;
        }
        public override void UseProgram()
        {
            // Already current, all good
            if (IsActive || CurrentHandle == ProgramHandle)
                return;

            Gl.UseProgram(ProgramHandle);
            CurrentHandle = ProgramHandle;
        }
        public override int GetUniformLocation(string uniform) => Gl.GetUniformLocation(ProgramHandle, uniform);
        public override void Dispose()
        {
            base.Dispose();
            if (ProgramHandle != 0)
            {
                Gl.DeleteProgram(ProgramHandle);
                ProgramHandle = 0;
            }
        }
    }
}
