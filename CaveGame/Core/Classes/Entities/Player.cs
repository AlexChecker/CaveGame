using System;
using System.Collections.Generic;
using CaveGame.Core.Classes.Items;
using CaveGame.Core.Classes.Registry;

namespace CaveGame.Core.Classes.Entities
{

    public class PlayerBase : EntityBase
    {
        public PlayerBase() : base("Player", '§', 100,100)
        {
            setTexture(new Texture('P'));
        }

        public override Entity create(Location location)
        {
            return new Player(location, this);
        }
    }

    public class Player : Entity
    {

        public static Random random = new Random();
        public Inventory inventory;
        public bool noclip = false;

        public Player(Location location, EntityBase @base) : base(location, @base)
        {
            inventory = new Inventory(9, 5);
        }

        public void teleport(Location location)
        {
            _location.world.entities.Remove(this);
            _location = location;
            location.world.entities.Add(this);
        }

        public override void move(int mx, int my, bool noclip = false)
        {
            if (random.Next(0, 10000) < _location.world.client.diff.Chance&&!_location.world.client.nobattle)
            {
                _location.world.client.runBattle();
            }
            base.move(mx, my, noclip);
        }

        //public Player Clone()
        //{
        //    //PlayerBase
        //}

        public void screenSync()
        {
            int px = -_location.toPoint().x;
            int py = -_location.toPoint().y;
            Window.offset.x = px + Program.WIDTH / 2;
            Window.offset.y = py + Program.HEIGHT / 2;
        }
        
        public void controlUpdate(ConsoleKey key)
        {
            if (inventory.inventoryFocus)
            {
                inventory.controlUpdate(key);
                return;
            }
            if (key == ConsoleKey.E)
            {
                inventory.inventoryFocus = !inventory.inventoryFocus;
                return;
            }

            //bool skillused = false;
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    move(0, -1,noclip);
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    move(0, 1,noclip);
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    move(-1, 0,noclip);
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    move(1, 0,noclip);
                    break;
            }

            screenSync();
        }
    }
}