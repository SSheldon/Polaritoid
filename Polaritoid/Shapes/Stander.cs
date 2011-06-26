using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Stander : Shape, IDirectable
    {
        private float direction;
        public float Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                if (direction >= 2F * (float)Math.PI) direction -= 2F * (float)Math.PI;
                if (direction < 0) direction += 2F * (float)Math.PI;
            }
        }

        public Stander(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity)
        {
            direction = 0;
        }

        public override void PreMove()
        {
            Direction += .05F;
        }

        public override Vector2 Orientation
        {
            get { return VecOps.Polar(1F, Direction); }
        }
    }
}