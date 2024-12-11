// See https://aka.ms/new-console-template for more information
// Now I'm doing exactly like I did in FinanceMaker which as of now (3.12.2024) 
// I'm loosing a lot of money 😃
// Sooooooo its working amazingggg
// Ok for now project 
//
// We makr the world map in c#, we want to use the to open street map api for the raster
// and ask chat gpt for the heights..
//
// But we know all this shit... (lets do this)
// Got this urls -
// 
// Raster - https://tile.nextzen.org/tilezen/terrain/v1/512/terrarium/6/19/21.png?api_key=ZVKjQi5pS1-afO-ziytQ8g&
// Vector - https://tile.nextzen.org/tilezen/vector/v1/512/all/6/19/21.mvt?api_key=ZVKjQi5pS1-afO-ziytQ8g&
// Heights Maybe? - https://tile.nextzen.org/tilezen/terrain/v1/512/normal/6/19/21.png?api_key=ZVKjQi5pS1-afO-ziytQ8g&
// 
// Now with all those, we will use the bruTile as a requests service 
// Also to transform between mercator and opengl coordinates-system we will use the netopologysuite
// So foreach tile the render pipline should look like this 
// 
// Camera (MVP, the field of view gets a big role here)
//   |
//   V  
// NetopologySuite (Transformin the camera coordinates to the mercator projection)
//  |
//  V
// BruhTile (HttpTileService) (of course here will come some predicition for the next tiles)
//  | 
//  V
// OpenGL (I know, I know), 
// For regular raster map (both the vector and the raster because the are both raster)
// Vertex Shader (just by the tile)
//  |
//  V
// Fragmenet Shader (to put the texture in ) consider reading about mipmap, maybe we can override the default mipmap
// (this was simple but for both the vectors and the height map it is gonna be a bit more complecated)
// lets start with geometries
// Vertex Shader (for the basic geometry we get) -> Tesselation Shader if we want to add smother
//                                                              |
//                                                              V
//                                                 Geometry Shader (If its a circle or other perfect shape)
//                                                              |
//                                                              V
//                                                 Fragment Shader (consider the style panel)
//
// And for the heights it looks the same just without the geometry. (but maybe we will need the height to be in the start)
// 
// And what we will need to create in this project
//
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using WorldMap;
using WorldMap.Common.Camera.Interfaces;
using WorldMap.Common.Factories.Interfaces;
using WorldMap.Common.OpenGL.Camera;
using WorldMap.Common.OpenGL.Factories;
using WorldMap.Heights.Map;
using WorldMap.Heights.Map.Interfaces;

var builder = Host.CreateApplicationBuilder();
var services = builder.Services;
// We can't just use the the client code as we always do, what we will do is creating the window 
// and then getting opengl api and then we will start to run the our engine
// Create a Silk.NET window
var options = WindowOptions.Default;
options.API = new GraphicsAPI(ContextAPI.OpenGL, new APIVersion(3, 3));
options.Position = new(200, 200);
options.PreferredDepthBufferBits = 32;
options.Title = "gl_VertexID";

var window = Window.Create(options);

var glfw = GlfwProvider.GLFW.Value;

if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    glfw.WindowHint(WindowHintInt.ContextVersionMajor, 4); // Change version if needed
    glfw.WindowHint(WindowHintInt.ContextVersionMinor, 6); // or 4.5, depending on your machine
    glfw.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
    glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
}
window.Load += () =>
{
    GL gl = window.CreateOpenGL();
    services.AddSingleton(gl);
    services.AddSingleton(window);
    services.AddSingleton(glfw);
    var inputContext = window.CreateInput();
    var keyboard = inputContext.Keyboards[0];
    var mouse = inputContext.Mice[0];
    services.AddSingleton(keyboard);
    services.AddSingleton(mouse);
    services.AddSingleton<ICameraContoller, CameraControllerGL>();
    services.AddSingleton<IBufferFactory, OpenGLFactory>();
    services.AddSingleton<IShaderFactory, OpenGLFactory>();
    services.AddSingleton<IMap, StaticMap>();
    services.AddSingleton<Client>();
    var provider = services.BuildServiceProvider();
    var client = provider.GetRequiredService<Client>();
    window.Run();
};
window.Size = new(800, 600);
var ticks = 17_000;
window.FramesPerSecond = ticks;
window.UpdatesPerSecond = ticks;
window.VSync = false;
window.Initialize();
builder.Build();




// using Silk.NET.OpenGL;
// using Silk.NET.Windowing;
// using Silk.NET.Input;
// using System;
// using System.Numerics;
// using Silk.NET.GLFW;
// using System.Runtime.InteropServices;

// class HexagonRender
// {
//     private static IWindow window;
//     private static GL gl;
//     private static uint vao, vbo;
//     private static uint shaderProgram;
//     public static Glfw GLFW { get; set; }

//     static void Main(string[] args)
//     {
//         // Create a window using Silk.NET
//         var options = WindowOptions.Default;
//         options.Size = new Silk.NET.Maths.Vector2D<int>(800, 600);
//         options.Title = "Hexagon Renderer with Tessellation";
//         window = Window.Create(options);

//         GLFW = GlfwProvider.GLFW.Value;
//         if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
//         {
//             GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4); // Change version if needed
//             GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 6); // or 4.5, depending on your machine
//             GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
//             GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
//         }
//         window.Load += OnLoad;
//         window.Render += OnRender;
//         window.Closing += OnClose;

//         window.Run();
//     }

//     private unsafe static void OnLoad()
//     {
//         gl = GL.GetApi(window);
//         var code = gl.GetString(StringName.Version);

//         Console.WriteLine();
//         // Define a single vertex for the tessellation pipeline
//         float[] vertices = { 0.0f, 0.0f, 0.0f }; // Center point

//         // Generate and bind VAO and VBO
//         vao = gl.GenVertexArray();
//         vbo = gl.GenBuffer();

//         gl.BindVertexArray(vao);
//         gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
//         gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), (float*)&vertices, BufferUsageARB.StaticDraw);

//         // Set vertex attribute pointers
//         gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*)0);
//         gl.EnableVertexAttribArray(0);

//         // Create shader program
//         shaderProgram = CreateShaderProgram();
//     }

//     private static void OnRender(double delta)
//     {
//         // Clear the screen
//         gl.Clear((uint)ClearBufferMask.ColorBufferBit);
//         gl.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);

//         // Use the shader program
//         gl.UseProgram(shaderProgram);

//         // Bind VAO and draw
//         gl.BindVertexArray(vao);
//         gl.PatchParameter(GLEnum.PatchVertices, 1); // Set patch vertices for tessellation
//         gl.DrawArrays(PrimitiveType.Patches, 0, 1);

//         // Swap buffers
//         window.SwapBuffers();
//     }

//     private static void OnClose()
//     {
//         // Cleanup OpenGL resources
//         gl.DeleteBuffer(vbo);
//         gl.DeleteVertexArray(vao);
//         gl.DeleteProgram(shaderProgram);
//     }

//     private static uint CreateShaderProgram()
//     {
//         // Vertex Shader
//         string vertexShaderSource = @"
//         #version 410 core
//         layout(location = 0) in vec3 position;
//         void main()
//         {
//             gl_Position = vec4(position, 1.0);
//         }";

//         // Tessellation Control Shader
//         string tessControlShaderSource = @"
//         #version 410 core
//         layout(vertices = 6) out;
//         void main()
//         {
//             if (gl_InvocationID == 0)
//             {
//                 gl_TessLevelInner[0] = 1.0;
//                 gl_TessLevelOuter[0] = 6.0;
//                 gl_TessLevelOuter[1] = 6.0;
//                 gl_TessLevelOuter[2] = 6.0;
//             }
//             gl_out[gl_InvocationID].gl_Position = vec4(
//                 cos(gl_InvocationID * 2.0 * 3.141592 / 6.0),
//                 sin(gl_InvocationID * 2.0 * 3.141592 / 6.0),
//                 0.0, 1.0);
//         }";

//         // Tessellation Evaluation Shader
//         string tessEvalShaderSource = @"
//         #version 410 core
//         layout(triangles, equal_spacing, cw) in;
//         void main()
//         {
//             gl_Position = (gl_TessCoord.x * gl_in[0].gl_Position +
//                            gl_TessCoord.y * gl_in[1].gl_Position +
//                            gl_TessCoord.z * gl_in[2].gl_Position);
//         }";

//         // Geometry Shader
//         string geometryShaderSource = @"
//         #version 410 core
//         layout(triangles) in;
//         layout(triangle_strip, max_vertices = 6) out;
//         void main()
//         {
//             for (int i = 0; i < gl_in.length(); i++)
//             {
//                 gl_Position = gl_in[i].gl_Position;
//                 EmitVertex();
//             }
//             EndPrimitive();
//         }";

//         // Fragment Shader
//         string fragmentShaderSource = @"
//         #version 410 core
//         out vec4 FragColor;
//         void main()
//         {
//             FragColor = vec4(1.0, 1.0, 1.0, 1.0);
//         }";

//         // Compile shaders
//         uint vertexShader = CompileShader(ShaderType.VertexShader, vertexShaderSource);
//         uint tessControlShader = CompileShader(ShaderType.TessControlShader, tessControlShaderSource);
//         uint tessEvalShader = CompileShader(ShaderType.TessEvaluationShader, tessEvalShaderSource);
//         uint geometryShader = CompileShader(ShaderType.GeometryShader, geometryShaderSource);
//         uint fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);

//         // Link shaders into a program
//         uint program = gl.CreateProgram();
//         gl.AttachShader(program, vertexShader);
//         gl.AttachShader(program, tessControlShader);
//         gl.AttachShader(program, tessEvalShader);
//         gl.AttachShader(program, geometryShader);
//         gl.AttachShader(program, fragmentShader);
//         gl.LinkProgram(program);

//         // Check for linking errors
//         gl.GetProgram(program, GLEnum.LinkStatus, out var success);
//         if (success == 0)
//         {
//             Console.WriteLine($"Program Linking Error: {gl.GetProgramInfoLog(program)}");
//         }

//         // Cleanup shaders
//         gl.DeleteShader(vertexShader);
//         gl.DeleteShader(tessControlShader);
//         gl.DeleteShader(tessEvalShader);
//         gl.DeleteShader(geometryShader);
//         gl.DeleteShader(fragmentShader);

//         return program;
//     }

//     private static uint CompileShader(ShaderType type, string source)
//     {
//         uint shader = gl.CreateShader(type);
//         gl.ShaderSource(shader, source);
//         gl.CompileShader(shader);

//         // Check for compilation errors
//         gl.GetShader(shader, ShaderParameterName.CompileStatus, out var success);
//         if (success == 0)
//         {
//             Console.WriteLine($"{type} Compilation Error: {gl.GetShaderInfoLog(shader)}");
//         }

//         return shader;
//     }
// }

