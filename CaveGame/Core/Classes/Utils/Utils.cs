using System.Collections.Generic;
using CaveGame.Core.Classes.Items;

namespace CaveGame.Core.Classes.Utils
{
    public class Utils
    {
        
    }

    public class Direction
    {
        public static List<Direction> around = new List<Direction>();

        static Direction()
        {
            around.Add(UP);            
            around.Add(DOWN);            
            around.Add(RIGHT);            
            around.Add(LEFT);            
        }
        
        public static Direction UP = new Direction(new Point(0, 1));
        public static Direction DOWN = new Direction(new Point(0, -1));
        public static Direction RIGHT = new Direction(new Point(1, 0));
        public static Direction LEFT = new Direction(new Point(-1, 0));
        public Point offset;

        public Direction(Point offset)
        {
            this.offset = offset;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Direction)
            {
                var dir = (Direction) obj;
                return dir.offset == this.offset;
            }

            return false;
        }
    }
}