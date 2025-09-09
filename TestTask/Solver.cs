using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestTask
{
    class Solver
    {
        public static void CreateSkin(Point3d<double> refPoint,
                int nx, int ny, int nz, double sphereRad,
                DiscreteFunction func, double deltaT,
                double delta, string skinFileName)
        {
            // --- Validation ---
            if (nx <= 0 || ny <= 0 || nz <= 0) throw new ArgumentException("Grid sizes must be positive");
            if (delta <= 0.0) throw new ArgumentException("delta (grid spacing) must be > 0");
            if (sphereRad < 0.0) throw new ArgumentException("sphereRad must be >= 0");
            if (deltaT <= 0.0) throw new ArgumentException("deltaT must be > 0");
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (string.IsNullOrEmpty(skinFileName)) throw new ArgumentNullException(nameof(skinFileName));

            // --- Convert reference point to Vector3 ---
            Vector3 refVec = new Vector3(refPoint.X, refPoint.Y, refPoint.Z);

            // --- Time range ---
            (double startT, double endT) = GetTimeRange(func);

            // --- Deletion mask (true = deleted) ---
            var deleted = new bool[nx * ny * nz];

            // --- Perform sphere sweep and delete points ---
            DeletePointsAlongPath(refVec, nx, ny, nz, delta, sphereRad, func, deltaT, startT, endT, deleted);

            // --- Extract top visible layer ---
            var topPoints = ExtractTopSkin(refVec, nx, ny, nz, delta, deleted);

            // --- Write results to file ---
            SkinWriter.Write(skinFileName, topPoints);
        }

        // ----------------- Helpers -----------------

        /// <summary>
        /// Use reflection to obtain the time range [tBegin, tEnd] from a DiscreteFunction.
        /// </summary>
        private static (double, double) GetTimeRange(DiscreteFunction f)
        {
            Type cur = f.GetType();
            FieldInfo fiBegin = null, fiEnd = null;

            while (cur != null)
            {
                fiBegin = cur.GetField("tBegin", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                fiEnd = cur.GetField("tEnd", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (fiBegin != null && fiEnd != null) break;
                cur = cur.BaseType;
            }

            if (fiBegin == null || fiEnd == null)
                throw new InvalidOperationException("Could not find tBegin/tEnd in DiscreteFunction.");

            double b = (double)fiBegin.GetValue(f);
            double e = (double)fiEnd.GetValue(f);
            if (e < b) throw new InvalidOperationException("Invalid time range: tEnd < tBegin");
            return (b, e);
        }

        /// <summary>
        /// Perform discrete time stepping and mark deleted points in the grid.
        /// </summary>
        private static void DeletePointsAlongPath(
            Vector3 refPoint,
            int nx, int ny, int nz,
            double delta,
            double sphereRad,
            DiscreteFunction func,
            double deltaT,
            double startT, double endT,
            bool[] deleted)
        {
            int Idx(int i, int j, int k) => (k * ny + j) * nx + i;

            double t = startT;
            while (t < endT - 1e-15)
            {
                double tNext = Math.Min(t + deltaT, endT);

                Vector3 a = ToVec(func.Evaluate(t));
                Vector3 b = ToVec(func.Evaluate(tNext));

                var capsule = new Capsule(a, b, sphereRad);

                // Bounding box
                double minX = Math.Min(a.X, b.X) - sphereRad;
                double minY = Math.Min(a.Y, b.Y) - sphereRad;
                double minZ = Math.Min(a.Z, b.Z) - sphereRad;
                double maxX = Math.Max(a.X, b.X) + sphereRad;
                double maxY = Math.Max(a.Y, b.Y) + sphereRad;
                double maxZ = Math.Max(a.Z, b.Z) + sphereRad;

                // Index ranges
                int ix0 = Math.Max(0, (int)Math.Ceiling((minX - refPoint.X) / delta));
                int iy0 = Math.Max(0, (int)Math.Ceiling((minY - refPoint.Y) / delta));
                int iz0 = Math.Max(0, (int)Math.Ceiling((minZ - refPoint.Z) / delta));

                int ix1 = Math.Min(nx - 1, (int)Math.Floor((maxX - refPoint.X) / delta));
                int iy1 = Math.Min(ny - 1, (int)Math.Floor((maxY - refPoint.Y) / delta));
                int iz1 = Math.Min(nz - 1, (int)Math.Floor((maxZ - refPoint.Z) / delta));

                // Mark points inside capsule as deleted
                for (int i = ix0; i <= ix1; i++)
                {
                    for (int j = iy0; j <= iy1; j++)
                    {
                        for (int k = iz0; k <= iz1; k++)
                        {
                            int idx = Idx(i, j, k);
                            if (deleted[idx]) continue;

                            var p = new Vector3(
                                refPoint.X + i * delta,
                                refPoint.Y + j * delta,
                                refPoint.Z + k * delta);

                            if (capsule.Contains(p))
                                deleted[idx] = true;
                        }
                    }
                }

                if (tNext == t) break;
                t = tNext;
            }
        }

        /// <summary>
        /// Extract topmost visible layer (skin) of undeleted points from the grid.
        /// </summary>
        private static List<Vector3> ExtractTopSkin(
             Vector3 refPoint,
             int nx, int ny, int nz,
             double delta,
             bool[] deleted)
        {
            int Idx(int i, int j, int k) => (k * ny + j) * nx + i;

            var topPoints = new List<Vector3>(nx * ny);
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    for (int k = nz - 1; k >= 0; k--)
                    {
                        if (!deleted[Idx(i, j, k)])
                        {
                            topPoints.Add(new Vector3(
                                refPoint.X + i * delta,
                                refPoint.Y + j * delta,
                                refPoint.Z + k * delta));
                            break;
                        }
                    }
                }
            }
            return topPoints;
        }


        /// <summary>
        ///  Convert Point3d<double> to Vector3.
        /// </summary>
        private static Vector3 ToVec(Point3d<double> p) =>
            new Vector3(p.X, p.Y, p.Z);


    }
}