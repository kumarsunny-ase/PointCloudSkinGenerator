using System;

namespace TestTask
{
    class DiscreteFunction
    {
        private double tBegin;
        private double tEnd;

        public DiscreteFunction(double t1, double t2)
        {
            this.tBegin = t1;
            this.tEnd = t2;
        }

        public virtual Point3d<double> Evaluate(double t)
        {
            if (t < tBegin || t > tEnd)
                throw new ArgumentOutOfRangeException(nameof(t));

            return new Point3d<double>();
        }
    }
}
