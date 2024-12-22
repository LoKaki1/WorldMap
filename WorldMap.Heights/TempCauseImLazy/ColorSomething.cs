using System;
using System.Numerics;

namespace WorldMap.Heights.TempCauseImLazy;

public struct ColorSomething
{
    public Vector3 Color { get; set; }

    public ColorSomething(int x, int y, int z)
    {
        Color = new Vector3(x, y, z);
    }
}
