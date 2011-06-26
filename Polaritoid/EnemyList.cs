using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Field : IEnumerable<Shape>
    {
        public readonly int width, height;
        private Player player;
        public Player Player
        {
            get
            {
                if (player == null)
                    foreach (Shape s in this)
                    {
                        if (s is Player) player = (Player)s;
                        break;
                    }
                return player;
            }
        }
        private TimeSpan time;
        public TimeSpan Time
        {
            get { return time; }
        }
        private List<Shape> shapes;

        public Field(int width, int height)
        {
            shapes = new List<Shape>();
            this.width = width;
            this.height = height;
        }

        public IEnumerator<Shape> GetEnumerator()
        {
            return shapes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Spawn(Type enemy, Vector2 position, Polarity polarity)
        {
            shapes.Add((Shape)enemy.GetConstructor(new Type[] { typeof(Field), typeof(Vector2), typeof(Polarity) }).Invoke(
                new object[] { this, position, polarity }));
        }

        public void Spawn(Vector2 position, Polarity polarity, float direction)
        {
            shapes.Add(new Rover(this, position, direction, polarity));
        }

        public bool Update(GameTime gameTime)
        {
            time = gameTime.TotalGameTime;
            bool playerDead = false;
            for (int i = 0; i < shapes.Count; i++) shapes[i].Update();
            for (int counter = shapes.Count - 1; counter >= 0; counter--)
            {
                Shape s = shapes[counter];
                if (s != player && s.CollisionCheck(player))
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