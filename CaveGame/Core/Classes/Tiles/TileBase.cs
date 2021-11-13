using System.Drawing;

namespace CaveGame.Core.Classes.Tiles
{
    public class TileBase : GameObjectBase
    {

        private string _name;
        private char drawChar;
        private OffsetRenderer _renderer;
        public bool smoothTexture = false;
        public bool isTransient = false;
        public Color color;
        public bool enableStacking = false;

        public static Location toGlobal(Location offset)
        {
            return new Location(offset.x * Texture.SIZE, offset.y * Texture.SIZE, offset.world);
        }

        public TileBase(string name, string tag, char drawChar) : base(tag)
        {
            _name = name;
            this.drawChar = drawChar;
        }
        
        public TileBase enableStack()
        {
            enableStacking = true;
            return this;
        }


        public TileBase enableTransient()
        {
            isTransient = true;
            return this;
        }

        public TileBase setTexture(Texture texture)
        {
            _renderer = texture;
            return this;
        }

        public override Tile createCopy(Location location)
        {
            return new Tile(this, location);
        }

        public virtual OffsetRenderer Renderer
        {
            get => _renderer;
        }
        
        public char DrawChar
        {
            get { return drawChar; }
        }

        public string Name => _name;
    }
}