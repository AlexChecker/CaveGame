namespace CaveGame.Core.Classes.Utils
{
    public class Difficulty
    {

        public static Difficulty EASY = new (200);
        public static Difficulty MEDIUM = new (400);
        public static Difficulty HARD = new (500);
        public static Difficulty HARDCORE = new (600);

        private int chance;
        
        private Difficulty(int chance)
        {
            this.chance = chance;
        }

        public int Chance
        {
            get => chance;
        }
    }
}