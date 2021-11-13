using System.Collections.Generic;
#nullable enable
namespace CaveGame.Core.Classes.Tiles
{
    public class TileGrid : Renderer
    {
        public List<Tile> tiles = new ();

        public void setTile(TileBase? b, Location location)
        {
            var current = getTileAt(location);
            if (current != null)
            {
                remove(current);
            }
            if (b == null)
            {
                removeTile(location);
            }
            else
            {
                add(b.createCopy(location));
            }
        }

        public void removeTile(Location location)
        {
            var cur = getTileAt(location);
            if (cur == null) return;
            remove(cur);
        }

        private void add(Tile tile)
        {
            tiles.Add(tile);
        }

        public void clear()
        {
            tiles.Clear();
        }

        public void remove(Tile tile)
        {
            tiles.Remove(tile);
        }
        
        public Tile? getTileAt(Location location)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.TileLoc.Equals(location))
                {
                    return tile;
                }
            }

            return null;
        }

        public void render(Window win)
        {
            foreach (Tile tile in tiles)
            {
                tile.render(win);
            }
        }
    }
}