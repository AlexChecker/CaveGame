using System;
using System.Collections.Generic;
using CaveGame.Core.Classes.Entities;

namespace CaveGame.Core.Classes.Skills
{
    public class Skills
    {
        public static List<Skill> skills = new();

        static Skills()
        {
            skills.Add(new JumpSkill("Резкий прыжок",10));
        }

        public static void use(Player p, ConsoleKey key)
        {
            foreach (Skill skill in skills)
            {
                skill.tryUseRaw(p, key,skill.manaUsage);
            }
        }
    }

    public class JumpSkill : Skill
    {
        public JumpSkill(string name,int manaUsage) : base(name,manaUsage)
        {
            
        }

        public override bool tryUse(Player p, ConsoleKey key,int manaCost)
        {
            if (p.mana >= manaCost)
                return key == ConsoleKey.Q;
            else return false;
        }

        public override void use(Player p)
        {
            p.mana -= 10;
            var direction = Console.ReadKey().Key;
            switch (direction)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    p.move(0, -10,p.noclip);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    p.move(0, 10,p.noclip);
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    p.move(-10, 0,p.noclip);
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    p.move(10, 0,p.noclip);
                    break;
            }
        }
    }
    
    public abstract class Skill //Абстракция
    {

        public string name;
        public int manaUsage;
        public Skill(string name,int manaUsage)
        {
            this.name = name;
            this.manaUsage = manaUsage;
        }

        public void tryUseRaw(Player p, ConsoleKey key, int manaCost)
        {
            if (tryUse(p, key,manaCost))
            {
                use(p);
            }
        }

        public abstract bool tryUse(Player p, ConsoleKey key,int manaCost); //Если тру
        public abstract void use(Player p);
    }
}