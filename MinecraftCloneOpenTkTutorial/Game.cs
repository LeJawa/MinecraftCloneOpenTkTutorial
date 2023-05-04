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

namespace MinecraftCloneOpenTkTutorial
{
    internal class Game : GameWindow
    {
        // CONSTANTS
        private static int SCREENWIDTH;
        private static int SCREENHEIGHT;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            SCREENWIDTH = width;
            SCREENHEIGHT = height;

            // Center the window on monitor
            CenterWindow(new Vector2i(SCREENWIDTH, SCREENHEIGHT));
        }
    }
}
