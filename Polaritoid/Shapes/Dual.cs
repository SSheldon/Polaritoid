using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Dual : Chaser
    {
        public float direction;

        public Dual(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity) 
        {
            direction = 0F;
        }

        public override void PreMove()
        {
            base.PreMove();
            if (!IsPlayerPolarity) velocity = Vector2.Negate(velocity);
            float diff = Math.Abs(VecOps.Direction(velocity) - direction);
            if ((diff > (float)Math.PI && VecOps.Direction(velocity) < direction) ||
                (diff <= (float)Math.PI && VecOps.Direction(velocity) > direction)) 
                direction += diff < .05F ? diff : .05F;
            else
                if ((diff <= (float)Math.PI && VecOps.Direction(velocity) < direction) || 
                    (diff > (float)Math.PI && VecOps.Direction(velocity) > direction))
                    direction -= diff < .05F ? diff : .05F;
            if (!IsPlayerPolarity) velocity = Vector2.Negate(velocity);

            if (direction >= 2F * (float)Math.PI) direction -= 2F * (float)Math.PI;
            if (direction < 0) direction += 2F * (float)Math.PI;
        }

        /// <summary>
        /// Returns true if player is dead.
        /// </summary>
        public override bool KillsPlayer()
        {
            if (polarity == Polarity.Blue)
            {
                if (VecOps.AngleBetween(GetOrientation(), velocity) < (float)Math.PI * .5F)
                {
                    //player collided with red side
                    //if player is blue, player
                    if (field.Player.polarity == Polarity.Blue)
                    {
                        //player dies
                        return true;
                    }
                    else
                    {
                        //dual dies
                        return false;
                    }
                }
                else
                {
                    //player collided with blue side
                    if (field.Player.polarity == Polarity.Red)
                    {
                        //player dies
                        return true;
                    }
                    else
                    {
                        //dual dies
                        return false;
                    }
                }
            }
            else
            {
                if (VecOps.AngleBetween(GetOrientation(), velocity) < (float)Math.PI * .5F)
                {
                    //player collided with blue side
                    if (field.Player.polarity == Polarity.Red)
                    {
                        //player dies
                        return true;
                    }
                    else
                    {
                        //dual dies
                        return false;
                    }
                }
                else
                {
                    //player collided with red side
                    if (field.Player.polarity == Polarity.Blue)
                    {
                        //player dies
                        return true;
                    }
                    else
                    {
                        //dual dies
                        return false;
                    }
                }
            }
        }

        public override Vector2 GetOrientation()
        {
            return VecOps.Polar(1F, direction);
        }
    }
}