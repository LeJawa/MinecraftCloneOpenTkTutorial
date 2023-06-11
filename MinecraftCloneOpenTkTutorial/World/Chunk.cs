using MinecraftCloneOpenTkTutorial.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MinecraftCloneOpenTkTutorial.World
{
    internal class Chunk
    {
        private List<Vector3> _chunkVerts = new List<Vector3>();
        private List<Vector2> _chunkUVs = new List<Vector2>();
        private List<uint> _chunkIndices = new List<uint>();

        const int SIZE = 16;
        const int HEIGHT = 32;

        public Vector3 position;

        private uint indexCount;

        VAO chunkVAO;
        VBO chunkVertexVBO;
        VBO chunkUVVBO;
        IBO chunkIBO;

        Texture texture;

        public Chunk(Vector3 position)
        {
            this.position = position;

            GenBlocks();
            BuildChunk();
        }

        /// <summary>
        /// Generate the data
        /// </summary>
        public void GenChunk()
        {

        }

        /// <summary>
        /// Gererate the appropriate block gaces given the data
        /// </summary>
        public void GenBlocks()
        {
            for (int i = 0; i < 3; i++)
            {
                int faceCount = 0;

                Block block = new Block(new Vector3(i, 0, 0));
                FaceData faceData;

                if (i == 0)
                {
                    faceData = block.GetFace(Faces.LEFT);
                    _chunkVerts.AddRange(faceData.vertices);
                    _chunkUVs.AddRange(faceData.uv); 
                    faceCount++;
                }

                if (i == 2)
                {
                    faceData = block.GetFace(Faces.RIGHT);
                    _chunkVerts.AddRange(faceData.vertices);
                    _chunkUVs.AddRange(faceData.uv); 
                    faceCount++;
                }

                faceData = block.GetFace(Faces.FRONT);
                _chunkVerts.AddRange(faceData.vertices);
                _chunkUVs.AddRange(faceData.uv);

                faceData = block.GetFace(Faces.BACK);
                _chunkVerts.AddRange(faceData.vertices);
                _chunkUVs.AddRange(faceData.uv);

                faceData = block.GetFace(Faces.TOP);
                _chunkVerts.AddRange(faceData.vertices);
                _chunkUVs.AddRange(faceData.uv);

                faceData = block.GetFace(Faces.BOTTOM);
                _chunkVerts.AddRange(faceData.vertices);
                _chunkUVs.AddRange(faceData.uv);

                faceCount += 4;

                AddIndices(faceCount);
            }
        }

        public void AddIndices(int amountFaces)
        {
            for (int i = 0; i < amountFaces; i++)
            {
                _chunkIndices.Add(0 + indexCount);
                _chunkIndices.Add(1 + indexCount);
                _chunkIndices.Add(2 + indexCount);
                _chunkIndices.Add(2 + indexCount);
                _chunkIndices.Add(3 + indexCount);
                _chunkIndices.Add(0 + indexCount);

                indexCount += 4;
            }
        }

        /// <summary>
        /// Take data and process it for rendering
        /// </summary>
        public void BuildChunk()
        {
            chunkVAO = new VAO();
            chunkVAO.Bind();

            chunkVertexVBO = new VBO(_chunkVerts); 
            chunkVertexVBO.Bind();

            chunkVAO.LinkToVAO(0, 3, chunkVertexVBO);

            chunkUVVBO = new VBO(_chunkUVs);
            chunkUVVBO.Bind();

            chunkVAO.LinkToVAO(1, 2, chunkUVVBO);

            chunkIBO = new IBO(_chunkIndices);

            texture = new Texture("dirt_16.png");
        }

        /// <summary>
        /// Drawing the chunk
        /// </summary>
        /// <param name="program"></param>
        public void Render(ShaderProgram program)
        {
            program.Bind();
            chunkVAO.Bind();
            chunkIBO.Bind();
            texture.Bind();

            GL.DrawElements(PrimitiveType.Triangles, _chunkIndices.Count, DrawElementsType.UnsignedInt, 0);
        }

        public void Delete()
        {
            chunkVAO.Delete();
            chunkVertexVBO.Delete();
            chunkUVVBO.Delete();
            chunkIBO.Delete();
            texture.Delete();
        }
    }
}
