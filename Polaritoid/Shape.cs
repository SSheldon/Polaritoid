using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    public class Shape
    {
        public Vector2 velocity;
        public Vector2 position;
        public Sprite sprite;
        public Polarity polarity;
        public int fieldWidth, fieldHeight;
        public int radius = 12;

        public Shape(Vector2 position, Vector2 velocity, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
        {
            this.position = position;
            this.velocity = velocity;
            this.polarity = polarity;
            this.sprite = new Sprite(texture, new Vector2(16, 16), .75F);
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
        }

        public virtual void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition)
        {
            Move();

            sprite.position = new Vector2(position.X - viewCornerPosition.X, 319 - position.Y + viewCornerPosition.Y);
            switch (polarity)
            {
                case Polarity.Blue:
                    sprite.tint = Color.Blue;
                    break;
                case Polarity.Red:
                    sprite.tint = Color.Red;
                    break;
                case Polarity.Both:
                    sprite.tint = Color.Purple;
                    break;
            }
        }

        public void Move()
        {
            position += velocity;
            if (position.X < radius) position.X = radius;
            if (position.X > fieldWidth - radius) position.X = fieldWidth - radius;
            if (position.Y < radius) position.Y = radius;
            if (position.Y > fieldHeight - radius) position.Y = fieldHeight - radius;
        }

        public float Speed
        {
            get { return velocity.Length(); }
        }
    }
}