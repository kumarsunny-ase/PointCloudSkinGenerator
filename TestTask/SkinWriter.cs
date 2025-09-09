using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace TestTask
{
    /// <summary>
    /// Utility class to write 3D points to a text file in ASCII format.
    /// </summary>
    class SkinWriter
    {
        /// <summary>
        /// Write points to file as lines of "x y z" (invariant culture, high precision).
        /// </summary>
        public static void Write(string fileName, IEnumerable<Vector3> points)
        {
            using (var w = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                var ci = CultureInfo.InvariantCulture;
                foreach (var p in points)
                {
                    w.WriteLine($"{p.X:G17} {p.Y:G17} {p.Z:G17}");
                }
            }
        }
    }
}
