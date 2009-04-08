using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Player : Shape
    {
        public Player(Vector2 position, Vector2 velocity, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : base(position, velocity, polarity, texture, fieldWidth, fieldHeight) { }

        public void Update(GameTime gameTime, Vector2 touchpadPosition)
        {
            velocity = 3F * touchpadPosition;

            if (velocity != Vector2.Zero) sprite.rotation = VecOps.Direction(new Vector2(velocity.X, -velocity.Y));

            //base.Update(gameTime, position, polarity);
        }
    }
}