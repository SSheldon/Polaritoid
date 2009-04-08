using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Smarty : Shape
    {
        public Smarty(Vector2 position, Vector2 velocity, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : base(position, velocity, polarity, texture, fieldWidth, fieldHeight) { }

        public override void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition)
        {
            velocity = playerPosition - position == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(playerPosition - position) * .5F;
            if (polarity == playerPolarity) velocity = Vector2.Negate(velocity);

            base.Update(gameTime, playerPosition, playerPolarity, viewCornerPosition);
        }
    }
}