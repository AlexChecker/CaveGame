using System;
using System.IO;
using System.Threading;
using CaveGame.Core.Classes;
using CaveGame.Core.Classes.Entities;
using CaveGame.Core.Classes.Registry;

namespace CaveGame.Core.Client
{
    public class Commands
    {
        public static void execute(string command,Window _window,Player p)
        {
            
            command = command.Trim('/');
            string[] args = command.Split(' ');
            switch (args[0])
                {
                    case "give":
                        if (args.Length < 3 || args.Length > 3)
                        {
                            _window.drawString(new Point(0,0),"incorrect command");
                        }
                        else
                        {
                            int count = 0;
                            try
                            {
                                count = Convert.ToInt32(args[2]);
                            }
                            catch (Exception e)
                            {
                                _window.drawString(new Point(0, 0), "incorrect command");
                                break;
                            }

                            var item = ItemRegistry.getType(args[1]).createStack(count);
                            item.Amount = item.Amount;
                            p.inventory.addItemSlot(item);
                        }

                        break;
                    case "tp":
                        int x = 0;
                        int y = 0;
                        try
                        {
                            x = Convert.ToInt32(args[1]);
                            y = Convert.ToInt32(args[2]);
                        }
                        catch
                        {
                            _window.drawString(new Point(0,0),"incorret syntax");
                            break;
                        }
                        //p.move(x,y);
                        p._location.x = x;
                        p._location.y = y;
                        break;
                    case "kill":
                        p.damage(p.hp);
                        break;
                    case  "infmana":
                        p.mana = 999999999;
                        break;
                    case "god":
                        p.hp = 999999999;
                        break;
                    case "noclip":
                        p.noclip = !p.noclip;
                        break;
                    case "map":
                        Program.client.loadMap(args[1]);
                        Program.client.changeWorld(args[1]);
                        break;
                }
            
        }
    }
}