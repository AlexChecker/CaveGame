using System;
using System.Collections.Generic;
using System.IO;
using CaveGame.Core.Classes.Dialogs;
using CaveGame.Core.Classes.Entities;
using CaveGame.Core.Classes.Registry;
using CaveGame.Core.Classes.Tiles;
using CaveGame.Core.Client;

namespace CaveGame.Core.Classes.Map
{
    public class MapBuilder
    {

        public List<Command> commands = new();
        public List<TileBase> triggerBases = new();
        public World world;

        public MapBuilder(World world)
        {
            this.world = world;
            //!onmap onrender tp x y
            commands.Add(new TpCommand("tp"));
            //!onmap onrender damage 200 
            commands.Add(new DamageCommand("damage"));
            commands.Add(new ItemGiveCommand("giveitem"));
            commands.Add(new DialogCommand("dialog"));
            commands.Add(new MapChangeCommand("map"));
            commands.Add(new RemoveManaCommand("manre"));
        }

        public void clearBases()
        {
            foreach (TileBase b in triggerBases)
            {
                TileRegistry.bases.Remove(b);
            }
        }

        public Command getCommand(string name)
        {
            foreach (Command cmd in commands)
            { 
                if (cmd.name == name)
                {
                    return cmd;
                }
            }

            return null;
        }

        //Æ
        public void readText(string path)
        {
            var y = 0;
            foreach(string line in File.ReadAllLines(path))
            {
                if (line.StartsWith("!"))
                {
                    string lineWi = line.Substring(1, line.Length - 1);
                    string[] argsRaw = lineWi.Split(": "); //trigger: A B dialog test
                    
                    var headerLine = argsRaw[1];
                    var argsraw = headerLine.Split(" ");
                    
                    switch (argsRaw[0])
                    {
                        case "trigger":
                            //_ tp args...
                            var characterRaw = argsraw[0];
                            var characterRawDraw = argsraw[1];
                            var commandRaw = argsraw[2];
                            var args = new string[line.Length-3];
                            for (int i = 3; i < argsraw.Length; i++)
                            {
                                args[i-3] = argsraw[i];
                            }

                            var command = getCommand(commandRaw);
                            if (command != null)
                            {
                                var b = new TriggerTileBase("Trigger", "trigger", characterRaw[0], characterRawDraw[0], args);
                                b.command = command;
                                b.isTransient = true;
                                triggerBases.Add(b);
                                TileRegistry.register(b);
                            }
                            break;
                            //string: <character> <string>
                        case "string":
                            var characterString = argsraw[0];
                            var bb = "";
                            for (int i = 1; i < argsraw.Length; i++)
                            {
                                bb += " "+argsraw[i];
                            }

                            var stringTileBase = new StringTileBase("String", "str", bb, characterString[0]);
                            triggerBases.Add(stringTileBase);
                            TileRegistry.register(stringTileBase);
                            break;
                        case "door":
                            var characterRawDoor = argsraw[0];
                            var characterRawDrawDoor = argsraw[1];
                            var doorBase = new DoorTileBase("Door", "door", characterRawDrawDoor[0],
                                characterRawDoor[0]);
                            triggerBases.Add(doorBase);
                            TileRegistry.register(doorBase);
                            break;
                        case "nobattle":
                            if(argsraw[0]=="1")
                                world.client.nobattle = true;
                            else 
                                world.client.nobattle = false;
                            break;
                    }
                }
                else
                {
                    loadLine(line, y);
                    y++;
                }
                /*if (line.StartsWith("!"))
                {
                    //Это хедер
                    var headerLine = line.Substring(1, line.Length-1);
                    //_ tp args...
                    var argsraw = headerLine.Split(" ");
                    var characterRaw = argsraw[0];
                    var characterRawDraw = argsraw[1];
                    var commandRaw = argsraw[2];
                    var args = new string[line.Length-3];
                    for (int i = 3; i < argsraw.Length; i++)
                    {
                        args[i-3] = argsraw[i];
                    }

                    var command = getCommand(commandRaw);
                    if (command != null)
                    {
                        var b = new TriggerTileBase("Trigger", "trigger", characterRaw[0], characterRawDraw[0], args);
                        b.command = command;
                        b.isTransient = true;
                        triggerBases.Add(b);
                        TileRegistry.register(b);
                    }
                }
                else
                {
                    loadLine(line, y);
                    y++;
                }*/
            }
        }

        public void loadLine(string line, int y)
        {
            var x = 0;
            foreach (char c in line)
            {
                x++;
                var b = TileRegistry.getByChar(c);
                if (b != null)
                {
                    world._tileGrid.setTile(b, new Location(x, y, world));
                }

                var bb = EntityRegistry.getByChar(c);
                if (bb != null)
                {
                    world.summonEntity(bb, new Location(x, y, world));
                }
            }
        }
    }

    public class StringTileBase : TileBase
    {
        public string text;
        
        public StringTileBase(string name, string tag, string text, char drawChar) : base(name, tag, drawChar)
        {
            setTexture(new Texture(' '));
            enableTransient();
            this.text = text;
        }

        public override Tile createCopy(Location location)
        {
            return new StringTile(this, location);
        }
    }

    public class StringTile : Tile
    {

        public string localText;
        public StringTile(StringTileBase b, Location location) : base(b, location)
        {
            localText = b.text;
        }

        public override void render(Window win)
        {
            win.drawString(Location.toPoint(), localText);
        }

        public override StringTileBase GBase => (StringTileBase) base.GBase;
    }
    

    public class TriggerTileBase : TileBase
    {

        public Command command;
        public string[] args;
        
        public TriggerTileBase(string name, string tag, char curchar, char drawChar, string[] args) : base(name, tag, curchar)
        {
            this.args = args;
            setTexture(new Texture(drawChar));
        }

        public override Tile createCopy(Location location)
        {
            return new TriggerTile(this, location);
        }
    }

    public class TriggerTile : Tile
    {

        public TriggerTile(TileBase b, Location location) : base(b, location)
        {
            
        }

        public override void onPlayerOverlap(Player p)
        {
            (GBase as TriggerTileBase).command.execute(this, (GBase as TriggerTileBase).args);
        }
    }
    public class MapChangeCommand : Command
    {
        public MapChangeCommand(string name) : base(name)
        {
        }
        
        public override void execute(TriggerTile tile, string[] args)
        {
            //Program.client._localWorld.clearEntities();
            //Program.client._localWorld._tileGrid.clear();
            Program.client.loadMap(args[0]);
            Program.client.changeWorld(args[0]);
        }
    }
    public class RemoveManaCommand : Command
    {
        public RemoveManaCommand(string name) : base(name)
        {
        }
        
        public override void execute(TriggerTile tile, string[] args)
        {
            Program.client._localWorld.getPlayer().mana = 0;
        }
    }
    public class DialogCommand : Command
    {
        public DialogCommand(string name) : base(name)
        {
        }
        
        public override void execute(TriggerTile tile, string[] args)
        {
            Program.client.startDialog(args[0]);
        }
    }

    public class ItemGiveCommand : Command
    {
        public ItemGiveCommand(string name) : base(name)
        {
        }
        
        public override void execute(TriggerTile tile, string[] args)
        {
            var type = ItemRegistry.getType(args[0]);
            tile.getWorld().getPlayer().inventory.addItemSlot(type.createStack(readInt(args, 1)));
            tile.removeFromWorld();
        }
    }
    
    public class DamageCommand : Command
    {
        public DamageCommand(string name) : base(name)
        {
        }
        
        public override void execute(TriggerTile tile, string[] args)
        {
            tile.getWorld().getPlayer().damage(readInt(args, 0));
        }
    }
    
    public class TpCommand : Command
    {
        public TpCommand(string name) : base(name)
        {
        }

        public override void execute(TriggerTile tile, string[] args)
        {
            tile.getWorld().getPlayer()._location = new Location(readInt(args, 0), readInt(args, 1), tile.getWorld());
        }
    }
    public class Command
    {

        public string name;

        public Command(string name)
        {
            this.name = name;
        }

        protected int readInt(string[] args, int i)
        {
            return Convert.ToInt32(args[i]);
        }

        public virtual void execute(TriggerTile tile, string[] args)
        {
            
        }
    }
}