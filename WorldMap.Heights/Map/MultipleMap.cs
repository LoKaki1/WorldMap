using WorldMap.Common.Math;
using WorldMap.Heights.Map.Interfaces;

namespace WorldMap.Heights.Map
{
    /// <summary>
    // Let's for start do it as stupid as possible without tesslation 
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
