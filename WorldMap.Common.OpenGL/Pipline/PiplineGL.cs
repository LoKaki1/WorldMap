using System.Collections.Concurrent;

namespace WorldMap.Common.OpenGL.Pipline
{
    public static class PiplineGL
    {
        private static ConcurrentQueue<Action> Pipline = new ConcurrentQueue<Action>();

        public static void EnqueToPipline(Action action)
        {
            Pipline.Enqueue(action);
        }

        public static void Run()
        {
            if (Pipline.IsEmpty || !Pipline.TryDequeue(out Action? action)) return;

            action?.Invoke();
        }

    }
}
