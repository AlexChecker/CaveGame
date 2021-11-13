using System;
using CaveGame.Core.Classes.Entities;

namespace CaveGame.Core.Classes.Battle
{
    public class Defense
    {
        private Player p;
        private int power;
        private bool animation;
        private int time = 1500;

        public Defense(Player p)
        {
            this.p = p;
        }
        //если нажат пробел, то добавляет к шкале защиты значение
        public void controlUpdate(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Spacebar:
                    power += 5;

                    break;
            }
        }
        //смотрит, не закончилось ли время у игрока, заполнил ли он шкалу? Если да,то выдаёт соответствующий урон
        public void onUpdate()
        {
            time--;
            if (time <= 0||power >= 100)
            {
                animation = false;
                if (power <30)
                {
                    p.damage(15);
                }
                else if ((power <80))
                {
                    p.damage(10);
                }
                else 
                {
                    p.damage(0);
                }
            }
        }
        //Рисует столбец из 3 одинаковых символов.
        public void drawInThree(Pixel px,Point up,Window win)
        {
            win.drawDot(up,px);
            win.drawDot(new Point(up.x,up.y-1),px);
            win.drawDot(new Point(up.x,up.y-2),px);
        }
        //отрисовывает шкалу защиты
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
                else if (i < 30)
                {
                    drawInThree(red,new Point(leftUp.x+i,leftUp.y),win);
                }
                else if (i < 80)
                {
                    drawInThree(yellow,new Point(leftUp.x+i,leftUp.y),win);
                }
                else
                {
                    drawInThree(green,new Point(leftUp.x+i,leftUp.y),win);
                }
            }
        }
        //Перерисовывает окно + меняет позицию указателя защиты (если не вызывается controlupdate, то показатель убывает)
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
                    power-=2;
                    
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