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
            : this(field, position, (float)(new Random().NextDouble() * 2D * Math.PI), polarity) { }

        public override void  PreMove()
        {
            if ((position + velocity).X < radius)
                velocity = Vector2.Reflect(velocity, Vector2.UnitX);
            if ((position + velocity).X > field.width - radius)
                velocity = Vector2.Reflect(velocity, -Vector2.UnitX);
            if ((position + velocity).Y < radius)
                velocity = Vector2.Reflect(velocity, Vector2.UnitY);
            if ((position + velocity).Y > field.height - radius)
                velocity = Vector2.Reflect(velocity, -Vector2.UnitY);
        }
    }
}