using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Chaser : Shape
    {
        public Chaser(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity) { }

        public override void PreMove()
        {
            velocity = field.Player.position - position == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(field.Player.position - position) * .5F;
        }
    }
}