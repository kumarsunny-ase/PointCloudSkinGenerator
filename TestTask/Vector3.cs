using System;

namespace TestTask
{
    struct Vector3
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Vector3(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        public static Vector3 operator +(Vector3 a, Vector3 b) =>
            new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <summary>
        /// Subtract vector b from vector a.
        /// </summary>
        public static Vector3 operator -(Vector3 a, Vector3 b) =>
            new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <summary>
        /// Multiply vector by a scalar.
        /// </summary>
        public static Vector3 operator *(double s, Vector3 v) =>
            new Vector3(s * v.X, s * v.Y, s * v.Z);

        /// <summary>
        /// Compute the dot product with another vector.
        /// </summary>
        public double Dot(Vector3 other) =>
            X * other.X + Y * other.Y + Z * other.Z;

        /// <summary>
        /// For the Cross product.
        /// </summary>
        public Vector3 Cross(Vector3 other) =>
            new Vector3(
                Y * other.Z - Z * other.Y,
                Z * other.X - X * other.Z,
                X * other.Y - Y * other.X
            );

        /// <summary>
        /// Compute the squared length of the vector.
        /// </summary>
        public double LengthSquared() => Dot(this);
        public double Length() => Math.Sqrt(LengthSquared());
    }
}
