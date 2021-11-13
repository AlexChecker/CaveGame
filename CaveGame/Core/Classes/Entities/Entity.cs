namespace CaveGame.Core.Classes.Entities
{
    public class Entity : Renderer
    {
        public Location _location;
        public EntityBase _base;
        public int hp;
        public int mana;
        
        public bool isDied
        {
            get
            {
                return hp <= 0;
            }
        }

        public Entity(Location location, EntityBase @base)
        {
            _location = location;
            _base = @base;
            hp = _base.maxHP;
        }



        public void damage(int i)
        {
            hp -= i;
            if (isDied)
            {
                die();
            }
        }

        public void die()
        {
            hp = 0;
            _location.world.entities.Remove(this);
        }

        public virtual void move(int mx, int my,bool noclip = false)
        {
            var tox = _location.x + mx;
            var toy = _location.y + my;
            var tileAt = _location.world.TileGrid.getTileAt(new Location(tox, toy, _location.world));
            if ((tileAt == null || tileAt != null && tileAt.isTransient())||noclip)
            {
                _location.x += mx;
                _location.y += my;
                if (tileAt != null)
                {
                    tileAt.onPlayerOverlap(_location.world.getPlayer());
                }
            }
        }

        public void render(Window win)
        {
            _base.renderer.render(win, _location.toPoint());
        }
    }
}