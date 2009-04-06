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

        public Shape(Vector2 position, Vector2 velocity, Polarity polarity, Sprite sprite)
        {
            this.position = position;
            this.velocity = velocity;
            this.polarity = polarity;
            this.sprite = sprite;
        }

        public virtual void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 touchpadPosition)
        {
            Move();

            sprite.position = position;
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
        }

        public float Speed
        {
            get { return velocity.Length(); }
        }
    }
}