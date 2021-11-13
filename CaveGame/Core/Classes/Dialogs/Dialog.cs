
using System;
using System.Collections.Generic;
using System.IO;


namespace CaveGame.Core.Classes.Dialogs
{
    public class Dialog : Renderer
    {
        public static string dialogFolder = "Dialogs";
        public int width = 80;
        public int height = 20;
        public List<Message> messages = new();
        public Message currentMessage = null;

        public Dialog(string dialogName)
        {
            if (!File.Exists($"{dialogFolder}/" + dialogName))
                throw new Exception($"Dialog file with name {dialogName} not found in dialogs/");
            string[] lines = File.ReadAllLines($"{dialogFolder}/"+dialogName);
            Message? messageTemp = null;
            foreach (string line in lines)
            {
                if (line.Length <= 1) continue;
                //Инициализация в список месседжей
                var withoutStartChar = line.Substring(1, line.Length - 1);
                if (line.StartsWith("//"))
                {
                    continue;
                }
                if (line.StartsWith("#"))
                {
                    if (messageTemp != null)
                    {
                        messages.Add(messageTemp);
                        messageTemp = new Message();
                    }
                    else
                    {
                        messageTemp = new Message();
                    }

                    var args = withoutStartChar.Split(":");
                    var tag = args[0];
                    var message = args[1];
                    messageTemp.message = message;
                    messageTemp.tag = tag;
                    messageTemp.stack();
                }else if (line.StartsWith("@"))
                {
                    var args = withoutStartChar.Split(" -> ");
                    var name = args[0];
                    var gto = args[1];
                    messageTemp.answers.Add(new Answer(name, gto));
                }
                //#hello:Привет брат
                //@Да -> hello
                //@Нет -> okey
                //#okey:Окей нормально 
                //@Пошёл в -> hello
            }

            if (messageTemp != null)
            {
                messages.Add(messageTemp);
            }
        }

        public int animationTemp = 0;
        public int animationTempMax = 0;
        public int animationWait = 0;
        public int animationWaitStat = 2; //16*100
        public bool animation = false;
        public string tempAnimation = "";
        public bool visible = false;

        public void startAnimation()
        {
            var message = currentMessage;
            if (message == null) return;
            visible = true;
            animation = true;
            animationTemp = 0;
            tempAnimation = "";
            animationTempMax = message.message.Length;
            animationWait = animationWaitStat;
        }

        public void startMessage()
        {
            var message = currentMessage;
            if (message == null)
            {
                currentMessage = messages[0];
            }
        }

        public void controlUpdate(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Escape:
                    stopAnimation();
                    break;
                case ConsoleKey.Enter:
                    if (animation)
                    {
                        animation = false;
                        return;
                    }
                    if (currentMessage == null)
                    {
                        stopAnimation();
                    }
                    else
                    {
                        if (enter(currentMessage))
                        {
                            startAnimation();
                        }
                    }
                    break;
                case ConsoleKey.UpArrow:
                    currentMessage.moveUp();
                    break;
                case ConsoleKey.DownArrow:
                    currentMessage.moveDown();
                    break;
            }
        }

        private void drawLine(Window win, Point origin, int y, string line)
        {
            win.drawString(origin + new Point(0, y), line);
        }

        public void stopAnimation()
        {
            animation = false;
            visible = false;
            currentMessage = null;
        }
        
        private void redrawWindow(Window win)
        {
            var message = currentMessage;
            if (message == null) return;
            //TODO Отрисовка (Максимально мало команд)
            var origin = new Point(Program.WIDTH / 2, Program.HEIGHT / 2);
            var yOffset = (int)((Program.HEIGHT / 2) * 0.45);
            var boxOrigin = origin + new Point(0, yOffset);
            var boxStart = boxOrigin - new Point(width / 2, height / 2);
            win.drawBox(boxStart, boxStart + new Point(width, height));
            if (animation)
            {
                animationWait--;
                if (animationWait <= 0)
                {
                    animationWait = animationWaitStat;
                    if (animationTemp <= animationTempMax-1)
                    {
                        tempAnimation += message.message[animationTemp];
                        animationTemp++;
                    }
                    else
                    {
                        animation = false;
                    }
                }
            }
            else
            {
                tempAnimation = message.message;
            }

            var textStart = boxStart + new Point(2, 2);

            var y = 0;
            foreach (string line in tempAnimation.Split("\n"))
            {
                drawLine(win, textStart, y, line.Replace("\n", ""));
                y++;
            }

            var offsetText = message.message.Length / Message.MAX_LENGTH;
            var startAnswer = boxStart + new Point(2, offsetText+5);
            var i = 0;
            foreach (var answer in message.answers)
            {
                if (message.selected == i)
                {
                    win.drawString(startAnswer + new Point(0, i*2), ">> "+answer.name);
                }
                else
                {
                    win.drawString(startAnswer + new Point(0, i*2), "   "+answer.name);
                }
                i++;
            }
            //win.drawBox(new Point(0, Program.HEIGHT - height), new Point(Program.WIDTH, Program.HEIGHT));
        }

        public void render(Window win)
        {
            redrawWindow(win);
        }

        public class Answer
        {
            public string name;
            public string gto;

            public Answer(string name, string gto)
            {
                this.name = name;
                this.gto = gto;
            }
        }

        public Message? getByTag(string tag)
        {
            foreach (Message msg in messages)
            {
                if (msg.tag == tag)
                {
                    return msg;
                }
            }

            return null;
        }
        
        private bool enter(Message message)
        {
            var nextAns = message.getSelected();
            if (nextAns == null)
            {
                stopAnimation();
                return false;
            }
            var next = getByTag(nextAns.gto);
            if (next != null)
            {
                currentMessage = next;
            }

            return true;
        }

        public class Message
        {
            public static int MAX_LENGTH = 50;
            public string message;
            public string tag;
            public int selected = 0;
            public List<Answer> answers = new();

            public void stack()
            {
                var b = "";
                var s = 0;
                for (int i = 0; i < message.Length; i++)
                {
                    b += message[i];
                    if (s >= MAX_LENGTH)
                    {
                        b += "\n";
                        s = 0;
                    }
                    s++;
                }

                message = b;
            }

            public Answer? getSelected()
            {
                if (selected < 0) return null;
                if (answers.Count > selected)
                {
                    return answers[Selected];
                }
                else
                {
                    return null;
                }
            }
            
            public int Selected
            {
                get { return selected;}
                set
                {
                    selected = value;
                    if (selected < 0)
                    {
                        selected = 0;
                    }

                    if (selected > answers.Count-1)
                    {
                        selected = answers.Count-1;
                    }
                }
            }

            public void enter(Dialog dialog)
            {
                dialog.enter(this);
            }

            public void moveUp()
            {
                Selected--;
            }

            public void moveDown()
            {
                Selected++;
            }
        }
    }
}