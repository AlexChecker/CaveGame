using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace CaveGame.Core.Classes.Items
{
    public class Inventory : OffsetRenderer
    {

        private ItemStack[] contents;
        private int size;
        private int x, y;
        private int sx = 0, sy = 0;
        public bool inventoryFocus = false;

        public Inventory(int x, int y)
        {
            size = x * y;
            this.x = x;
            this.y = y;
            contents = new ItemStack[size];
        }

        public ItemStack? getSelected()
        {
            var current = contents[sy * y + sx];
            return current;
        }

        public void clearNulls()
        {
            var toDelete = new List<int>();
            var i = 0;
            foreach (ItemStack stack in Contents)
            {
                if (stack == null)
                {
                    i++;
                    continue;
                }
                if (stack.Amount <= 0)
                {
                    toDelete.Add(i);
                }
                i++;
            }

            foreach (int toDel in toDelete)
            {
                contents[toDel] = null;
            }
        }

        public ItemStack getItemByBase(Item item)
        {
            foreach (var stack in Contents)
            {
                if (stack == null) continue;
                if (item.Equals(stack.item))
                {
                    return stack;
                }
            }

            return null;
        }

        public void controlUpdate(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.E:
                    inventoryFocus = !inventoryFocus;
                    break;
                case ConsoleKey.Enter:
                    var cur = getSelected();
                    if (cur != null)
                    {
                        cur.onUse();
                        clearNulls();
                    }
                    inventoryFocus = false;
                    break;
                case ConsoleKey.RightArrow:
                    sx++;
                    if (sx >= x)
                    {
                        sx = x-1;
                    }

                    if (sx < 0)
                    {
                        sx = 0;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    sx--;
                    if (sx >= x)
                    {
                        sx = x-1;
                    }

                    if (sx < 0)
                    {
                        sx = 0;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    sy--;
                    if (sy >= y)
                    {
                        sy = y-1;
                    }

                    if (sy < 0)
                    {
                        sy = 0;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    sy++;
                    if (sy >= y)
                    {
                        sy = y-1;
                    }

                    if (sy < 0)
                    {
                        sy = 0;
                    }
                    break;
            }
        }

        public ItemStack? getItem(int i)
        {
            return contents[i];
        }

        public bool hasFreeSlot(ItemStack toAdd)
        {
            return getFreeSlot(toAdd) != -1;
        }

        public int getFreeSlot(ItemStack toAdd)
        {
            for (int i = 0; i < size; i++)
            {
                var current = getItem(i);
                if (current == null)
                {
                    return i;
                }
                if (current.isStackable(toAdd))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool addItemSlot(ItemStack stack)
        {
            var toAdd = getFreeSlot(stack);
            var current = getItem(toAdd);
            if (current == null)
            {
                setItem(stack, toAdd);
                return true;
            }
            else
            {
                current.stack(stack);
            }

            return false;
        }
        
        public void setItem(ItemStack stack, int i)
        {
            contents[i] = stack;
        }

        public ItemStack[] Contents => contents;
        public void render(Window win, Point origin)
        {
            var originMod = origin;
            //originMod.x -= Program.WIDTH/2;
            //originMod.y -= Program.HEIGHT/2;
            win.drawBox(originMod, originMod + new Point(x*8+1, y*8+1));
            win.drawString(originMod + new Point(1, 0), "Инвентарь");
            var xo = 0;
            var yo = 0;
            var maxx = x;
            foreach (ItemStack stack in Contents)
            {
                if (xo == sx && yo == sy)
                {
                    win.drawBox(new Point(originMod.x + xo*8 + 1, originMod.y + yo*8 + 1), new Point(originMod.x + xo*8 + 8, originMod.y + yo*8 + 8));
                }
                if (stack != null)
                {
                    stack.render(win, new Point(originMod.x + xo*8 + 1, originMod.y + yo*8 + 1));
                }

                xo++;
                if (xo >= maxx)
                {
                    yo++;
                    xo = 0;
                }
            }
        }
    }
}