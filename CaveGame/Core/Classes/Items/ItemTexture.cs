using System.IO;

namespace CaveGame.Core.Classes.Items
{
    public class ItemTexture : OffsetRenderer
    {

        public char[,] chars = new char[8,8];

        public void loadFromFile(string path)
        {
            var x = 0;
            var y = 0;
            foreach (string line in File.ReadAllLines(path))
            {
                foreach (char c in line)
                {
                    if (c == '-')
                    {
                        chars[x, y] = ' ';
                    }
                    else
                    {
                        chars[x, y] = c;
                    }

                    x++;
                }
                y++;
                x = 0;
            }
        }

        public void render(Window win, Point origin)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var to = chars[i, j];
                    if (to == ' ') continue;
                    win.drawDot(new Point(origin.x + i, origin.y + j), new Pixel(to));
                }
            }
        }
    }
}