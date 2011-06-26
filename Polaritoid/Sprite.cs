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
        public float layerDepth;

        public Sprite(Texture2D texture, Vector2 position, Color tint, float rotation, Vector2 origin, float scale, float layerDepth)
        {
            this.texture = texture;
            this.position = position;
            this.tint = tint;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.layerDepth = layerDepth;
        }

        public void Update(Vector2 position, Color tint, float rotation, float scale)
        {
            this.position = position;
            this.tint = tint;
            this.rotation = rotation;
            this.scale = scale;
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
                layerDepth);
        }
    }
}