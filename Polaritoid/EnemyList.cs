using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Field
    {
        public readonly int width, height;
        //20 < r * sqrt(2), so only one shape will fit in a square on the grid
        private const int TILESIZE = 20;
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
        private List<Shape> enemies;
        private Shape[,] grid;
        private Shape this[Vector2 v]
        {
            get
            {
                return grid[(int)(v.X / TILESIZE), (int)(v.Y / TILESIZE)];
            }
            set
            {
                grid[(int)(v.X / TILESIZE), (int)(v.Y / TILESIZE)] = value;
            }
        }

        public Field(int width, int height)
        {
            this.width = width;
            this.height = height;
            enemies = new List<Shape>();
            grid = new Shape[(int)Math.Ceiling((double)width / TILESIZE),
                (int)Math.Ceiling((double)height / TILESIZE)];
        }

        public IEnumerable<Shape> Shapes
        {
            get
            {
                yield return player;
                for (int i = 0; i < enemies.Count; i++)
                {
                    yield return enemies[i];
                }
            }
        }

        public IEnumerable<Shape> SurroundingShapes(Vector2 position)
        {
            int x = (int)(position.X / TILESIZE);
            int y = (int)(position.Y / TILESIZE);
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
            if (!AssignToGrid(s)) return; //square was occupied
            if (s is Player)
            {
                if (player == null)
                    player = (Player)s;
                //we can't add a second player
                else this[s.position] = null;
            }
            else enemies.Add(s);
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

        /// <summary>
        /// Returns true if the player is dead.
        /// </summary>
        public bool Update(GameTime gameTime)
        {
            time = gameTime.TotalGameTime;
            bool playerDead = false;
            //update player
            this[player.position] = null;
            player.Move(player.velocity);
            AssignToGrid(player);
            //update enemies
            Nullerator<Shape> num = new Nullerator<Shape>(enemies);
            while (num.MoveNext())
            {
                Shape s = num.Current;
                this[s.position] = null;
                //update current
                if (s.Update())
                {
                    //shape hit the player
                    if (s.KillsPlayer)
                    {
                        playerDead = true;
                    }
                    else
                    {
                        //shape is dead, nullify it
                        s = null;
                        num.Nullify();
                    }
                } //end collision check
                if (s != null) AssignToGrid(s);
            } //end enemies update loop
            return playerDead;
        }

        public bool AssignToGrid(Shape s)
        {
            if (this[s.position] == null)
            {
                this[s.position] = s;
                return true;
            }
            else return false;
        }
    }
}