using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Rover : Shape
    {
        public Rover(Field field, Vector2 position, float direction, Polarity polarity)
            : base(field, position, polarity) 
        {
            velocity = VecOps.Polar(.5F, direction);
        }

        public Rover(Field field, Vector2 position, Polarity polarity)
            : this(field, position, MathHelper.WrapAngle((float)(new Random().NextDouble()) * MathHelper.TwoPi), polarity) { }

        public override void  PreMove()
        {
            Vector2 nextPos = position + velocity;
            if (nextPos.X < RADIUS)
                velocity = Vector2.Reflect(velocity, Vector2.UnitX);
            if (nextPos.X > field.width - RADIUS)
                velocity = Vector2.Reflect(velocity, -Vector2.UnitX);
            if (nextPos.Y < RADIUS)
                velocity = Vector2.Reflect(velocity, Vector2.UnitY);
            if (nextPos.Y > field.height - RADIUS)
                velocity = Vector2.Reflect(velocity, -Vector2.UnitY);
        }
    }
}