using CaveGame.Core.Classes.Entities;

namespace CaveGame.Core.Classes.Items
{
    public class Item
    {
        private string name;
        private string tag;
        public int maxStackSize = 64;
        public ItemTexture texture = new ();

        public Item(string name, string tag, string texturePath)
        {
            this.name = name;
            this.tag = tag;
            texture.loadFromFile("items/"+texturePath+".txt");
        }

        public virtual void onUse(Player p, ItemStack item)
        {
            
        }

        public ItemStack createStack(int amount)
        {
            return new ItemStack(this, amount);
        }

        public string Name => name;

        public string Tag => tag;

        public override bool Equals(object? obj)
        {
            if (obj is Item)
            {
                return ((Item) obj).tag == tag;
            }

            return false;
        }
    }
}