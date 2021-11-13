namespace CaveGame.Core.Classes.Entities
{
    public class EntityBase
    {
        public string name;
        public char curchar;
        public OffsetRenderer renderer;
        public int maxHP = 10;
        public int maxMana = 10;

        public EntityBase(string name, char curchar, int maxHP,int maxMana)
        {
            this.name = name;
            this.curchar = curchar;
            this.maxHP = maxHP;
            this.maxMana = maxMana;
        }

        public EntityBase setTexture(Texture texture)
        {
            renderer = texture;
            return this;
        }

        public virtual Entity create(Location location)
        {
            return new Entity(location, this);
        }
    }
}