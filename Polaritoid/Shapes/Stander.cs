using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Stander : Shape
    {
        protected float direction;
        public override float Direction
        {
            get { return direction; }
        }

        public Stander(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity)
        {
            direction = 0;
        }

        public override void PreMove()
        {
            direction += .05F;
            if (direction >= 2F * (float)Math.PI) direction -= 2F * (float)Math.PI;
            if (direction < 0) direction += 2F * (float)Math.PI;
        }
    }
}