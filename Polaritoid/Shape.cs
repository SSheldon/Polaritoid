using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    public enum Polarity { Red, Blue, Both };

    public class Shape
    {
        public Vector2 velocity = Vector2.Zero;
        public Vector2 position;
        public Polarity polarity;
        public int radius = 16;
        protected Field field;

        public Shape(Field field, Vector2 position, Polarity polarity)
        {
            this.field = field;
            this.position = position;
            this.polarity = polarity;
        }

        public void Update()
        {
            PreMove();
            Move();
            PostMove();
        }

        public virtual void PreMove() { }
        public virtual void PostMove() { }

        public void Move()
        {
            position += velocity;
            if (position.X < radius) position.X = radius;
            if (position.X > field.width - radius) position.X = field.width - radius;
            if (position.Y < radius) position.Y = radius;
            if (position.Y > field.height - radius) position.Y = field.height - radius;
        }

        public virtual bool CollisionCheck(Shape other)
        {
            return Vector2.Distance(other.position, position) <= radius * 2;
        }

        public float Speed
        {
            get { return velocity.Length(); }
        }

        public virtual Vector2 Orientation
        {
            get { return Vector2.Normalize(velocity); }
        }

        public virtual bool KillsPlayer()
        {
            return !IsPlayerPolarity;
        }

        public bool IsPlayerPolarity
        {
            get
            {
                return polarity == Polarity.Blue && field.Player.polarity != Polarity.Red ||
                    polarity == Polarity.Red && field.Player.polarity != Polarity.Blue;
            }
        }
    }

    public interface IDirectable
    {
        float Direction
        {
            get;
            set;
        }
    }

    public static class IDirectableExtension
    {
        public static void TurnTowards(this IDirectable s, float dir)
        {
            float diff = Math.Abs(dir - s.Direction);
            if ((diff > (float)Math.PI && dir < s.Direction) || (diff <= (float)Math.PI && dir > s.Direction))
                s.Direction += diff < .05F ? diff : .05F;
            else
                if ((diff <= (float)Math.PI && dir < s.Direction) || (diff > (float)Math.PI && dir > s.Direction))
                    s.Direction -= diff < .05F ? diff : .05F;

            if (s.Direction >= 2F * (float)Math.PI) s.Direction -= 2F * (float)Math.PI;
            if (s.Direction < 0) s.Direction += 2F * (float)Math.PI;
        }

        public static void TurnTowards(this IDirectable s, Vector2 orientation)
        {
            s.TurnTowards(VecOps.Direction(orientation));
        }
    }
}