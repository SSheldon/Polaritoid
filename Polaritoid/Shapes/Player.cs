using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Player : Shape
    {
        private float direction;

        public Player(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity)
        {
            direction = 0;
        }

        public override void  PreMove()
        {
            if (velocity != Vector2.Zero) direction = VecOps.Direction(velocity);
        }

        public override Vector2 GetOrientation()
        {
            return VecOps.Polar(1F, direction);
        }
    }
}