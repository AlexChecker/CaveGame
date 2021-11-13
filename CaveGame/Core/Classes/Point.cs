

using CaveGame.Core.Client;

namespace CaveGame.Core.Classes
{
    public class Point
    {
        public int x;
        public int y;
        /// <summary>
        /// Просто точка
        /// </summary>
        /// <param name="x">Расстояние от левого края</param>
        /// <param name="y">Расстояние от верхнего края</param>
        public Point(int x,int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(Point boxStart)
        {
            this.x = boxStart.x;
            this.y = boxStart.y;
        }

        public Location toLocation(World world)
        {
            return new Location(x, y, world);
        }

        public Point lerp(Point o, float p)
        {
            return new Point(Window.lerp(x, o.x, p), Window.lerp(y, o.y, p));
        }

        public static Point operator +(Point f, Point f2)
        {
            return new Point(f.x + f2.x, f.y + f2.y);
        }
        public static Point operator -(Point f, Point f2)
        {
            return new Point(f.x - f2.x, f.y - f2.y);
        }


        public override bool Equals(object? obj)
        {
            if (obj is Point)
            {
                var p = (Point) obj;
                return p.x == this.x && p.y == this.y;
            }
            return false;
        }
    }
}