using System;

namespace TestTask
{
    class Capsule
    {
        private readonly Vector3 a, b;
        private readonly double radius;

        /// <summary>
        /// Construct a capsule defined by endpoints a and b and the radius.
        /// </summary>
        public Capsule(Vector3 start, Vector3 end, double radius)
        {
            a = start; b = end; this.radius = radius;
        }

        /// <summary>
        /// Compute squared distance from a point to the line segment [a,b].
        /// </summary>
        private double DistanceSquared(Vector3 p)
        {
            Vector3 ab = b - a;
            Vector3 ap = p - a;

            double denom = ab.LengthSquared();
            if (denom < 1e-18)
                return ap.LengthSquared(); // Degenerate case: a ≈ b

            double t = ap.Dot(ab) / denom;
            t = Math.Max(0.0, Math.Min(1.0, t));

            Vector3 closest = a + t * ab;
            return (p - closest).LengthSquared();
        }

        /// <summary>
        /// Check whether a given point lies inside or on the capsule.
        /// </summary>
        public bool Contains(Vector3 p) =>
            DistanceSquared(p) <= radius * radius;
    }
}
