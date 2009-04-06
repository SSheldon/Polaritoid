using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    public class Sprite
    {
        public Texture2D texture;
        public float rotation;
        public Vector2 position;
        public Vector2 origin;
        public float scale;
        public Color tint;

        public Sprite(Texture2D texture, Vector2 position, Vector2 origin)
        {
            this.texture = texture;
            this.position = position;
            this.origin = origin;
            rotation = 0.0F;
            scale = .75F;
            tint = Color.White;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(
                texture,
                position,
                null,
                tint,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0.0F);
        }
    }
}