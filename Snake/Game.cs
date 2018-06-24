using System;
using System.Collections.Generic;
using System.Threading;

using CGL;
using CGL.Board;

namespace Snake
{
    class Game : CGL.Game
    {
        private Snake snake;
        private readonly Graphics snakeGraphics = new Graphics(ConsoleColor.Green, '¤');
        private readonly Graphics appleGraphics = new Graphics(ConsoleColor.Red, 'o');

        public Game(Rectangle gameRenderArea, Graphics initialBoardCharGraphics, Graphics initialBorderGraphics = null) 
            : base(gameRenderArea, initialBoardCharGraphics, initialBorderGraphics)
        {
        }

        private void SetupSnake() =>
            snake = new Snake(
                board,
                snakeGraphics,
                board.GetRandomSpawnPoint()
            );

        private void SpawnApple() =>
            board.EditEntities(
                new Tile(TileType.Gatherable | TileType.Walkable, appleGraphics, board.GetRandomSpawnPoint()),
                EditOperation.UpdateOrCreate
            );

        #region Overrides

        protected override void Initialize()
        {
            SetupSnake();

            board.Draw();
            board.EditEntities(snake, EditOperation.UpdateOrCreate);
            SpawnApple();

            Console.SetCursorPosition(0, 0);

            keyPressActions = new Dictionary<(ConsoleModifiers, ConsoleKey), Action>
            {
                { (0, ConsoleKey.W), () => snake.MovementDirection = Direction.Up },
                { (0, ConsoleKey.S), () => snake.MovementDirection = Direction.Down },
                { (0, ConsoleKey.A), () => snake.MovementDirection = Direction.Left },
                { (0, ConsoleKey.D), () => snake.MovementDirection = Direction.Right },

                { (0, ConsoleKey.UpArrow), () => snake.MovementDirection = Direction.Up },
                { (0, ConsoleKey.DownArrow), () => snake.MovementDirection = Direction.Down },
                { (0, ConsoleKey.LeftArrow), () => snake.MovementDirection = Direction.Left },
                { (0, ConsoleKey.RightArrow), () => snake.MovementDirection = Direction.Right },
            };
        }

        protected override void LoopStart()
        {
            Thread.Sleep(65);

            if (!snake.Move())
                SetupSnake();
        }

        protected override void EntityMoved(object sender, EntityMovementEventArgs e)
        {
            if (e.Tile.Type.HasFlag(TileType.Gatherable))
            {
                snake.Grow();
                SpawnApple();
            }
        }

        #endregion
    }
}
