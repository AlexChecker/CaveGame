using System;
using System.Threading;
using CaveGame.Core.Classes.Entities;

namespace CaveGame.Core.Classes.Battle
{
    public class Battle : Renderer
    {
        private Entity _entity;
        private Player _player;
        private bool attack = true;
        private bool animation = false;
        private int power = 10;
        private int time = 0;
        private int interval = 0;
        private int intervalsize = 7;
        private bool rewind = false;
        public int x = 100, y = 3;
        private int defended = 0;
        private int clicks = 0;
        private string charset = " █▄▌▐▀░▒▓⌂";
        public Battle(Entity entity, Player player)
        {
            _entity = entity;
            _player = player;
        }

        public bool isAllAlive()
        {
            return _entity.hp != 0 && _player.hp != 0;
        }

        public void drawCursor(Pixel px, Point up, Window win)
        {
            drawInThree(px, up, win, 5);   
        }
        
        public void drawInThree(Pixel px, Point up, Window win, int size = 3)
        {
            win.drawDot(up, px);
            for (int i = 0; i < size; i++)
            {
                win.drawDot(new Point(up.x, up.y - i), px);
            }
        }
        
        public delegate ConsoleColor DrawLogic(int i);

        public void drawBar(Window win, DrawLogic logic)
        {
            Point leftUp = this.leftUp();
            
            Pixel border = new Pixel('[');
            Pixel border2 = new Pixel(']');
            Pixel red = new Pixel('|');
            red.fColor = ConsoleColor.Red;
            Pixel yellow = new Pixel('|');
            yellow.fColor = ConsoleColor.Yellow;
            Pixel green = new Pixel('|');
            win.drawDot(leftUp,red);
            green.fColor = ConsoleColor.Green;
            for (int i = 0; i < x; i++)
            {
                var color = logic(i);
                var px = new Pixel('|');
                px.fColor = color;
                drawInThree(px, new Point(leftUp.x + i, leftUp.y), win);
            }
            drawInThree(border, new Point(leftUp.x, leftUp.y), win);
            drawInThree(border2, new Point(leftUp.x + x, leftUp.y), win);
        }
        
        public void defenseBar(Window win)
        {
            drawBar(win, (i)=>
            {
                if (interval<=i && i<= interval+intervalsize)
                {
                    return ConsoleColor.Green;
                }
                else
                {
                    return ConsoleColor.Red;
                }
            });
        }
        public void attackBar(Window win)
        {
            drawBar(win, (i)=>
            {
                if (i < 15 || i > 85)
                {
                    return ConsoleColor.Red;
                }
                else if (i < 30 || i > 70)
                {
                    return ConsoleColor.DarkYellow;
                }
                else
                {
                    return ConsoleColor.Green;
                }
            });
        }
        public void controlUpdate(ConsoleKey key)
        {
            if (attack)
            {
                if (key == ConsoleKey.Spacebar)
                {
                    animation = false;
                    if (power >= 30 && power <= 70)
                    {
                        _entity.damage(15);
                    }
                    else if ((power >= 15 && power < 30) || (power > 70 && power <= 85))
                    {
                        _entity.damage(10);
                    }
                    else if (power < 15 || power > 85)
                    {
                        _entity.damage(5);
                    }
                    power = 0;
                    attack = false;
                }

            }
            else
            {
                if (clicks <5)
                {
                    if (key == ConsoleKey.Spacebar)
                    {
                        if (interval <= power && power <= interval + intervalsize)
                        {
                            defended++;
                        }

                        Random rng = new Random();
                        interval = rng.Next(0, 100 - intervalsize);
                        clicks++;
                    }
                }
                else
                {
                    if (defended == 5) _player.damage(0);
                    else _player.damage(5*(5-defended));
                    attack = true;
                    clicks = 0;
                    defended = 0;
                }

            }
        }

        public Point leftUp()
        {
            return new Point(Program.WIDTH/2 - x/2, Program.HEIGHT/2 - y/2);
        }
        public void drawStats(Window win)
        {
            win.drawString(new Point(100, 5), "HP: "+_player.hp.ToString()+"/"+_player._base.maxHP);
            win.drawString(new Point(100, 6), "EHP: "+_entity.hp.ToString()+"/"+_entity._base.maxHP);
            win.drawString(new Point(100, 7), "DF: "+defended);
            win.drawString(new Point(100, 8), "cl: "+clicks);
        }

        public void drawenemy(Window win)
        {
            Random rand = new Random();
            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    win.drawString(new Point(Program.WIDTH/2-5+x,10+y),charset[rand.Next(charset.Length)].ToString());
                }
            }
        }


        public void render(Window win)
        {
            if (!isAllAlive()) Program.client.battle = null;
            Window.offsetDraw = false;
            //Point origin = new Point(Program.WIDTH/2 - 55/2, Program.HEIGHT/2 - 3);
            Point leftUp = this.leftUp();
            //win.drawDot(new Point(0, 0), new Pixel('@'));
            //win.drawDot(leftUp, new Pixel('@'));
             Pixel attackPointer = new Pixel('|');
             animation = true;
             int animationTemp = 2;
                drawStats(win);
             if (attack)
             {
                 if (animation)
                 {
                     attackBar(win);
                     animationTemp--;
                     if (rewind)
                     {
                         power--;
                     }
                     else
                     {
                         power++;
                     }
                     if (power <= 0) rewind = false;
                     if (power >= 100) rewind = true;
                     drawCursor(attackPointer,new Point(leftUp.x+power,leftUp.y+1),win);
                     drawenemy(win);
                 }
             }
             else
             {
                 if (animation)
                 {
                     defenseBar(win);
                     animationTemp--;
                     if (rewind)
                     {
                         power-=2;
                     }
                     else
                     {
                         power+=2;
                     }
                     if (power <= 0) rewind = false;
                     if (power >= 100) rewind = true;
                         drawCursor(attackPointer,new Point(leftUp.x+power,leftUp.y+1),win);
                         
                     
                 }
             }
            Window.offsetDraw = true;
        }
    
}
    
}