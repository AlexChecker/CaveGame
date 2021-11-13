using System;
using System.Collections.Generic;
using CaveGame.Core.Classes.Dialogs;
using CaveGame.Core.Classes.Entities;
using CaveGame.Core.Classes.Items;
using CaveGame.Core.Classes.Tiles;


namespace CaveGame.Core.Classes.Registry
{
    public class TileRegistry
    {

        public static List<TileBase> bases = new ();

        public static TileBase STONE = new TileBase("Камень", "stone", 'S').setTexture(new Texture('#'));

        public static WallTileBase WALL = new WallTileBase("Стена", "wall", '#');
        public static TileBase GRASS = new TileBase("Трава", "",'`').enableTransient().setTexture(new Texture('`'));
        public static TileBase WATER = new TileBase("Вода", "", '~').setTexture(new Texture('~'));
        public static TileBase SPAWN = new TileBase("Спавн", "spawn", '@').enableTransient().setTexture(new Texture(' '));
        static TileRegistry()
        {
			register(STONE);
            register(WALL);
            register(GRASS);
            register(WATER);
            register(SPAWN);
        }

        public static TileBase getByChar(char i)
        {
            foreach (var b in bases)
            {
                if (b.DrawChar == i)
                {
                    return b;
                }
            }
            return null;
        }

        public static void register(TileBase b)
        {
            bases.Add(b);
        }
    }

    public class EntityRegistry
    {

        public static List<EntityBase> bases = new();
		
		public static EntityBase GLITCH_1 = new EntityBase("Глюк",  '1', 20,10).setTexture(new Texture((""+(char)220)[0]));
		public static EntityBase GLITCH_2 = new EntityBase("Глюк",  '2', 40,10).setTexture(new Texture((""+(char)221)[0]));
		public static EntityBase GLITCH_3 = new EntityBase("Глюк",  '3', 60,10).setTexture(new Texture((""+(char)222)[0]));
		public static EntityBase GLITCH_4 = new EntityBase("Глюк",  '4', 100,10).setTexture(new Texture((""+(char)223)[0]));
        public static PlayerBase PLAYER = new ();
		
        static EntityRegistry()
        {
            register(GLITCH_1);
            register(GLITCH_2);
            register(GLITCH_3);
            register(GLITCH_4);
            register(PLAYER);
        }

        public static void register(EntityBase b)
        {
         bases.Add(b);   
        }

        public static EntityBase getByChar(char c)
        {
            foreach (var b in bases)
            {
                if (b.curchar == c)
                {
                    return b;
                }  
            }

            return null;
        }
    }

    public class ItemRegistry
    {

        
        
        public static List<Item> itemBases = new ();

        public static Item SWORD = new ("Меч", "sword", "sword");

        public static Potion POTION = new ("Хилка", "potion", "potion");

        public static ManaCrystal MANA = new("Кристалл","crystal","manacrystal");

        public static KeyItem KEY = new("КлючеГ","key","key");
        
        public class KeyItem : Item
        {
            public KeyItem(string name, string tag, string texturePath) : base(name, tag, texturePath)
            {
            }
            public override void onUse(Player p, ItemStack item)
            {
                
            }
        }
        public class ManaCrystal : Item
        {
            public ManaCrystal(string name, string tag, string texturePath) : base(name, tag, texturePath)
            {
            }
            public override void onUse(Player p, ItemStack item)
            {
                item.Amount -= 1;
                p.mana += 10;
            }
        }

        public class Potion : Item
        {
            public Potion(string name, string tag, string texturePath) : base(name, tag, texturePath)
            {
            }

            public override void onUse(Player p, ItemStack item)
            {
                item.Amount -= 1;
                p.hp += 10;
            }
        }
        
        static ItemRegistry()
        {
            SWORD.maxStackSize = 1;
            register(SWORD);
            POTION.maxStackSize = 10;
            register(POTION);
            MANA.maxStackSize = 10;
            register(MANA);
            KEY.maxStackSize = 5;
            register(KEY);
        }

        public static Item getType(string tag)
        {
            foreach (Item item in itemBases)
            {
                if (item.Tag == tag)
                {
                    return item;
                }
            }

            return null;
        }
        
        public static void register(Item item)
        {
            itemBases.Add(item);
        }
    }
}