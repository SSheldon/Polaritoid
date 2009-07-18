using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Chaser : Shape
    {
        public Chaser(Vector2 position, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : base(position, polarity, texture, fieldWidth, fieldHeight) { }

        public override void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition)
        {
            velocity = playerPosition - position == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(playerPosition - position) * .5F;

            base.Update(gameTime, playerPosition, playerPolarity, viewCornerPosition);
        }
    }
}