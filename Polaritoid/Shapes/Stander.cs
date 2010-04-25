using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Stander : Shape
    {
        protected float direction;

        public Stander(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity)
        {
            direction = 0;
        }

        public override void PreMove()
        {
            direction += .05F;
            if (direction >= 2F * (float)Math.PI) direction -= 2F * (float)Math.PI;
        }

        public override Vector2 GetOrientation()
        {
            return VecOps.Polar(1F, direction);
        }
    }
}