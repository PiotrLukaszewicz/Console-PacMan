using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePacMan.GameClasses
{
    class Ghost
    {
        //69 //28
        private Position ghostPos;
        public int prevPosX;
        public int prevPosY;

        private string symbol = ((char)9787).ToString();
        private ConsoleColor color;
        public string Direction = "up";

        public static string[] possibleDirections =
        {
            "up",
            "down",
            "left",
            "right"
        };
        //private static string direction = possibleDirections[random.Next(0, possibleDirections.Length)];
        public static Random random = new Random();


        public Ghost(ConsoleColor color, int x, int y)
        {
            // Create new monster with default values
            this.color = color;
            this.ghostPos = new Position(x, y);
            this.prevPosX = x;
            this.prevPosY = y;
        }

        public bool CheckLeftCell(Ghost[] ghostList, int x, int y, string[,] border)
        {
            bool isEmpty = true;
            foreach (var ghost in ghostList)
            {
                if (x - 1 == ghost.GetPosX() && y == ghost.GetPosY())
                {
                    isEmpty = false;
                    break;
                }
            }

            if (border[y, x - 1] == "#")
            {
                isEmpty = false;
            }

            return isEmpty;
        }
        public bool CheckRightCell(Ghost[] ghostList, int x, int y, string[,] border)
        {
            bool isEmpty = true;
            foreach (var ghost in ghostList)
            {
                if (x + 1 == ghost.GetPosX() && y == ghost.GetPosY())
                {
                    isEmpty = false;
                    break;
                }
            }

            if (border[y, x + 1] == "#")
            {
                isEmpty = false;
            }


            return isEmpty;
        }
        public bool CheckUpCell(Ghost[] ghostList, int x, int y, string[,] border)
        {
            bool isEmpty = true;
            foreach (var ghost in ghostList)
            {
                if (x == ghost.GetPosX() && y - 1 == ghost.GetPosY())
                {
                    isEmpty = false;
                    break;
                }
            }

            if (border[y - 1, x] == "#")
            {
                isEmpty = false;
            }

            return isEmpty;
        }
        public bool CheckDownCell(Ghost[] ghostList, int x, int y, string[,] border)
        {
            bool isEmpty = true;
            foreach (var ghost in ghostList)
            {
                if (x == ghost.GetPosX() && y + 1 == ghost.GetPosY())
                {
                    isEmpty = false;
                }
            }

            if (border[y + 1, x] == "#")
            {
                isEmpty = false;
            }

            return isEmpty;
        }
        public string GetSymbol()

        {
            return this.symbol;
        }
        public int GetPosX()
        {
            return this.ghostPos.X;

        }
        public int GetPosY()
        {
            return this.ghostPos.Y;
        }
        public ConsoleColor GetColor()
        {
            return this.color;
        }
        public void EraseGhost()
        {
            Console.SetCursorPosition(prevPosX, prevPosY);
            Console.Write(' ');
        }
        public void MoveRight()
        {
            if (ghostPos.X + 1 < 34)
            {
                prevPosX = ghostPos.X;
                prevPosY = ghostPos.Y;
                ghostPos.X++;
            }
        }
        public void MoveLeft()
        {
            if (ghostPos.X - 1 > 0)
            {
                prevPosX = ghostPos.X;
                prevPosY = ghostPos.Y;
                ghostPos.X--;
            }
        }
        public void MoveDown()
        {
            if (ghostPos.Y + 1 < 28)
            {
                prevPosX = ghostPos.X;
                prevPosY = ghostPos.Y;
                ghostPos.Y++;
            }
        }
        public void MoveUp()
        {
            if (ghostPos.Y - 1 > 0)
            {
                prevPosX = ghostPos.X;
                prevPosY = ghostPos.Y;
                ghostPos.Y--;
            }
        }

    }
}
