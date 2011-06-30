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
                TurnTowards(!IsPlayerPolarity ? velocity : Vector2.Negate(velocity)));
        }

        private bool OppositeIsPlayerPolarity
        {
            get
            {
                Polarity opp = (polarity == Polarity.Red ? Polarity.Blue : Polarity.Red);
                return opp == Polarity.Blue && field.Player.polarity != Polarity.Red ||
                    opp == Polarity.Red && field.Player.polarity != Polarity.Blue;
            }
        }

        /// <summary>
        /// Returns true if player is dead.
        /// </summary>
        public override bool KillsPlayer()
        {
            if (VecOps.AngleBetween(VecOps.Polar(1F, direction), velocity) < MathHelper.PiOver2)
            {
                //player collided with polarity side
                return base.KillsPlayer();
            }
            else
            {
                //player collided with opposite polarity side
                return !OppositeIsPlayerPolarity;
            }
        }
    }
}