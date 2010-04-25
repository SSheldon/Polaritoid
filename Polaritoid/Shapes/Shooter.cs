using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Shooter : Stander
    {
        public TimeSpan? lastShot;

        public Shooter(Field field, Vector2 position, Polarity polarity)
            : base(field, position, polarity)
        {
            lastShot = null;
        }

        public override void PreMove()
        {
            if (!lastShot.HasValue) lastShot = field.Time;

            Vector2 displacementToPlayer = field.Player.position - position;
            float diff = Math.Abs(VecOps.Direction(displacementToPlayer) - direction);
            if ((diff > (float)Math.PI && VecOps.Direction(displacementToPlayer) < direction) ||
                (diff <= (float)Math.PI && VecOps.Direction(displacementToPlayer) > direction))
                direction += diff < .05F ? diff : .05F;
            else
                if ((diff <= (float)Math.PI && VecOps.Direction(displacementToPlayer) < direction) ||
                    (diff > (float)Math.PI && VecOps.Direction(displacementToPlayer) > direction))
                    direction -= diff < .05F ? diff : .05F;

            if (direction >= 2F * (float)Math.PI) direction -= 2F * (float)Math.PI;
            if (direction < 0) direction += 2F * (float)Math.PI;

            if (field.Time.Subtract(lastShot.Value).Seconds > 2)
            {
                field.Spawn(position, polarity, direction);
                lastShot = field.Time;
            }

        }
    }
}