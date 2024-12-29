namespace WorldMap.Map.WorldContants
{
    public static class Constants
    {
        /// <summary>
        /// World size in tiles (the number of tiles foreach axis)
        /// </summary>
        public const int WORLD_SIZE = 4_194_304;
        /// <summary>
        /// World maximum zoom level
        /// </summary>
        public const int WORLD_ZOOM_LEVELS = 22;
        /// <summary>
        /// Chunk size
        /// </summary>
        public const int CHUNK_SIZE = 512;
        /// <summary>
        /// Vercidum stuff
        /// </summary>
        public const int VERTICES_PER_RUN = CHUNK_SIZE * 2 + 4;
        public const int VERTICES_PER_CHUNK = VERTICES_PER_RUN * CHUNK_SIZE;
        public const int VERTICES_PER_RUN_NOT_DEGENERATE = VERTICES_PER_RUN - 3;
    }
}
