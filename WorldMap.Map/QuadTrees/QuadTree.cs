using System.Numerics;
using System.Runtime.InteropServices;
using WorldMap.Map.Vertices;
using WorldMap.Map.WorldContants;

namespace WorldMap.Map.QuadTrees
{

    public class QuadTree
    {
        public QuadTreeNode Root { get; private set; }
        private readonly QuadPool m_Pool;
        private readonly int m_MaxDepth;
        private readonly int m_MaxQuadsInMemory;

        public QuadTree(int size, int maxDepth, int maxQuadsInMemory)
        {
            m_MaxDepth = maxDepth;
            m_MaxQuadsInMemory = maxQuadsInMemory;
            m_Pool = new QuadPool(maxQuadsInMemory);
            Root = m_Pool.GetNode(new QuadBounds(0, 0, size));
        }

        public void Update(Vector3 cameraPosition, float lodBaseDistance)
        {
            Root.Update(cameraPosition, lodBaseDistance, m_MaxDepth, m_Pool);
        }

        public void Render()
        {
            Root.Render();
        }
    }

    public class QuadTreeNode
    {
        private readonly QuadBounds m_Bounds;
        public QuadTreeNode[] Children { get; private set; }
        public bool HasChildren { get; private set; }
        private readonly QuadData m_Data;

        public QuadBounds Bounds => m_Bounds;

        public QuadTreeNode(QuadBounds bounds)
        {
            m_Bounds = bounds;
            m_Data = new QuadData(bounds);
            Children = null;
            HasChildren = false;
        }
        public void Reset()
        {
            Children = null;
            HasChildren = false;
            m_Data.Reset(); // Reset the data to minimize state retention
        }

        public void FreeData()
        {
            m_Data.FreeMemory(); // Explicitly free memory-intensive resources
        }

        public void Update(Vector3 cameraPosition, float lodBaseDistance, int maxDepth, QuadPool pool)
        {
            float distanceToCamera = m_Bounds.GetDistanceTo(cameraPosition);

            if (distanceToCamera < lodBaseDistance && m_Bounds.Level < maxDepth)
            {
                if (!HasChildren)
                    Subdivide(pool);

                foreach (var child in Children)
                    child.Update(cameraPosition, lodBaseDistance / 2, maxDepth, pool);
            }
            else if (HasChildren)
            {
                Collapse(pool);
            }
        }
        public int GetNumberOfChildren()
        {
            if (!HasChildren || Children == null)
            {
                return 1;
            }

            int size = 1;
            
            foreach (var child in Children)
            {
                size += child.GetNumberOfChildren();    
            }

            return size;
        }
        private void GetQuadsShaderIndirectUniform(QuadsShaderIndirectUniformStorage a, int count)
        {
            if (!HasChildren || Children == null)
            {
                a.TriangleSize[count] = (uint)Bounds.TrinagleSize;
                a.QuadPostion[count] = new Vector2(Bounds.X, Bounds.Y);

                return;
            }
            
            foreach(var child in Children)
            {
                count++;
                child.GetQuadsShaderIndirectUniform(a, count);
            }

            count++;
            a.TriangleSize[count] = (uint)Bounds.TrinagleSize;
            a.QuadPostion[count] = new Vector2(Bounds.X, Bounds.Y);
        }
        public QuadsShaderIndirectUniformStorage GetQuadsShaderIndirectUniform()
        {
            var count = GetNumberOfChildren();
            var a = new QuadsShaderIndirectUniformStorage(new Vector2[count], new uint[count]);

            GetQuadsShaderIndirectUniform(a, count);

            return a;
        }
        public void Render()
        {
            if (HasChildren)
            {
                foreach (var child in Children)
                    child.Render();
            }
            else
            {
                m_Data.Render();
            }
        }

        private void Subdivide(QuadPool pool)
        {
            if (HasChildren) return;

            Children = new QuadTreeNode[4];
            int halfSize = m_Bounds.Size / 2;

            Children[0] = pool.GetNode(new QuadBounds(m_Bounds.X, m_Bounds.Y, halfSize, m_Bounds.Level + 1));
            Children[1] = pool.GetNode(new QuadBounds(m_Bounds.X + halfSize, m_Bounds.Y, halfSize, m_Bounds.Level + 1));
            Children[2] = pool.GetNode(new QuadBounds(m_Bounds.X, m_Bounds.Y + halfSize, halfSize, m_Bounds.Level + 1));
            Children[3] = pool.GetNode(new QuadBounds(m_Bounds.X + halfSize, m_Bounds.Y + halfSize, halfSize, m_Bounds.Level + 1));

            HasChildren = true;
        }

        private void Collapse(QuadPool pool)
        {
            if (!HasChildren) return;

            foreach (var child in Children)
                pool.ReturnNode(child);

            Children = null;
            HasChildren = false;
        }
    }

    public struct QuadBounds
    {
        public float X { get; }
        public float Y { get; }
        public int Size { get; }
        public int Level { get; }
        public int TrinagleSize { get; }


        public QuadBounds(float x, float y, int size, int level = 0)
        {
            X = x;
            Y = y;
            Size = size;
            Level = level;
            TrinagleSize = Constants.CHUNK_SIZE / (Constants.WORLD_ZOOM_LEVELS - Level);
        }

        public float GetDistanceTo(Vector3 point)
        {
            float centerX = X + Size / 2;
            float centerY = Y + Size / 2;
            return MathF.Sqrt((point.X - centerX) * (point.X - centerX) + (point.Y - centerY) * (point.Y - centerY) + point.Z * point.Z);
        }
    }

    public unsafe class QuadData
    {
        private readonly QuadBounds m_Bounds;
        private int m_Size;
        private MapVertex* m_Offset;
        //private float[,] m_FloatData; // 512x512 floats
        //private byte[,] m_PixelData; // 512x512 pixels

        public QuadBounds Bounds => m_Bounds;

        public QuadData(QuadBounds bounds)
        {
            m_Bounds = bounds;
            m_Size = Marshal.SizeOf<MapVertex>() * Constants.VERTICES_PER_CHUNK;
            LoadData();
        }

        public void Reset()
        {
            // Optionally clear temporary references but keep arrays if reusing
        }

        public void FreeMemory()
        {
            // Explicitly free large arrays to save memory
            //m_FloatData = null;
            //m_PixelData = null;
            // Allocator.Free(ref m_Offset, ref m_Size);
            //GC.Collect(); // Force garbage collection to reclaim memory
        }

        private void LoadData()
        {
            //m_Offset = Allocator.Alloc<MapVertex>(m_Size);

            // here we should add all the loading heights pixels normals etc'
        }

        public void Render()
        {
            // Render logic here
            Console.WriteLine($"Rendering data for bounds: {m_Bounds.X}, {m_Bounds.Y}, {m_Bounds.Size}");
        }
    }

    public class QuadPool
    {
        private readonly Stack<QuadTreeNode> m_Pool;
        private readonly int m_MaxCapacity;
        private int m_ActiveCount;

        public QuadPool(int capacity)
        {
            m_Pool = new Stack<QuadTreeNode>(capacity);
            m_MaxCapacity = capacity;
            m_ActiveCount = 0;
        }

        /// <summary>
        /// Gets a node from the pool or creates a new one if the pool is empty.
        /// </summary>
        /// <param name="bounds">The bounds for the quad node.</param>
        /// <returns>A QuadTreeNode instance.</returns>
        public QuadTreeNode GetNode(QuadBounds bounds)
        {
            QuadTreeNode node;

            if (m_Pool.Count > 0)
            {
                node = m_Pool.Pop();
            }
            else
            {
                node = new QuadTreeNode(bounds);
            }

            m_ActiveCount++;
            return node;
        }

        /// <summary>
        /// Returns a node to the pool for reuse, freeing large data arrays to save memory.
        /// </summary>
        /// <param name="node">The QuadTreeNode to return.</param>
        public void ReturnNode(QuadTreeNode node)
        {
            if (m_Pool.Count < m_MaxCapacity)
            {
                node.Reset(); // Reset the node before pooling it
                m_Pool.Push(node);
            }
            else
            {
                node.FreeData(); // Free memory-intensive data if the pool is full
            }

            m_ActiveCount = Math.Max(m_ActiveCount - 1, 0);
        }

        /// <summary>
        /// Gets the number of active nodes currently in use.
        /// </summary>
        public int ActiveCount => m_ActiveCount;

        /// <summary>
        /// Gets the number of available nodes in the pool.
        /// </summary>
        public int AvailableCount => m_Pool.Count;
    }

}
