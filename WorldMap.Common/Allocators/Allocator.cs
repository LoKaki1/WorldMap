using System;
using System.Runtime.InteropServices;

namespace WorldMap.Common.Allocators;

public static unsafe class Allocator
{
#pragma warning disable CS8500
    public static T* Alloc<T>(int byteCount) => (T*)NativeMemory.Alloc((nuint)byteCount);
#pragma warning restore

    public static void* AllocZeroed(int byteCount) => NativeMemory.AllocZeroed((nuint)byteCount);

    public static void Free<T>(ref T* data, ref int bytes) where T : unmanaged
    {
        // Free if not already freed
        if (data != null)
        {
            bytes = 0;
            NativeMemory.Free(data);
        }

        data = null;
    }

    public static void Free<T>(ref T* data) where T : unmanaged
    {
        if (data != null)
            NativeMemory.Free(data);

        data = null;
    }
}
