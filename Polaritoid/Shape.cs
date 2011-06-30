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

        public virtual float Direction
        {
            get { return VecOps.Direction(velocity); }
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

        private static float AngleTo(float from, float to)
        {
            //assume from and to are wrapped angles
            float diff = to - from;
            if (Math.Abs(diff) > MathHelper.Pi)
                return (diff > 0 ? -1F : 1F) * MathHelper.TwoPi + diff;
            else return diff;
        }

        /// <summary>
        /// Returns the angle by which the shape should turn to attain the specified angle.
        /// </summary>
        protected float TurnTowards(float dir)
        {
            return MathHelper.Clamp(AngleTo(Direction, dir), -.05F, .05F);
        }

        /// <summary>
        /// Returns the angle by which the shape should turn to attain the specified orientation.
        /// </summary>
        protected float TurnTowards(Vector2 orientation)
        {
            return TurnTowards(VecOps.Direction(orientation));
        }
    }
}