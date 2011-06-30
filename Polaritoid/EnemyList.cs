using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Field
    {
        public readonly int width, height;
        private Player player;
        public Player Player
        {
            get { return player; }
        }
        private TimeSpan time;
        public TimeSpan Time
        {
            get { return time; }
        }
        private List<Shape> shapes;
        private Shape[,] grid;
        private Shape this[Vector2 v]
        {
            get
            {
                return grid[(int)(v.X / 20), (int)(v.Y / 20)];
            }
            set
            {
                grid[(int)(v.X / 20), (int)(v.Y / 20)] = value;
            }
        }

        public Field(int width, int height)
        {
            this.width = width;
            this.height = height;
            shapes = new List<Shape>();
            //20 < r * sqrt(2), so only one shape will fit in a square on the grid
            grid = new Shape[(int)Math.Ceiling(width / 20.0), (int)Math.Ceiling(height / 20.0)];
        }

        public IEnumerable<Shape> Shapes
        {
            get
            {
                for (int i = 0; i < shapes.Count; i++)
                {
                    yield return shapes[i];
                }
            }
        }

        public IEnumerable<Shape> SurroundingShapes(Vector2 position)
        {
            int x = (int)(position.X / 20);
            int y = (int)(position.Y / 20);
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < grid.GetLength(0) &&
                        j >= 0 && j < grid.GetLength(1) &&
                        grid[i, j] != null)
                        yield return grid[i, j];
                }
            }
        }

        private void Spawn(Shape s)
        {
            if (s is Player)
            {
                if (player == null)
                    player = (Player)s;
                //else we're trying to add a second player?
            }
            this[s.position] = s;
            shapes.Add(s);
        }

        public void Spawn(Type enemy, Vector2 position, Polarity polarity)
        {
            Spawn((Shape)enemy.GetConstructor(new Type[] { typeof(Field), typeof(Vector2), typeof(Polarity) }).Invoke(
                new object[] { this, position, polarity }));
        }

        public void Spawn(Vector2 position, Polarity polarity, float direction)
        {
            Spawn(new Rover(this, position, direction, polarity));
        }

        public bool Update(GameTime gameTime)
        {
            time = gameTime.TotalGameTime;
            foreach (Shape s in Shapes) s.Update();
            return DeathCheck();
        }

        /// <summary>
        /// Removes dead shapes and returns true if the player is dead.
        /// </summary>
        private bool DeathCheck()
        {
            bool playerDead = false;
            for (int counter = shapes.Count - 1; counter >= 0; counter--)
            {
                Shape s = shapes[counter];
                if (s != Player && s.CollisionCheck(Player))
                {
                    if (s.KillsPlayer())
                    {
                        playerDead = true;
                    }
                    else
                    {
                        shapes.RemoveAt(counter);
                    }
                }
            }
            return playerDead;
        }
    }
}