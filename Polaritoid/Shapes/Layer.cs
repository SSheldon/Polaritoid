using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Layer : Rover
    {
        protected TimeSpan? lastMine;
        protected Polarity minePolarity;

        public Layer(Field field, Vector2 position, float direction, Polarity polarity)
            : base(field, position, direction, polarity)
        {
            minePolarity = polarity;
        }

        public Layer(Field field, Vector2 position, Polarity polarity)
            : this(field, position, RandomAngle(), polarity) { }

        public override void PreMove()
        {
            base.PreMove();
            if (!lastMine.HasValue) lastMine = field.Time;
            if (field.Time.Subtract(lastMine.Value).Seconds > 2)
            {
                Vector2 offset = Vector2.Normalize(velocity) * (-2 * RADIUS);
                field.Spawn(typeof(Stander), position + offset, minePolarity);
                lastMine = field.Time;
            }
        }
    }
}