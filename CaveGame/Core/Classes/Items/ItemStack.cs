using CaveGame.Core.Classes.Entities;

namespace CaveGame.Core.Classes.Items
{
    public class ItemStack : OffsetRenderer
    {

        public Item item;
        public int amount;

        public ItemStack(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        public void onUse()
        {
            item.onUse(Program.client._localWorld.getPlayer(), this);
        }

        public int tryStack(ItemStack other)
        {
            if (isStackable(other)){
                stack(other);
                return 0;
            }else {
                var diff = (other.amount + amount) - item.maxStackSize;
                amount = item.maxStackSize;
                return diff;
            }
        }

        public void stack(ItemStack stack)
        {
            if (isStackable(stack)) {
                amount += stack.amount;
                stack.amount = 0;
            }
        }

        public bool isStackable(int amount)
        {
            return this.amount + amount <= item.maxStackSize;
        }
        
        public bool isStackable(ItemStack stack)
        {
            if (isSimilar(stack))
            {
                return isStackable(stack.amount);
            }
            return false;
        }
        
        public bool isSimilar(ItemStack other)
        {
            return other.item.Equals(this.item);
        }

        public int Amount
        {
            get => amount;
            set
            {
                var toSet = value;
                if (toSet > item.maxStackSize)
                {
                    toSet = item.maxStackSize;
                }

                if (toSet < 0)
                {
                    toSet = 0;
                }
                amount = toSet;
            }
        }

        public void render(Window win, Point origin)
        {
            item.texture.render(win, origin);
            win.drawString(origin, item.Name);
            win.drawString(origin + new Point(0, 7), $"x{amount}");
        }
    }
}