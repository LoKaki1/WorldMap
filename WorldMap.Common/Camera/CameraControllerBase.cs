using System.Numerics;
using WorldMap.Common.Camera.Helpers;
using WorldMap.Common.Camera.Interfaces;

namespace WorldMap.Common.Camera;

public abstract class CameraControllerBase : ICameraContoller
{
    public Vector3 Position { get; set; } = new Vector3(0.5f, 0.1f, 0.5f);
    public float Pitch { get; set; }
    public float Yaw { get; set; }
    public float FieldOfView { get; set; } = 50.0f / 180.0f * MathF.PI;
    public abstract float Aspect { get; }
    public float NearPlane { get; set; } = 0.25f;
    public float FarPlane { get; set; } = 50;

    public Matrix4x4 GetViewProjection()
    {
        var view = CameraHelper.CreateFPSView(Position, Pitch, Yaw);
        var proj = Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView, Aspect, NearPlane, FarPlane);

        return view * proj;
    }
    public abstract void Update();
}
