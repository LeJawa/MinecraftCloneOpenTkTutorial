using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftCloneOpenTkTutorial.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

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

        private List<uint> _indices = new List<uint>()
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
        VAO _vao = null!;
        IBO _ibo = null!;
        ShaderProgram _program = null!;
        Texture _texture = null!;

        // Camera
        Camera _camera = null!;

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

            _vao = new VAO();

            VBO vbo = new VBO(_vertices);
            _vao.LinkToVAO(0, 3, vbo);


            VBO uv = new VBO(_texCoords);
            _vao.LinkToVAO(1, 2, uv);

            _ibo = new IBO(_indices);

            _program = new ShaderProgram("Default.vert", "Default.frag");
            _texture = new Texture("dirt_16.png");

            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(_width, _height, Vector3.Zero);
            CursorState = CursorState.Grabbed; 
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            // Delete VAO, VBO, EBO and shader program
            _vao.Delete();
            _ibo.Delete();
            _texture.Delete();
            _program.Delete();

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // set the color to fill the screen with
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            // fill the screen with the color
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _program.Bind();
            _vao.Bind();
            _ibo.Bind();
            _texture.Bind();

            // transformation matrices
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = _camera.getViewMatrix();
            Matrix4 projection = _camera.getProjectionMatrix();

            model *= Matrix4.CreateRotationY(_yRot);
            _yRot += 0.001f;

            model *= Matrix4.CreateTranslation(0f, 0f, -3f);

            int modelLocation = GL.GetUniformLocation(_program.ID, "model");
            int viewLocation = GL.GetUniformLocation(_program.ID, "view");
            int projectionLocation = GL.GetUniformLocation(_program.ID, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);

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

        
        public static string LoadShaderSource(string filePath)
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
