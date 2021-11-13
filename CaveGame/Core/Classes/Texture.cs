using System;

namespace CaveGame.Core.Classes
{
    public class Texture : OffsetRenderer
    {
        public const int SIZE = 3;

        private char c = ' ';
        public ConsoleColor _fColor = ConsoleColor.White;
        public ConsoleColor _bColor = ConsoleColor.Black;
        
        // Texture text = new Texture("#@#", "@ @", "###");
        public Texture(char c)
        {
            this.c = c;
        }

        public void render(Window window, Point origin)
        {
            window.drawDot(origin, new Pixel(c));
        }
    }
}