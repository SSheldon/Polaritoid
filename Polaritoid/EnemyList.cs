using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Field : List<Shape>
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

        public Field(int width, int height)
            : base()
        {
            this.width = width;
            this.height = height;
        }

        public void Spawn(Type enemy, Vector2 position, Polarity polarity)
        {
            this.Add((Shape)enemy.GetConstructor(new Type[] { typeof(Field), typeof(Vector2), typeof(Polarity) }).Invoke(
                new object[] { this, position, polarity }));
        }

        public void Spawn(Vector2 position, Polarity polarity, float direction)
        {
            this.Add(new Rover(this, position, direction, polarity));
        }

        public bool Update(GameTime gameTime)
        {
            time = gameTime.TotalGameTime;
            bool playerDead = false;
            for (int i = 0; i < Count; i++) this[i].Update();
            for (int counter = this.Count - 1; counter >= 0; counter--)
            {
                Shape s = this[counter];
                if (s != player && s.CollisionCheck(player))
                {
                    if (s.KillsPlayer())
                    {
                        playerDead = true;
                    }
                    else
                    {
                        this.RemoveAt(counter);
                    }
                }
            }
            return playerDead;
        }
    }
}