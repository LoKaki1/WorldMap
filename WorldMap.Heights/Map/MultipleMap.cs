using WorldMap.Common.Math;
using WorldMap.Heights.Map.Interfaces;

namespace WorldMap.Heights.Map
{
    /// <summary>
    /// Contains a multiple chunks this class should work with the chunkmap 
    /// and should use the uniform buffer for buffering the camera position for the tessellation
    /// </summary>
    public sealed class MultipleMap : IMap
    {
        private const int LEVELS = 2;
        private readonly List<IMap> m_Maps;
        public bool IsRendering { get; private set; }
        public bool IsUpdating { get; private set; }

        public MultipleMap()
        {
            m_Maps = new List<IMap>();

            var numberOfChunks = (int) Enumerable.Range(1, LEVELS).Sum(_ => 8 * _) + 1;

            for (int i = 0; i < numberOfChunks; i++)
            {
                //var map = new StaticMap()
            }
        }


        public void Render() => throw new NotImplementedException();
        public void Update() => throw new NotImplementedException();
    }
}
