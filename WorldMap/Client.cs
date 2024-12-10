using System;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using WorldMap.Common.Camera.Interfaces;
using WorldMap.Common.OpenGL.Pipline;
using WorldMap.Heights.Map.Interfaces;

namespace WorldMap;

public unsafe class Client
{
    private readonly ICameraContoller m_CameraController;
    private readonly IKeyboard m_Keyboard;
    private readonly IMouse m_Mouse;
    private readonly IMap m_Map;
    private readonly IWindow m_Window;
    private readonly GL m_GlContext;
    private readonly Glfw m_GlfwContext;
    private int m_Width;
    private int m_Height;

    public Client(ICameraContoller cameraController,
                  IKeyboard keyboard,
                  IMouse mouse,
                  IMap map,
                  GL glContext,
                  IWindow window,
                  Glfw glfwContext)
    {
        m_CameraController = cameraController;
        m_Keyboard = keyboard;
        m_Mouse = mouse;
        m_Map = map;
        m_GlContext = glContext;
        m_Window = window;
        m_GlfwContext = glfwContext;
        // Callback when the window is created

        var currentWindow = (WindowHandle*)m_Window.Handle;
        m_GlfwContext.GetFramebufferSize(currentWindow, out int width, out int height);

        (m_Width, m_Height) = (width, height);


        m_Window.Render += (_) => Render();

        m_Window.Size = new(800, 600);
        m_Window.FramesPerSecond = 144;
        m_Window.UpdatesPerSecond = 144;
        m_Window.VSync = false;
        m_Window.FramebufferResize += OnFrameBufferResize;
        // m_Window.FocusChanged += SilkOnFocusChanged;
    }

    private void OnFrameBufferResize(Vector2D<int> d)
    {
        var currentWindow = (WindowHandle*)m_Window.Handle;
        m_GlfwContext.GetFramebufferSize(currentWindow, out int width, out int height);

        (m_Width, m_Height) = (width, height);
        m_GlContext.Viewport(0, 0, (uint)m_Width, (uint)m_Height);
    }
    private void Render()
    {
        m_CameraController.Update();
        m_Map.Update();
        PreRenderSetup();
        m_Map?.Render();
        PiplineGL.Run();

    }

    void PreRenderSetup()
    {
        // Prepare rendering
        m_GlContext.ClearColor(0, 0, 0, 0);
        m_GlContext.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
    }
}
