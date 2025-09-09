using System;
using System.Globalization;

namespace TestTask
{
    public class Point3d<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }

        public Point3d()
        {
            this.X = default(T);
            this.Y = default(T);
            this.Z = default(T);
        }

        public Point3d(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3d(Point3d<T> other)
        {
            this.X = other.X;
            this.Y = other.Y;
            this.Z = other.Z;
        }

        public T this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.X;
                    case 1:
                        return this.Y;
                    case 2:
                        return this.Z;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.X = value;
                        return;
                    case 1:
                        this.Y = value;
                        return;
                    case 2:
                        this.Z = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override int GetHashCode()
        {
            if (typeof(T) == typeof(double))
            {
                Point3d<double> thisObj = (Point3d<double>)((object)this);
                return (int)(thisObj.X * 10000.0) * -7919 + (int)(thisObj.Y * 10000.0) * 4447 + (int)(thisObj.Z * 10000.0) * 6569;
            }
            else if (typeof(T) == typeof(float))
            {
                Point3d<float> thisObj = (Point3d<float>)((object)this);
                return (int)(thisObj.X * 10000.0) * -7919 + (int)(thisObj.Y * 10000.0) * 4447 + (int)(thisObj.Z * 10000.0) * 6569;
            }

            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (typeof(T) == typeof(double))
            {
                Point3d<double> thisObj = (Point3d<double>)((object)this);
                Point3d<double> otherObj = (Point3d<double>)obj;
                return Math.Abs(thisObj.X - otherObj.X) < 0.000001 &&
                    Math.Abs(thisObj.Y - otherObj.Y) < 0.000001 &&
                    Math.Abs(thisObj.Z - otherObj.Z) < 0.000001;
            }
            else if (typeof(T) == typeof(float))
            {
                Point3d<float> thisObj = (Point3d<float>)((object)this);
                Point3d<float> otherObj = (Point3d<float>)obj;
                return Math.Abs(thisObj.X - otherObj.X) < 0.0001 &&
                    Math.Abs(thisObj.Y - otherObj.Y) < 0.0001 &&
                    Math.Abs(thisObj.Z - otherObj.Z) < 0.0001;
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        public string ToString(CultureInfo culture)
        {
            return String.Format(culture, "{0:0.0000##############}; {1:0.0000##############}; {2:0.0000##############}", this.X, this.Y, this.Z);
        }
    }
}
