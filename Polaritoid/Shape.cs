using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    public class Shape
    {
        public Vector2 velocity = Vector2.Zero;
        public Vector2 position;
        protected Sprite sprite;
        public Polarity polarity;
        public int fieldWidth, fieldHeight;
        public int radius = 16;
        public bool dead = false;

        public Shape(Vector2 position, Polarity polarity, Texture2D texture, int fieldWidth, int fieldHeight)
        {
            this.position = position;
            this.polarity = polarity;
            this.sprite = new Sprite(texture, new Vector2(16, 16), (float)radius / 16F);
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
        }

        public virtual void Update(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition/*, List<Shape> enemies*/)
        {
            Move();

            sprite.position = new Vector2(position.X - viewCornerPosition.X, 320 - position.Y + viewCornerPosition.Y);
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

        /// <summary>
        /// Returns true if player is dead.
        /// </summary>
        public virtual bool CollisionCheck(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity)
        {
            if (Vector2.Distance(playerPosition, position) <= radius * 2)
            {
                if ((polarity == Polarity.Blue && playerPolarity == Polarity.Red) ||
                    (polarity == Polarity.Red && playerPolarity == Polarity.Blue))
                {
                    //player dies
                    return true;
                }
                else
                {
                    //shape dies
                    dead = true;
                    return false;
                }
            }
            else return false;
        }

        public virtual void Draw(SpriteBatch batch)
        {
            sprite.Draw(batch);
        }

        public float Speed
        {
            get { return velocity.Length(); }
        }
    }
}