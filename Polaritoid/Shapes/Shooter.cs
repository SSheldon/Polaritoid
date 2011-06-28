using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Shooter : Stander
    {
        public TimeSpan? lastShot;

        public Shooter(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity)
        {
            lastShot = null;
        }

        public override void PreMove()
        {
            if (!lastShot.HasValue) lastShot = field.Time;

            direction += TurnTowards(field.Player.position - position);
            if (direction >= 2F * (float)Math.PI) direction -= 2F * (float)Math.PI;
            if (direction < 0) direction += 2F * (float)Math.PI;

            if (field.Time.Subtract(lastShot.Value).Seconds > 2)
            {
                field.Spawn(position, polarity, Direction);
                lastShot = field.Time;
            }
        }
    }
}