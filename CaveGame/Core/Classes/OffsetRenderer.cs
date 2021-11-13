namespace CaveGame.Core.Classes
{
    public interface Renderer
    {
        void render(Window win);
    }
    public interface OffsetRenderer
    {
        void render(Window win, Point origin);
    }
}