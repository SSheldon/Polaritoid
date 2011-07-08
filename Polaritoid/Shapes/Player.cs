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

        public override float Direction
        {
            get
            {
                //cache direction so it doesn't jump if player stops moving
                if (velocity != Vector2.Zero)
                    direction = base.Direction;
                return direction;
            }
        }
    }
}