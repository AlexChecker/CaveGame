using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using CaveGame.Core.Classes;
using CaveGame.Core.Classes.Battle;
using CaveGame.Core.Classes.Dialogs;
using CaveGame.Core.Classes.Entities;
using CaveGame.Core.Classes.Items;
using CaveGame.Core.Classes.Registry;
using CaveGame.Core.Classes.Skills;
using CaveGame.Core.Classes.Utils;

namespace CaveGame.Core.Client
{
    public class Client
    {
        public Window _window;
        public bool willStop = false;
        public List<World> worlds = new();
        public World _localWorld;
        public Dialog? dialog = null;
        public Player player;
        public Battle? battle = null;
        public Difficulty diff = Difficulty.EASY;
        public bool nobattle = false;

        public Client()
        {
            _window = new Window(Program.WIDTH, Program.HEIGHT);
            loadMap("main");
            changeWorld("main");
        }

        public void runBattle()
        {
            Random rand = new Random();
            battle = new Battle(EntityRegistry.bases[rand.Next(EntityRegistry.bases.Count)].create(new Location(0,0,_localWorld)), player);
        }

        public void changeWorld(string tag)
        {
            World cur = getWorld(tag);
            if (cur != null)
            {
                if (_localWorld != null)
                {
                    _localWorld.builder.clearBases();
                }
                cur.loadMap();
                player.teleport(cur.spawnLocation);
            }

            _localWorld = cur;
        }

        public World getWorld(string map)
        {
            foreach (World world in worlds)
            {
                if (world.tag.Equals(map))
                {
                    return world;
                }
            }

            return null;
        }

        public void loadMap(string map)
        {
            if (getWorld(map) != null) return;
            World loaded = new World(this, map);
            if (player == null)
            {
                player = EntityRegistry.PLAYER.create(loaded.spawnLocation) as Player;
                loaded.spawnEntity(player);
            }
            worlds.Add(loaded);
        }

        public void startDialog(string path)
        {
            dialog = new Dialog(path);
            dialog.startMessage();
            dialog.startAnimation();
        }

        public void onInit()
        {
            
        }

        public void inputCycle()
        {
            string command = "";
            while (true)
            {
                var key = Console.ReadKey().Key;
                var p = _localWorld.getPlayer();
                lock (_locker)
                {
                    if (key == ConsoleKey.Oem2)
                    {
                        command = Console.ReadLine();
                        //_window.drawString(new Point(0,0),command);
                        Commands.execute(command,_window,p);
                    }

                    //Console.Write(key);
                    Skills.use(p, key);
                    
                    var disControls = false;
                    if (dialog != null)
                    {
                        if (dialog.visible)
                        {
                            disControls = true;
                            dialog.controlUpdate(key);
                        }
                    }

                    if (battle != null)
                    {
                        disControls = true;

                        battle.controlUpdate(key);
                        
                    }

                    if (!disControls)
                    {
                        if (p != null)
                        {
                            p.controlUpdate(key);
                        }
                    }
                }
            }
        }

        private void drawDied()
        {
            //_window.drawString(new Point(Program.WIDTH / 2 - 55 / 2, Program.HEIGHT / 2 - 8),"██╗   ██╗ █████╗ ██╗   ██╗  ██████╗ ██╗███████╗██████╗ ");
            //_window.drawString(new Point(Program.WIDTH / 2 - 55 / 2, Program.HEIGHT / 2 - 7),"╚██╗ ██╔╝██╔══██╗██║   ██║  ██╔══██╗██║██╔════╝██╔══██╗");
            //_window.drawString(new Point(Program.WIDTH / 2 - 55 / 2, Program.HEIGHT / 2 - 6)," ╚████╔╝ ██║  ██║██║   ██║  ██║  ██║██║█████╗  ██║  ██║");
            //_window.drawString(new Point(Program.WIDTH / 2 - 55 / 2, Program.HEIGHT / 2 - 5),"  ╚██╔╝  ██║  ██║██║   ██║  ██║  ██║██║██╔══╝  ██║  ██║");
            //_window.drawString(new Point(Program.WIDTH / 2 - 55 / 2, Program.HEIGHT / 2 - 4),"   ██║   ╚█████╔╝╚██████╔╝  ██████╔╝██║███████╗██████╔╝");
            //_window.drawString(new Point(Program.WIDTH / 2 - 55 / 2, Program.HEIGHT / 2 - 3),"   ╚═╝    ╚════╝  ╚═════╝   ╚═════╝ ╚═╝╚══════╝╚═════╝ ");
                       _window.drawString(new Point(Program.WIDTH/2 - 55/2, Program.HEIGHT/2 - 3), @"
            ██╗   ██╗ █████╗ ██╗   ██╗  ██████╗ ██╗███████╗██████╗ 
            ╚██╗ ██╔╝██╔══██╗██║   ██║  ██╔══██╗██║██╔════╝██╔══██╗
             ╚████╔╝ ██║  ██║██║   ██║  ██║  ██║██║█████╗  ██║  ██║
              ╚██╔╝  ██║  ██║██║   ██║  ██║  ██║██║██╔══╝  ██║  ██║
               ██║   ╚█████╔╝╚██████╔╝  ██████╔╝██║███████╗██████╔╝
               ╚═╝    ╚════╝  ╚═════╝   ╚═════╝ ╚═╝╚══════╝╚═════╝ ");
        }

        public void drawStats(Player p)
        {
            Window.offsetDraw = false;
            _window.drawString(new Point(5, 5), "HP: "+p.hp.ToString()+"/"+p._base.maxHP);
            _window.drawString(new Point(5, 6), "MP: "+p.mana.ToString()+"/"+p._base.maxMana);
            _window.drawString(new Point(5, 7), "BT: "+nobattle);
            Window.offsetDraw = true;
        }

        public void onUpdate()
        {
            lock (_locker)
            {
                var p = _localWorld.getPlayer();
                if (p == null)
                {
                    Window.offsetDraw = false;
                    drawDied();
                    Window.offsetDraw = true;
                    return;
                }
                else
                {
                    p.screenSync();
                }
                drawStats(player);
                if (battle == null)
                {
                    _localWorld.render(_window);
                    Window.offsetDraw = false;

                    if (p.inventory.inventoryFocus)
                    {
                        p.inventory.render(_window, new Point(Program.WIDTH / 2 - 16, Program.HEIGHT / 2 - 16));
                    }

                    if (dialog != null)
                    {
                        if (dialog.visible)
                        {
                            dialog.render(_window);
                        }
                    }

                    Random chance = new Random();


                    Window.offsetDraw = true;
                }
                else
                {
                    battle.render(_window);
                }
            }
        }

        static readonly object _locker = new object();
        
        public void mainCycle()
        {
            onInit();
            try
            {
                while (!willStop)
                {
                    Thread.Sleep(16);
                    _window.clearBuffer();
                    onUpdate();
                    _window.drawBuffer();
                    Console.SetCursorPosition(0, 0);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}