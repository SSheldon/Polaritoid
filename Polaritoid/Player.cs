using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Player : Shape
    {
        public Player(Vector2 position, Vector2 velocity, Polarity polarity, Sprite sprite)
            : base(position, velocity, polarity, sprite)
        {

        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 touchpadPosition)
        {
            velocity = 3F * new Vector2(touchpadPosition.X, -touchpadPosition.Y);

            base.Update(gameTime, playerPosition, playerPolarity, touchpadPosition);

            if (velocity != Vector2.Zero) sprite.rotation = VecOps.Direction(velocity);
        }
    }
}