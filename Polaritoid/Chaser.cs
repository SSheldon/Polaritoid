using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Chaser : Shape
    {
        public Chaser(Vector2 position, Vector2 velocity, Polarity polarity, Sprite sprite)
            : base(position, velocity, polarity, sprite)
        {

        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 touchpadPosition)
        {
            velocity = playerPosition - position == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(playerPosition - position) * .5F;

            base.Update(gameTime, playerPosition, playerPolarity, touchpadPosition);
        }
    }
}