using System.Runtime.CompilerServices;
using System.Threading;
using CaveGame.Core.Client;

namespace CaveGame.Core.Classes
{
    public class Location
    {
        public int x, y;
        public World world;

        public Location(int x, int y, World world)
        {
            this.x = x;
            this.y = y;
            this.world = world;
        }

        public Location(Location loc)
        {
            this.x = loc.x;
            this.y = loc.y;
            this.world = loc.world;
        }

        public static Location operator +(Location f, Location f2)
        {
            return new Location(f.x + f2.x, f.y + f2.y, f.world);
        }
        
        public static Location operator -(Location f, Location f2)
        {
            return new Location(f.x - f2.x, f.y - f2.y, f.world);
        }
        
        public static Location operator *(Location f, Location f2)
        {
            return new Location(f.x * f2.x, f.y * f2.y, f.world);
        }

        public Point toPoint()
        {
            return new Point(x, y);
        }

        public Location asBlock()
        {
            return new Location(x * Texture.SIZE, y * Texture.SIZE, world);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Location)
            {
                var l = (Location) obj;
                return l.x == x && l.y == y && l.world.Equals(world);
            }
            return false;
        }
    }
}