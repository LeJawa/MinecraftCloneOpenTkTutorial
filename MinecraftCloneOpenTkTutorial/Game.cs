using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace MinecraftCloneOpenTkTutorial
{
    internal class Game : GameWindow
    {
        List<Vector3> _vertices = new List<Vector3>()
        {
            // front face
            new Vector3(-0.5f, 0.5f, 0.5f), // topleft vert
            new Vector3(0.5f, 0.5f, 0.5f), // topright vert
            new Vector3(0.5f, -0.5f, 0.5f), // bottomright vert
            new Vector3(-0.5f, -0.5f, 0.5f), // bottomleft vert
            // right face
            new Vector3(0.5f, 0.5f, 0.5f), // topleft vert
            new Vector3(0.5f, 0.5f, -0.5f), // topright vert
            new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
            new Vector3(0.5f, -0.5f, 0.5f), // bottomleft vert
            // back face
            new Vector3(0.5f, 0.5f, -0.5f), // topleft vert
            new Vector3(-0.5f, 0.5f, -0.5f), // topright vert
            new Vector3(-0.5f, -0.5f, -0.5f), // bottomright vert
            new Vector3(0.5f, -0.5f, -0.5f), // bottomleft vert
            // left face
            new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
            new Vector3(-0.5f, 0.5f, 0.5f), // topright vert
            new Vector3(-0.5f, -0.5f, 0.5f), // bottomright vert
            new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert
            // top face
            new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
            new Vector3(0.5f, 0.5f, -0.5f), // topright vert
            new Vector3(0.5f, 0.5f, 0.5f), // bottomright vert
            new Vector3(-0.5f, 0.5f, 0.5f), // bottomleft vert
            // bottom face
            new Vector3(-0.5f, -0.5f, 0.5f), // topleft vert
            new Vector3(0.5f, -0.5f, 0.5f), // topright vert
            new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
            new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert
        };

        private List<Vector2> _texCoords = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        private uint[] _indices =
        {
            // first face
            // top triangle
            0, 1, 2,
            // bottom triangle
            2, 3, 0,

            4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20,
        };

        // Render Pipeline vars
        private int _vao;
        private int _shaderProgram;
        private int _vbo;
        private int _texVbo;
        private int _ebo;
        private int _textureId;

        // Camera
        Camera _camera;

        // Transformation variables
        private float _yRot = 0f;


        // width and height of screen
        private int _width;
        private int _height;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            _width = width;
            _height = height;

            // Center the window on monitor
            CenterWindow(new Vector2i(_width, _height));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);

            _width = e.Width;
            _height = e.Height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            /// STEPS
            ///     1. create VAO
            ///     2. bind VAO
            ///     3. create vertex VBO with data
            ///     4. point slot 0 of VAO to VBO
            ///     5. unbind vertex VBO
            ///     6. create texture VBO with data
            ///     7. point slot of VAO to VBO
            ///     8. unbind texture VBO
            ///     9. unbind VAO
            ///


            // generate the vao
            _vao = GL.GenVertexArray();
            // bind the vao
            GL.BindVertexArray(_vao);

            // --- Vertices VBO ---

            // generate a buffer
            _vbo = GL.GenBuffer();
            // bind that buffer as an array buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            // store data in the vbo
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes, _vertices.ToArray(), BufferUsageHint.StaticDraw);

            // point slot (0) of the VAO to the currently bound VBO
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            // enable the slot
            GL.EnableVertexArrayAttrib(_vao, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // --- Texture VBO ---

            _texVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _texVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _texCoords.Count * Vector2.SizeInBytes, _texCoords.ToArray(), BufferUsageHint.StaticDraw);

            // point slot (1) of the VAO to the currently bound VBO
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            // enable the slot
            GL.EnableVertexArrayAttrib(_vao, 1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // unbind the vao
            GL.BindVertexArray(0);

            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


            // create the shader program
            _shaderProgram = GL.CreateProgram();

            // create the vertex shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            // add the source code from " Default.vert" in the shaders file
            GL.ShaderSource(vertexShader, LoadShaderSourc("Default.vert"));
            // compile the shader
            GL.CompileShader(vertexShader);

            // same as vertex shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSourc("Default.frag"));
            GL.CompileShader(fragmentShader);

            // attach the shaders to the shader program
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);

            // link the program to OpenGL
            GL.LinkProgram(_shaderProgram);

            // delete the shaders
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // --- TEXTURES ---
            _textureId = GL.GenTexture();

            // activate the texture in the unit
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _textureId);

            // texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            // load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult dirtTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/dirt_16.png"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dirtTexture.Width, dirtTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dirtTexture.Data);
            // unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(_width, _height, Vector3.Zero);
            CursorState = CursorState.Grabbed; 
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            // Delete VAO, VBO, EBO and shader program
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            GL.DeleteTexture(_textureId);
            GL.DeleteProgram(_shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // set the color to fill the screen with
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            // fill the screen with the color
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // draw our triangle
            GL.UseProgram(_shaderProgram);

            GL.BindTexture(TextureTarget.Texture2D, _textureId);

            // bind vao
            GL.BindVertexArray(_vao);
            // bind ebo
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);


            // transformation matrices
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = _camera.getViewMatrix();
            Matrix4 projection = _camera.getProjectionMatrix();

            model *= Matrix4.CreateRotationY(_yRot);
            _yRot += 0.001f;

            model *= Matrix4.CreateTranslation(0f, 0f, -3f);

            int modelLocation = GL.GetUniformLocation(_shaderProgram, "model");
            int viewLocation = GL.GetUniformLocation(_shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(_shaderProgram, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            //GL.DrawArrays(PrimitiveType.Triangles, 0, 4);  // draw the triangle

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;

            base.OnUpdateFrame(args);
            _camera.Update(KeyboardState, MouseState, args);
        }

        // Function to load a text  file and return its contents as a string
        public static string LoadShaderSourc(string filePath)
        {
            string shaderSource = "";

            try
            {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
                {
                    shaderSource = reader.ReadToEnd();
                }
                // Console.WriteLine(shaderSource);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file: " + e.Message);
            }

            return shaderSource;
        }
    }
}
