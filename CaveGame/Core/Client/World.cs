using System;
using System.Collections.Generic;
using CaveGame.Core.Classes;
using CaveGame.Core.Classes.Entities;
using CaveGame.Core.Classes.Map;
using CaveGame.Core.Classes.Registry;
using CaveGame.Core.Classes.Tiles;

namespace CaveGame.Core.Client
{
    public class World : Renderer
    {

        public TileGrid _tileGrid;
        public List<Entity> entities = new();
        public Location spawnLocation;
        public string tag;
        public MapBuilder builder;
        public Client client;


        public World(Client client)
        {
            spawnLocation = new Location(0, 0, this);
            this._tileGrid = new TileGrid();
            this.client = client;
            builder = new MapBuilder(this);
            builder.readText($@"Maps\{tag}.map");
        }
        
        public World(Client client, string tag)
        {
            this.tag = tag;
            spawnLocation = new Location(0, 0, this);
            _tileGrid = new TileGrid();
            this.client = client;
            loadMap();
        }

        public void loadMap()
        {
            if (builder != null)
            {
                builder.clearBases();
            }
            builder = new MapBuilder(this);
            builder.readText($@"Maps\{tag}.map");
            spawnLocation = getSpawnTileLocation();
        }

        public void spawnEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void removeEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        //public void loadMap(string mapname)
        //{
        //    //builder = new MapBuilder(this);
        //    Inventory backup = getPlayer().inventory;
        //    int backuphp = getPlayer().hp;
        //    int backupmana = getPlayer().mana;
        //    clearEntities();
        //    _tileGrid.clear();
        //    builder.readText($@"Maps\{mapname}");
        //    getPlayer().inventory = backup;
        //    getPlayer().hp = backuphp;
        //    getPlayer().mana = backupmana;
        //}

        public Player getPlayer()
        {
            foreach (var ent in entities)
            {
                if (ent is Player)
                {
                    return (Player) ent;
                }
            }

            return null;
        }

        public void summonEntity(EntityBase entity, Location location)
        {
            summonEntity(entity.create(location));
        }
        
        public void summonEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void clearEntities()
        {
            entities.Clear();
        }

        public Entity getEntityAt(Location location)
        {
            foreach (var entity in entities)
            {
                if (entity._location.Equals(location))
                {
                    return entity;
                }
            }

            return null;
        }

        public void render(Window win)
        {
            //for (int i = 0; i < Program.WIDTH; i++)
            //{
            //    for (int j = 0; j < Program.HEIGHT; j++)
            //    {
            //        var px = new Pixel(' ');
            //        px.bColor = ConsoleColor.Gray;
            //        win.drawDot(new Point(i, j), px);
            //    }
            //}
            _tileGrid.render(win);
            foreach (var entity in entities)
            {
                entity.render(win);
            }
            Console.SetCursorPosition(0,0);
        }

        public Location getSpawnTileLocation()
        {
            foreach (Tile tile in _tileGrid.tiles)
            {
                if (tile.GBase.Equals(TileRegistry.SPAWN))
                {
                    return new Location(tile.Location);
                }
            }

            return null;
        }

        public TileGrid TileGrid => _tileGrid;
    }
}