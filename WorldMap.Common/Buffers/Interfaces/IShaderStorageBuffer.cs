using WorldMap.Common.Models.Enums;

namespace WorldMap.Common.Buffers.Interfaces
{
    /// <summary>
    /// Should store all the relevant data for the shader storage buffer
    /// </summary>
    public  unsafe interface IShaderStorageBuffer<T>
    {
        /// <summary>
        /// Stores the handle for the ssbo
        /// </summary>
        uint SSBO { get; }
        /// <summary>
        /// Because mac nowadays won't allow us to use the ssbo we must use it as
        /// a uniform buffer object (works the same with a lot more boiler plate)
        /// </summary>
        SSBOTypes Type { get; }

        /// <summary>
        /// Buffer the ssbo to the shader (No need to pass the data size because I can just use the sizeof the ssbotype (if no array stored in it))
        /// </summary>
        /// <param name="data">The ssbo data (can be anything just make sure you describe its interface in the shader code)</param>
        /// <param name="action">Most of the times just a delegat to clean the data when it finishes</param>
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
        void BufferObject(T* data, Action? action = null) ;
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
    }
}
