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

        public void Spawn(Enemy enemy, Vector2 position, Polarity polarity)
        {
            Shape s = null;
            switch (enemy)
            {
                case Enemy.Chaser:
                    s = new Chaser(this, position, polarity);
                    break;
                case Enemy.Dual:
                    s = new Dual(this, position, polarity);
                    break;
                case Enemy.Rover:
                    s = new Rover(this, position, polarity);
                    break;
                case Enemy.Smarty:
                    s = new Smarty(this, position, polarity);
                    break;
                case Enemy.Stander:
                    s = new Stander(this, position, polarity);
                    break;
                case Enemy.Shooter:
                    s = new Shooter(this, position, polarity);
                    break;
                case Enemy.Layer:
                    s = new Layer(this, position, polarity);
                    break;
            }
            this.Add(s);
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
            //foreach (Shape s in this) s.Update();
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