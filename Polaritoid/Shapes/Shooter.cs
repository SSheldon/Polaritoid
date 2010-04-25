using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Shooter : Stander, IDirectable
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

            this.TurnTowards(field.Player.position - position);

            if (field.Time.Subtract(lastShot.Value).Seconds > 2)
            {
                field.Spawn(position, polarity, Direction);
                lastShot = field.Time;
            }
        }
    }
}