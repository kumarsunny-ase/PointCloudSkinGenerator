namespace TestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //Point cloud reference point at 0, 0, 0
            Point3d<double> referencePoint = new Point3d<double>(0.0, 0.0, 0.0);

            //Number of points in x direction
            int nx = 1000;

            //Number of points in y direction
            int ny = 500;

            //Number of points in z direction
            int nz = 100;

            //Distance between points in the point grid (same for x, y and z directions)
            double delta = 1.0;

            //Discrete step for move function calculation
            double deltaT = 0.01;

            //Radius of the sphere
            double sphereRad = 5.0;

            //Name of the file to write the skin data to
            string skinFileName = "C:\\Mac\\Home\\Downloads\\TestDataresult\\new\\Result.asc";

            //Function object to be evaluated
            ArcFunction func = new ArcFunction(0.0, 1.0, 150.0);

            //Evaluation here
            Solver.CreateSkin(referencePoint, nx, ny, nz, sphereRad, func, deltaT, delta, skinFileName);
        }
    }
}