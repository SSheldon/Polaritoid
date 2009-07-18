using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Player : Shape
    {
        public Player(Vector2 position, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : base(position, polarity, texture, fieldWidth, fieldHeight) { }

        public void Update(GameTime gameTime, Vector2 touchpadPosition, Vector2 viewCornerPosition)
        {
            velocity = 3F * touchpadPosition;

            if (velocity != Vector2.Zero) sprite.rotation = VecOps.Direction(new Vector2(velocity.X, -velocity.Y));

            base.Update(gameTime, position, polarity, viewCornerPosition);
        }
    }
}