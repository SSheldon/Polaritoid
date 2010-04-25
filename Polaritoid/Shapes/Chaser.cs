using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Chaser : Shape
    {
        public Chaser(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity) { }

        public override void PreMove()
        {
            velocity = field.Player.position - position == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(field.Player.position - position) * .5F;
        }
    }
}