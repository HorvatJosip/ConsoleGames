using System;
using System.Collections.Generic;
using System.Threading;

using CGL;
using CGL.Board;
using CGL.Printables;

namespace Snake
{
    class Game : CGL.Game
    {
        #region Fields

        private Snake snake;
        private Graphics snakeGraphics, appleGraphics;

        private PrintableArea<string> scoreBoard;
        private int score;
        private Text lblScore;
        private Position txtScoreStartPosition;

        #endregion

        public Game(Rectangle gameRenderArea, Graphics initialBoardCharGraphics, Graphics initialBorderGraphics = null)
            : base(gameRenderArea, initialBoardCharGraphics, initialBorderGraphics) { }

        #region Methods

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
            snakeGraphics = new Graphics(ConsoleColor.Green, '¤');
            appleGraphics = new Graphics(ConsoleColor.Red, 'o');

            lblScore = new Text("Score:", ConsoleColor.Cyan);
            txtScoreStartPosition = new Position(lblScore.Content.Count + 1, 0);

            scoreBoard = new PrintableArea<string>(new Rectangle(
                startX: 0,
                startY: 0,
                 width: board.DrawableArea.Width,
                height: board.DrawableArea.StartPosition.Y
            ));

            scoreBoard.EditFixedText(EditOperation.Create, "lblScore", lblScore, true);
            scoreBoard.EditChangableText(EditOperation.Create, "txtScore", new Text(txtScoreStartPosition, "0", ConsoleColor.White), true);

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

                { (0, ConsoleKey.Escape), () => Exit(new Text("Thanks for playing", ConsoleColor.Cyan)) }
            };
        }

        protected override void LoopStart()
        {
            Thread.Sleep(65);

            if (!snake.Move())
            {
                SetupSnake();
                ChangeScore(true);
            }
        }

        protected override void EntityMoved(object sender, EntityMovementEventArgs e)
        {
            if (e.Tile.Type.HasFlag(TileType.Gatherable))
            {
                board.EditEntities(e.Tile, EditOperation.Delete);
                snake.Grow();
                SpawnApple();

                ChangeScore(false);
            }
        }

        #endregion

        private void ChangeScore(bool reset)
        {
            score = reset ? 0 : score + 1;

            var txtScore = new Text(txtScoreStartPosition, $"{score}", ConsoleColor.White);
            scoreBoard.EditChangableText(EditOperation.Update, "txtScore", txtScore, true);
        } 

        #endregion
    }
}
