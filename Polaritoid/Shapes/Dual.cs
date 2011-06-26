using System;
using Microsoft.Xna.Framework;

namespace Polaritoid
{
    public class Dual : Chaser, IDirectable
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

        public Dual(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity) 
        {
            direction = 0F;
        }

        public override void PreMove()
        {
            base.PreMove();

            if (!IsPlayerPolarity) this.TurnTowards(velocity);
            else this.TurnTowards(Vector2.Negate(velocity));
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
            if (VecOps.AngleBetween(Orientation, velocity) < (float)Math.PI * .5F)
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

        public override Vector2 Orientation
        {
            get { return VecOps.Polar(1F, direction); }
        }
    }
}