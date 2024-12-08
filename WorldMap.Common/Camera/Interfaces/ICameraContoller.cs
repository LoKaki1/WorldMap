using System.Numerics;

namespace WorldMap.Common.Camera.Interfaces;

public interface ICameraContoller
{
    /// <summary>
    /// The horizontal angle
    /// </summary>
    float Pitch { get; set; }
    /// <summary>
    /// The Vertical angle
    /// </summary>
    float Yaw { get; set; }
    /// <summary>
    /// Camera position
    /// </summary>
    Vector3 Position { get; set; }

    float FieldOfView { get; set; }
    float Aspect { get; set; }
    float NearPlane { get; set; }
    float FarPlane { get; set; }

    /// <summary>
    /// Get the current view matrix (the matrix you multiply each object to get its releative position )
    /// </summary>
    /// <returns>Matrix which the describe the camera view projection</returns>
    Matrix4x4 GetViewProjection();
    /// <summary>
    /// Update the camera state
    /// </summary>
    void Update();
}
