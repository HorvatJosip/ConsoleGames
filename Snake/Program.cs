using System;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameRenderArea = new CGL.Rectangle(
                0,
                0,
                (int)(Console.LargestWindowWidth  * 0.4),
                (int)(Console.LargestWindowHeight * 0.4)
            );

            Console.SetWindowSize(
                gameRenderArea.Width,
                gameRenderArea.Height
            );
            Console.CursorVisible = false;

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var game = new Game(
                gameRenderArea,
                new CGL.Graphics(ConsoleColor.White, ' '),
                new CGL.Graphics(ConsoleColor.White, '#')
            );
        }
    }
}
