namespace WorldMap.Heights.Map.Interfaces
{
    public interface IMap
    {
        bool IsRendering { get; }
        bool IsUpdaeting { get; }

        void Update();
        void Render();
    }
}
