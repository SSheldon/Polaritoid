using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    class Layer : Rover
    {
        private TimeSpan? lastMine;

        public Layer(Field field, Vector2 position, float direction, Polarity polarity)
            : base(field, position, direction, polarity) { }

        public Layer(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity) { }

        public override void PreMove()
        {
            base.PreMove();
            if (!lastMine.HasValue) lastMine = field.Time;
            if (field.Time.Subtract(lastMine.Value).Seconds > 2)
            {
                field.Spawn(Enemy.Stander, position, polarity);
                lastMine = field.Time;
            }
        }
    }
}