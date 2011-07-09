using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Dual : Chaser
    {
        private float direction;
        public override float Direction
        {
            get { return direction; }
        }

        public Dual(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity) 
        {
            direction = 0F;
        }

        public override void PreMove()
        {
            base.PreMove();

            direction = MathHelper.WrapAngle(direction +
                TurnTowards(base.KillsPlayer ? velocity : Vector2.Negate(velocity)));
        }

        /// <summary>
        /// Returns true if player is dead.
        /// </summary>
        public override bool KillsPlayer
        {
            get
            {
                if (VecOps.AngleBetween(VecOps.Polar(1F, direction), velocity) < MathHelper.PiOver2)
                {
                    //player collided with polarity side
                    return base.KillsPlayer;
                }
                else
                {
                    //player collided with opposite polarity side
                    return polarity.Opposite.HasPolaritiesLackedBy(field.Player.polarity);
                }
            }
        }
    }
}