using System;

namespace TestTask
{
    class ArcFunction : DiscreteFunction
    {
        private double radius;

        public ArcFunction(double t1, double t2, double arcRad)
            : base(t1, t2)
        {
            this.radius = arcRad;
        }

        public override Point3d<double> Evaluate(double t)
        {
            base.Evaluate(t);

            Point3d<double> point = new Point3d<double>();

            point.X = 500.0 + radius * Math.Sin(Math.PI * 2 * t);
            point.Y = 250.0 + radius * Math.Cos(Math.PI * 2 * t);
            point.Z = 100.0;

            return point;
        }
    }
}
