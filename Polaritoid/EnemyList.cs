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
            if (s is Player)
            {
                if (player == null)
                    player = (Player)s;
                else return; //we're trying to add a second player?
            }
            else enemies.Add(s);
            this[s.position] = s;
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
            player.Update();
            //update enemies
            for (int i = 0, next = 0; i < enemies.Count; i = next)
            {
                //advance next to the next non-null shape
                do
                {
                    next++;
                } while (next < enemies.Count && enemies[next] == null);
                //if current is null, swap next into its spot
                if (enemies[i] == null)
                {
                    //break if no more non-null shapes
                    if (!MoveUpNext(i, next)) break;
                }
                Shape s = enemies[i];
                //update current
                s.Update();
                if (s.CollisionCheck(Player))
                {
                    if (s.KillsPlayer())
                    {
                        playerDead = true;
                    }
                    else
                    {
                        //shape is dead, nullify it
                        enemies[i] = null;
                        //swap next into its spot
                        MoveUpNext(i, next);
                        //decrement so swapped shape gets updated
                        i--;
                    }
                } //end collision check
            } //end enemies update loop
            return playerDead;
        }

        /// <summary>
        /// Returns true if there was another shape to move up.
        /// </summary>
        private bool MoveUpNext(int i, int next)
        {
            if (next < enemies.Count)
            {
                //move next up and nullify current
                enemies[i] = enemies[next];
                enemies[next] = null;
                return true;
            }
            else
            {
                //no more shapes, so remove the remaining nulls
                for (int j = enemies.Count - 1; j >= i; j--)
                    enemies.RemoveAt(j);
                return false;
            }
        }
    }
}