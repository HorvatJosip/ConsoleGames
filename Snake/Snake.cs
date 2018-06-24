using System;
using System.Collections.Generic;

using CGL;
using CGL.Board;
using CGL.Entities;
using CGL.Printables;

namespace Snake
{
    class Snake : Entity
    {
        private List<Entity> body;
        private Board board;
        private Graphics graphics;

        public Direction MovementDirection { get; set; }

        public Snake(Board board, Graphics graphics, Position position) : base(graphics, position)
        {
            board.ThrowIfNull(nameof(board));

            this.board = board;
            this.graphics = graphics;

            body = new List<Entity>
            {
                new Drawable(graphics, position)
            };

            MovementDirection = Utils.GetRandomEnumValue(exclusions: Direction.None);
        }

        public bool Move()
        {
            var newPositions = new Position[body.Count];

            for (int i = 1; i < newPositions.Length; i++)
                newPositions[i] = body[i - 1].Position;

            newPositions[0] = body[0].Position.GetRelativePosition(MovementDirection, 1);

            for (int i = 0; i < body.Count; i++)
                try
                {
                    if (!board.MoveEntity(body[i], newPositions[i]))
                    {
                        foreach (var entity in body)
                            board.EditEntities(entity, EditOperation.Delete);

                        return false;
                    }
                }
                catch(IndexOutOfRangeException ex) { return true; }

            return true;
        }

        public void Grow()
        {
            Direction direction;
            var last = body[body.Count - 1];

            if (body.Count == 1) //head only
            {
                switch (MovementDirection)
                {
                    case Direction.None:
                        return;
                    case Direction.Up:
                        direction = Direction.Down;
                        break;
                    case Direction.Down:
                        direction = Direction.Up;
                        break;
                    case Direction.Left:
                        direction = Direction.Right;
                        break;
                    case Direction.Right:
                        direction = Direction.Left;
                        break;
                    default:
                        throw new UnknownEnumValueException();
                }
            }
            else
            {
                var nextToLast = body[body.Count - 2];

                var x = last.Position.X - nextToLast.Position.X;
                var y = last.Position.Y - nextToLast.Position.Y;

                //...[][]==>
                //...[]...
                //...[]...
                if (y == 1)
                    direction = Direction.Down;

                //...[]...
                //...[]...
                //...[][]==>
                else if (y == -1)
                    direction = Direction.Up;

                //   /\
                //   ||
                //...[]...
                //...[]...
                //...[][].
                else if (x == 1)
                    direction = Direction.Right;

                //   /\
                //   ||
                //...[]...
                //...[]...
                //.[][]...
                else //x == -1
                    direction = Direction.Left;
            }

            body.Add(new Drawable(
                body[0].Graphics,
                last.Position.GetRelativePosition(direction, 1)
            ));
        }
    }
}
