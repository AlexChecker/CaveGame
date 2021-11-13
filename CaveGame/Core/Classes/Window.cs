using System;
using System.Text;

namespace CaveGame.Core.Classes
{
    public class Window
    {

        public static int lerp(int x, int x1, float p)
        {
            return (int)(x * (1 - p) + x1 * p);
        }

        private int w, h;

        private Pixel?[,] buffer;

        public static bool offsetDraw = true;
        public static Point offset = new Point(0, 0);

        public Window(int w, int h)
        {
            this.w = w;
            this.h = h;
            buffer = new Pixel?[w, h];
        }

        public void drawBox(Point from, Point to)
        {
            clearBox(from, to);
            for (int i = from.x; i < to.x; i++)
            {
                drawDot(new Point(i, from.y), new Pixel('═'));
                drawDot(new Point(i, to.y), new Pixel('═'));
            }
            for (int i = from.y; i < to.y; i++)
            {
                drawDot(new Point(from.x, i), new Pixel('║'));
                drawDot(new Point(to.x, i), new Pixel('║'));
            }
            drawDot(new Point(from.x, from.y), new Pixel('╔'));//1
            drawDot(new Point(to.x, from.y), new Pixel('╗'));//2
            drawDot(new Point(to.x, to.y), new Pixel('╝'));//3
            drawDot(new Point(from.x, to.y), new Pixel('╚'));//4
        }

        public void clearBox(Point from, Point to)
        {
            for (int i = from.x; i < to.x; i++)
            {
                for (int j = from.y; j < to.y; j++)
                {
                    drawDot(new Point(i, j), new Pixel(' '));
                }
            }
        }

        public void drawLine(Point f, Point f2, Pixel c)
        {
            for (float i = 0; i < 1; i += 0.01f)
            {
                var lerped = f.lerp(f2, i);
                drawDot(lerped, c);
            }
        }

        public void drawString(Point origin, String text)
        {
            var y = 0;
            foreach (string line in text.Split("\n"))
            {
                for (int i = 0; i < line.Length; i++)
                {
                    drawDot(origin + new Point(i, y), new Pixel(line[i]));
                }

                y++;
            }
            //for (int i = 0; i < text.Length; i++)
            //{
            //    drawDot(origin + new Point(i, 0), new Pixel(text[i]));
            //}
        }
        
        public void drawDot(int x, int y, char px)
        {
            var cur = buffer[x, y];
            if (cur != null)
            {
                cur.c = px;
            }
            else
            {
                drawDot(new Point(x, y), new Pixel(px));
            }
        }
        
        public void drawDot(Point point, char px)
        {
            var cur = buffer[point.x, point.y];
            if (cur != null)
            {
                cur.c = px;
            }
            else
            {
                drawDot(point, new Pixel(px));
            }
        }
        
        public void drawDot(Point point, Pixel px)
        {
            var tox = point.x + offset.x;
            var toy = point.y + offset.y;
            if (!offsetDraw)
            {
                tox = point.x;
                toy = point.y;
            }
            if (tox < 0 || toy < 0) return;
            if (buffer.GetLength(0) > tox && buffer.GetLength(1) > toy)
            {
                if (tox < w && toy < h)
                {
                    buffer[tox, toy] = px;
                }
            }
        }

        public void clrDot(Point p)
        {
            buffer[p.x, p.y]?.delete();
        }

        public void clearBuffer()
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    buffer[i, j] = null;
                }
            }
        }

        public void drawBuffer()
        {
            var rawBuffer = buildBuffer();
            FastDraw.draw((short)w, (short)h, rawBuffer);
        }

        public FastDraw.CharInfo[] buildBuffer()
        {
            var buffer = new FastDraw.CharInfo[w * h];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    var inBuf = this.buffer[j, i];
                    if (inBuf != null)
                    {
                        var inBufN = (Pixel) inBuf;
                        
                        buffer[j + i * w] = inBufN.getInfo();
                        continue;
                    }

                    var b = new FastDraw.CharInfo();
                    b.Attributes = 0;
                    b.Char.UnicodeChar = ' ';
                    buffer[j + i * w] = b;
                }
            }

            return buffer;
        }
    }
}