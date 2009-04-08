using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    class Dual : Shape
    {
        Sprite other;
        public float direction;

        public Dual(Vector2 position, Vector2 velocity, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
            : base(position, velocity, polarity, texture, fieldWidth, fieldHeight) 
        {
            other = new Sprite(texture, new Vector2(16, 16), 1F);
            direction = 0F;
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition)
        {
            velocity = playerPosition - position == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(playerPosition - position) * .5F;
            if (polarity != playerPolarity) velocity = Vector2.Negate(velocity);
            float diff = Math.Abs(VecOps.Direction(velocity) - direction);
            if ((diff > (float)Math.PI && VecOps.Direction(velocity) < direction) ||
                (diff <= (float)Math.PI && VecOps.Direction(velocity) > direction)) 
                direction += diff < .05F ? diff : .05F;
            else
                if ((diff <= (float)Math.PI && VecOps.Direction(velocity) < direction) || 
                    (diff > (float)Math.PI && VecOps.Direction(velocity) > direction))
                    direction -= diff < .05F ? diff : .05F;
            if (polarity != playerPolarity) velocity = Vector2.Negate(velocity);

            if (direction >= 2F * (float)Math.PI) direction -= 2F * (float)Math.PI;
            if (direction < 0) direction += 2F * (float)Math.PI;

            sprite.rotation = VecOps.Direction(new Vector2(Orientation.X, -Orientation.Y));

            //update for other sprite
            other.position = new Vector2(position.X - viewCornerPosition.X, 320 - position.Y + viewCornerPosition.Y);
            switch (polarity)
            {
                case Polarity.Blue:
                    other.tint = Color.Red;
                    break;
                case Polarity.Red:
                    other.tint = Color.Blue;
                    break;
                case Polarity.Both:
                    other.tint = Color.Purple;
                    break;
            }
            other.rotation = sprite.rotation + (float)Math.PI;

            base.Update(gameTime, playerPosition, playerPolarity, viewCornerPosition);
        }

        public override void Draw(SpriteBatch batch)
        {
            sprite.Draw(batch);
            other.Draw(batch);
        }

        public Vector2 Orientation
        {
            get { return VecOps.Polar(1F, direction); }
            set { direction = VecOps.Direction(value); }
        }
    }
}