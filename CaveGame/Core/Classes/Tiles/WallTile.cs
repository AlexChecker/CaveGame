namespace CaveGame.Core.Classes.Tiles
{
    public class WallTile : Tile
    {
        public WallTile(WallTileBase b, Location location) : base(b, location)
        {
        }

        public char getPixel(bool up, bool down, bool left, bool right, WallTileBase b)
        {
            if (up && left && right && down) return b.tiles[0];
            if (up && down && left) return  b.tiles[1];
            if (up && down && right) return  b.tiles[2];
            if (up && left && right) return b.tiles[3];
            if (down && left && right) return b.tiles[4];
            if (up && down) return b.tiles[5];
            if (up && left) return b.tiles[6];
            if (up && right) return b.tiles[7];
            if (down && left) return b.tiles[8];
            if (down && right) return b.tiles[9];
            if (left && right) return b.tiles[10];
            if (up) return b.tiles[11];
            if (down) return b.tiles[12];
            if (left) return b.tiles[14];
            if (right) return b.tiles[13];
            return 'X';
        }

        public override void render(Window win)
        {
            var world = Location.world;
            var b = GBase as WallTileBase;
            var upTile = world._tileGrid.getTileAt(Location + new Location(0, -1, world));
            var downTile = world._tileGrid.getTileAt(Location + new Location(0, 1, world));
            var leftTile = world._tileGrid.getTileAt(Location + new Location(-1, 0, world));
            var rightTile = world._tileGrid.getTileAt(Location + new Location(1, 0, world));
            var up = upTile != null;
            var down = downTile != null;
            var left = leftTile != null;
            var right = rightTile != null;
            if (up)
            {
                up = upTile.GBase.enableStacking;
            }
            if (down)
            {
                down = downTile.GBase.enableStacking;
            }
            if (left)
            {
                left = leftTile.GBase.enableStacking;
            }
            if (right)
            {
                right = rightTile.GBase.enableStacking;
            }
            var curChar = ' ';
            win.drawDot(Location.toPoint(), new Pixel(getPixel(up, down, left, right, b)));
        }//デロル
    }

    public class WallTileBase : TileBase
    {

        public string tiles = "╬╣╠╩╦║╝╚╗╔═╨╥╞╡";
        
        public WallTileBase(string name, string tag, char drawChar) : base(name, tag, drawChar)
        {
            enableStack();
        }

        public override Tile createCopy(Location location)
        {
            return new WallTile(this, location);
        }
    }
}