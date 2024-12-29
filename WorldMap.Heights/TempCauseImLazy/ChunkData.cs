using System;
using System.Numerics;

namespace WorldMap.Heights.TempCauseImLazy;

public struct ChunkData
{
    public Vector3 Color { get; set; }

    public ChunkData(int x, int y, int z)
    {
        Color = new Vector3(x, y, z);
    }
}
