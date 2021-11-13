using System;
using System.Collections.Generic;
using CaveGame.Core.Classes.Entities;
using CaveGame.Core.Classes.Utils;
using CaveGame.Core.Client;

namespace CaveGame.Core.Classes.Tiles
{
    public class Tile : GameObject
    {
        private Location _tileLoc;

        public List<Tile> TilesAround
        {
            get
            {
                var tiles = new List<Tile>();
                foreach (var aro in Direction.around)
                {
                    tiles.Add(getRelative(aro.offset.toLocation(getWorld())));
                }
                return tiles;
            }
        }

        public Tile(TileBase b, Location location) : base(b, location)
        {
            _tileLoc = location;
        }

        public virtual bool isTransient()
        {
            return GBase.isTransient;
        }

        public virtual void onPlayerOverlap(Player p)
        {
            
        }
        
        public override void render(Window win)
        {
            GBase.Renderer.render(win, Location.toPoint());
        }

        public Tile getRelative(Location offset)
        {
            if (offset.world != getWorld())
            {
                throw new ArgumentException("World not equals");
            }
            return getWorld().TileGrid.getTileAt(_tileLoc + offset);
        }

        public void removeFromWorld()
        {
            getWorld()._tileGrid.remove(this);
        }

        public World getWorld()
        {
            return Location.world;
        }

        public Location TileLoc => _tileLoc;

        public override TileBase GBase => (TileBase) base.GBase;
    }
}