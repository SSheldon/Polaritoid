using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Smarty : Chaser
    {
        public Smarty(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity) { }

        public override void PreMove()
        {
            base.PreMove();
            if (IsPlayerPolarity) velocity = Vector2.Negate(velocity);
        }
    }
}