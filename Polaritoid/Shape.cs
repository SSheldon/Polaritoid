using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    public struct Polarity
    {
        public readonly bool red, blue;

        private Polarity(bool red, bool blue)
        {
            this.red = red;
            this.blue = blue;
        }

        public static readonly Polarity Red = new Polarity(true, false);
        public static readonly Polarity Blue = new Polarity(false, true);
        public static readonly Polarity Both = new Polarity(true, true);
        public static readonly Polarity None = new Polarity(false, false);

        public bool HasAllPolaritiesOf(Polarity other)
        {
            //if (other.red && !this.red) return false;
            //if (other.blue && !this.blue) return false;
            //return true;
            return !(other.red && !this.red) && !(other.blue && !this.blue);
        }

        public bool HasPolaritiesLackedBy(Polarity other)
        {
            //if (!other.red && this.red) return true;
            //if (!other.blue && this.blue) return true;
            //return false;
            return (!other.red && this.red) || (!other.blue && this.blue);
        }

        public Polarity Opposite
        {
            get { return new Polarity(!red, !blue); }
        }
    }

    public class Shape
    {
        public const int RADIUS = 16;

        public Vector2 velocity = Vector2.Zero;
        public Vector2 position;
        public Polarity polarity;
        protected Field field;

        public Shape(Field field, Vector2 position, Polarity polarity)
        {
            this.field = field;
            this.position = position;
            this.polarity = polarity;
        }

        /// <summary>
        /// Returns true if this shape hit the player.
        /// </summary>
        public bool Update()
        {
            PreMove();
            Move(velocity);
            return CollisionCheck(field.Player);
        }

        public virtual void PreMove() { }

        public void Move(Vector2 movement)
        {
            position += movement;
            if (position.X < RADIUS) position.X = RADIUS;
            if (position.X > field.width - RADIUS) position.X = field.width - RADIUS;
            if (position.Y < RADIUS) position.Y = RADIUS;
            if (position.Y > field.height - RADIUS) position.Y = field.height - RADIUS;
        }

        public virtual bool CollisionCheck(Shape other)
        {
            return Vector2.Distance(other.position, position) <= RADIUS * 2;
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
            get { return !polarity.HasPolaritiesLackedBy(field.Player.polarity); }
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