namespace LoveCheckers.Models
{
    // a nice way to wrap up X and Y values into one class
    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point()
        {
            
        }
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            Point b = (Point) obj;
            return (X == b.X && Y == b.Y);
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
    }
}