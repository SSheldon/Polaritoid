using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Shooter : Shape
    {
        public float direction;
        public TimeSpan? lastShot;

        public Shooter(Vector2 position, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : base(position, polarity, texture, fieldWidth, fieldHeight)
        {
            direction = 0F;
            lastShot = null;
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition)
        {
            if (!lastShot.HasValue) lastShot = gameTime.TotalGameTime;

            Vector2 displacementToPlayer = playerPosition - position;
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

            sprite.rotation = VecOps.Direction(new Vector2(VecOps.Polar(1F, direction).X, -VecOps.Polar(1F, direction).Y));

            base.Update(gameTime, playerPosition, playerPolarity, viewCornerPosition);
        }
    }
}