

namespace CaveGame.Core.Classes
{
    /**
     * Пустышка с позицией
     */
    public class GameObjectBase
    {
        private string tag;

        public GameObjectBase(string tag)
        {
            this.tag = tag;
        }

        public virtual GameObject createCopy(Location location)
        {
            return new GameObject(this, location);
        }

        public string Tag => tag;
    }
}