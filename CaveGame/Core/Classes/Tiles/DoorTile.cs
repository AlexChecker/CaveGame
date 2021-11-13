using CaveGame.Core.Classes.Registry;

namespace CaveGame.Core.Classes.Tiles
{

    public class DoorTileBase : TileBase
    {
        public DoorTileBase(string name, string tag, char texture, char drawChar) : base(name, tag, drawChar)
        {
            setTexture(new Texture(texture));
        }

        public override Tile createCopy(Location location)
        {
            return new DoorTile(this, location);
        }
    }
    
    public class DoorTile : Tile
    {
        public DoorTile(DoorTileBase b, Location location) : base(b, location)
        {
        }

        public override bool isTransient()
        {
            var p = Location.world.getPlayer();
            if (p == null) return false;
            var i = p.inventory.getItemByBase(ItemRegistry.KEY);
            var has = i != null;
            if (has)
            {
                i.Amount--;
                p.inventory.clearNulls();
            }
            return has;
        }
    }
}