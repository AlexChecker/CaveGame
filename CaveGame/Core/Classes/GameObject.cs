namespace CaveGame.Core.Classes
{
    public class GameObject : Renderer
    {
        private Location location;
        private GameObjectBase gBase;

        public GameObject(GameObjectBase b, Location location)
        {
            gBase = b;
            this.location = location;
        }

        public virtual void render(Window win)
        {
            win.drawString(location.toPoint(), $"[NULL RENDERER: {GBase.Tag}]");
        }
        
        public virtual Location Location
        {
            get => location;
            set => location = value;
        }

        public virtual GameObjectBase GBase => gBase;
    }
}