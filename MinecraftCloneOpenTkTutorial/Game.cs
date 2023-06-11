using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftCloneOpenTkTutorial.Graphics;
using MinecraftCloneOpenTkTutorial.World;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinecraftCloneOpenTkTutorial
{
    internal class Game : GameWindow
    {
        Chunk _chunk;
        ShaderProgram _program;

        // Camera
        Camera _camera = null!;

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

            _chunk = new Chunk(Vector3.Zero);
            _program = new ShaderProgram("Default.vert", "Default.frag");
            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(_width, _height, Vector3.Zero);
            CursorState = CursorState.Grabbed; 
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            _chunk.Delete();

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // set the color to fill the screen with
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            // fill the screen with the color
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // transformation matrices
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = _camera.getViewMatrix();
            Matrix4 projection = _camera.getProjectionMatrix();

            int modelLocation = GL.GetUniformLocation(_program.ID, "model");
            int viewLocation = GL.GetUniformLocation(_program.ID, "view");
            int projectionLocation = GL.GetUniformLocation(_program.ID, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            _chunk.Render(_program);

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
