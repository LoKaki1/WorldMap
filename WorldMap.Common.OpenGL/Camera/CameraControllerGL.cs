using Silk.NET.Input;
using Silk.NET.Windowing;
using System.Numerics;
using WorldMap.Common.Camera;
using WorldMap.Common.Camera.Helpers;

namespace WorldMap.Common.OpenGL.Camera
{
    public sealed class CameraControllerGL : CameraControllerBase
    {
        private readonly IKeyboard m_Keyboard;
        private readonly IMouse m_Mouse;
        private readonly IWindow m_Window;
        
        private bool m_CaptureMouse;
        
        private Vector2 m_LastMouse;

        public override float Aspect => m_Window.Size.X / (float)m_Window.Size.Y;

        public CameraControllerGL(IKeyboard keyboard, IMouse Mouse, IWindow Window)
        {
            m_Keyboard = keyboard;
            m_Mouse = Mouse;
            m_Window = Window;

            m_CaptureMouse = true;
            m_LastMouse = m_Mouse.Position;

            m_Keyboard.KeyDown += OnKeyDown;
        }
        private void OnKeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Escape)
            {
                m_CaptureMouse = !m_CaptureMouse;

                // Don't snip the camera when capturing the mouse
                m_LastMouse = m_Mouse.Position;
            }
        }
        public override void Update()
        {
            // Update camera roatation
            if (m_CaptureMouse)
            {
                var diff = m_LastMouse - m_Mouse.Position;

                Yaw -= diff.X * 0.003f;
                Pitch += diff.Y * 0.003f;

                m_Mouse.Position = new Vector2(m_Window.Size.X / 2, m_Window.Size.Y / 2);
                m_LastMouse = m_Mouse.Position;
                m_Mouse.Cursor.CursorMode = CursorMode.Hidden;
            }
            else
            {
                m_Mouse.Cursor.CursorMode = CursorMode.Normal;
            }

            // Update camera position
            // Fly camera movement
            float movementSpeed = 0.15f;

            if (m_Keyboard.IsKeyPressed(Key.ShiftLeft))
            {
                movementSpeed *= 16;
            }
            if (m_Keyboard.IsKeyPressed(Key.W))
                Position += CameraHelper.FromPitchYaw(Pitch, Yaw) * movementSpeed;
            else if (m_Keyboard.IsKeyPressed(Key.S))
                Position -= CameraHelper.FromPitchYaw(Pitch, Yaw) * movementSpeed;

            if (m_Keyboard.IsKeyPressed(Key.A))
                Position += CameraHelper.FromPitchYaw(0, Yaw - MathF.PI / 2) * movementSpeed;
            else if (m_Keyboard.IsKeyPressed(Key.D))
                Position += CameraHelper.FromPitchYaw(0, Yaw + MathF.PI / 2) * movementSpeed;

            if (m_Keyboard.IsKeyPressed(Key.E))
                Position += CameraHelper.FromPitchYaw(MathF.PI / 2, 0) * movementSpeed;
            else if (m_Keyboard.IsKeyPressed(Key.Q))
                Position += CameraHelper.FromPitchYaw(-MathF.PI / 2, 0) * movementSpeed;
        }
    }
}
