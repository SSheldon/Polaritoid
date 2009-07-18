using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Layer : Shape
    {
        public TimeSpan? lastMine;

        public Layer(Vector2 position, float direction, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : base(position, polarity, texture, fieldWidth, fieldHeight) 
        {
            velocity = VecOps.Polar(.5F, direction);
        }

        public Layer(Vector2 position, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : this(position, (float)(new Random().NextDouble() * 2D * Math.PI), polarity, texture, fieldWidth, fieldHeight) { }

        public override void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition)
        {
            if ((position + velocity).X < radius)
                velocity = Vector2.Reflect(velocity, Vector2.UnitX);
            if ((position + velocity).X > fieldWidth - radius)
                velocity = Vector2.Reflect(velocity, -Vector2.UnitX);
            if ((position + velocity).Y < radius)
                velocity = Vector2.Reflect(velocity, Vector2.UnitY);
            if ((position + velocity).Y > fieldHeight - radius)
                velocity = Vector2.Reflect(velocity, -Vector2.UnitY);

            sprite.rotation = VecOps.Direction(new Vector2(velocity.X, -velocity.Y));

            if (!lastMine.HasValue) lastMine = gameTime.TotalGameTime;

            base.Update(gameTime, playerPosition, playerPolarity, viewCornerPosition);
        }
    }
}