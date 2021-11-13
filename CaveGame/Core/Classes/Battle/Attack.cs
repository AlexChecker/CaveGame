using System;
using CaveGame.Core.Classes.Entities;

namespace CaveGame.Core.Classes.Battle
{
    public class Attack: Renderer
    {
        private int difficulty;
        private int power;
        private Entity encounter;
        private int pointer = 0;
        private bool animation = false;
        private bool rewind = false;

        public Attack(int difficulty, Entity encounter)
        {
            this.difficulty = difficulty;
            this.encounter = encounter;
        }


        //если нажат пробел, то выдаём вражине леща с силой, соответствующей указателю атаки
        public void controlUpdate(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Spacebar:
                    animation = false;
                    if (power >= 30 && power <= 70)
                    {
                        encounter.damage(15);
                    }
                    else if ((power >= 15 && power < 30) || (power > 70 && power <= 85))
                    {
                        encounter.damage(10);
                    }
                    else if (power < 15 || power > 85)
                    {
                        encounter.damage(5);
                    }

                    break;
            }
        }
        //см класс Defense
        public void drawInThree(Pixel px,Point up,Window win)
        {
            win.drawDot(up,px);
            win.drawDot(new Point(up.x,up.y-1),px);
            win.drawDot(new Point(up.x,up.y-2),px);
        }
        //см класс Defense
        public void drawBar(Window win)
        {
            Point origin = new Point(Program.WIDTH / 2, Program.HEIGHT / 2);
            Point leftUp = new Point(origin.x - 51, origin.y - 1);
            Pixel border = new Pixel('[');
            Pixel border2 = new Pixel(']');
            Pixel red = new Pixel('|');
            red.fColor = ConsoleColor.Red;
            Pixel yellow = new Pixel('|');
            yellow.fColor = ConsoleColor.Yellow;
            Pixel green = new Pixel('|');
            green.fColor = ConsoleColor.Green;
            for (int i = 0; i < 102; i++)
            {
                if (i == 0)
                {
                    drawInThree(border,new Point(leftUp.x+i,leftUp.y),win);
                }
                else if (i == 101)
                {
                    drawInThree(border2,new Point(leftUp.x+i,leftUp.y),win);
                }
                else if (i < 15 || i > 85)
                {
                    drawInThree(red,new Point(leftUp.x+i,leftUp.y),win);
                }
                else if (i < 30 || i > 70)
                {
                    drawInThree(yellow,new Point(leftUp.x+i,leftUp.y),win);
                }
                else
                {
                    drawInThree(green,new Point(leftUp.x+i,leftUp.y),win);
                }
            }
        }
        //Отвечает за отрисовку. Постоянно двигает указатель атаки
        public void redrawWindow(Window win)
        {
            Point origin = new Point(Program.WIDTH / 2, Program.HEIGHT / 2);
            Point leftUp = new Point(origin.x - 50, origin.y - 1);
            Pixel attackPointer = new Pixel('|');
            animation = true;
            
            int animationTemp = 2;
            while (animation)
            {
                drawBar(win);
                animationTemp--;
                if (animationTemp <= 0)
                {
                    if (rewind)
                    {
                        power--;
                    }
                    else
                    {
                        power++;
                    }
                    if (power == 0) rewind = false;
                    if (power == 100)rewind = true;
                    drawInThree(attackPointer,new Point(leftUp.x+power,leftUp.y),win);
                }
            }

        }

        public void render(Window win)
        {
            redrawWindow(win);
            
        }
    }
}