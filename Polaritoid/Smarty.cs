using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Smarty : Shape
    {
        public Smarty(Vector2 position, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : base(position, polarity, texture, fieldWidth, fieldHeight) { }

        public override void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition)
        {
            velocity = playerPosition - position == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(playerPosition - position) * .5F;
            if (polarity == Polarity.Blue && playerPolarity != Polarity.Red ||
                polarity == Polarity.Red && playerPolarity != Polarity.Blue) velocity = Vector2.Negate(velocity);

            base.Update(gameTime, playerPosition, playerPolarity, viewCornerPosition);
        }
    }
}