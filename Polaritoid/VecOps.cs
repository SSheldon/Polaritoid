using System;
using Microsoft.Xna.Framework;

static class VecOps
{
    #region Vector Creators
    /// <summary>
    /// Returns a Vector2 containing the 2D Cartesian coordinates of a vector specified in polar notation.
    /// </summary>
    /// <param name="length">Length of the Vector2.</param>
    /// <param name="direction">Direction of the Vector2, measured in terms of the angle, in radians, measured counterclockwise from the positive x-axis to the vector.</param>
    public static Vector2 Polar(float length, float direction)
    {
        return new Vector2(length * (float)Math.Cos((double)direction), length * (float)Math.Sin((double)direction));
    }

    /// <summary>
    /// Returns a Vector3 containing the 3D Cartesian coordinates of a vector specified in cylindrical notation.
    /// </summary>
    public static Vector3 Cylindrical(float radius, float azimuth, float height)
    {
        return new Vector3(Polar(radius, azimuth), height);
    }

    /// <summary>
    /// Returns a Vector3 containing the 3D Cartesian coordinates of a vector specified in spherical notation.
    /// </summary>
    public static Vector3 Spherical(float radius, float azimuth, float zenith)
    {
        return new Vector3(
            radius * (float)Math.Sin((double)zenith) * (float)Math.Cos((double)azimuth),
            radius * (float)Math.Sin((double)zenith) * (float)Math.Sin((double)azimuth),
            radius * (float)Math.Cos((double)zenith));
    }
    #endregion

    /// <summary>
    /// Calculates the triple product of three Vector3s.
    /// </summary>
    public static float TripleProduct(Vector3 value1, Vector3 value2, Vector3 value3)
    {
        return Vector3.Dot(value1, Vector3.Cross(value2, value3));
    }

    /// <summary>
    /// Calculates the vector that results when a vector is projected onto another. (the vector component of one vector along another)
    /// </summary>
    /// <param name="projectedVector">The vector that is being projected.</param>
    /// <param name="vectorProjectedOnto">The vector that is being projected onto.</param>
    public static Vector3 Projection(Vector3 projectedVector, Vector3 vectorProjectedOnto)
    {
        return (Vector3.Dot(projectedVector, vectorProjectedOnto) / vectorProjectedOnto.LengthSquared()) * vectorProjectedOnto;
    }

    /// <summary>
    /// Calculates the vector that results when a vector is projected onto another. (the vector component of one vector along another)
    /// </summary>
    /// <param name="projectedVector">The vector that is being projected.</param>
    /// <param name="vectorProjectedOnto">The vector that is being projected onto.</param>
    public static Vector2 Projection(Vector2 projectedVector, Vector2 vectorProjectedOnto)
    {
        return (Vector2.Dot(projectedVector, vectorProjectedOnto) / vectorProjectedOnto.LengthSquared()) * vectorProjectedOnto;
    }

    #region Angle Methods
    /// <summary>
    /// Calculates the angle, measured in radians, between two vectors.
    /// </summary>
    public static float AngleBetween(Vector3 value1, Vector3 value2)
    {
        float a = Vector3.Dot(value1, value2) / (value1.Length() * value2.Length());
        return (float)Math.Acos((double)MathHelper.Clamp(a, -1F, 1F));
    }

    /// <summary>
    /// Calculates the angle, measured in radians, between two vectors.
    /// </summary>
    public static float AngleBetween(Vector2 value1, Vector2 value2)
    {
        float a = Vector2.Dot(value1, value2) / (value1.Length() * value2.Length());
        return (float)Math.Acos((double)MathHelper.Clamp(a, -1F, 1F));
    }

    /// <summary>
    /// Calculates the direction of a Vector2, measured in terms of the angle, in radians, measured counterclockwise from the positive x-axis to the Vector2.
    /// </summary>
    /// <param name="vec">Vector2 whose direction is to be calculated.</param>
    public static float Direction(Vector2 value)
    {
        return (float)Math.Atan2((double)value.Y, (double)value.X);
    }
    #endregion
}