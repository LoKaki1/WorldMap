namespace WorldMap.Heights.Map.Interfaces
{
    public interface IMap
    {
        bool IsRendering { get; }
        bool IsUpdating { get; }

        void Update();
        void Render();
    }
}
