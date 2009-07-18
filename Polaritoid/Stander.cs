using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Stander : Shape
    {
        public Stander(Vector2 position, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : base(position, polarity, texture, fieldWidth, fieldHeight) { }

        public override void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition)
        {
            sprite.rotation += .05F;
            if (sprite.rotation >= 2F * (float)Math.PI) sprite.rotation -= 2F * (float)Math.PI;

            base.Update(gameTime, playerPosition, playerPolarity, viewCornerPosition);
        }
    }
}