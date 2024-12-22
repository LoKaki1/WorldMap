using WorldMap.Common.Models.Enums;
using WorldMap.Common.Shaders.Interfaces;

namespace WorldMap.Common.Models.Buffers
{
    public sealed unsafe class ShaderStorageBufferParameters<T> where T : unmanaged
    {
        /// <summary>
        /// Tell which type of storage buffer to use (Relevant when changing opertating systems)
        /// </summary>
        public SSBOTypes SSBOTypes { get; set; }
        /// <summary>
        /// When we use the uniform buffer object we get the binding buffer to the shader buffer (relevant for mac only)
        /// </summary>
        public IShaderProgram ShaderProgram { get; set; }
        /// <summary>
        /// When we use the uniform buffer object we should name it (relevant for mac only)
        /// </summary>
        public string SSBOName { get; set; }
        public T* Data { get; }
        public int Size { get; set; }
        public uint BindingPoint { get; set; }

        public ShaderStorageBufferParameters(SSBOTypes sSBOTypes,
                                             IShaderProgram shaderProgram,
                                             T* data,
                                             string sSBOName,
                                             uint bindingPoint = 0)
        {
            SSBOTypes = sSBOTypes;
            ShaderProgram = shaderProgram;
            SSBOName = sSBOName;
            Data = data;
            Size = sizeof(T);
            BindingPoint = bindingPoint;
        }
    }
}
