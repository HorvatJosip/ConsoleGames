using System;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameRenderArea = new CGL.Rectangle(
                startX: 0,
                startY: 2,
                 width: (int)(Console.LargestWindowWidth * 0.25),
                height: (int)(Console.LargestWindowHeight * 0.25)
            );

            Console.SetWindowSize(
                 width: gameRenderArea.Width,
                height: gameRenderArea.Bottom + 1
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
