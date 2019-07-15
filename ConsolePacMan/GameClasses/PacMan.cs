using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Media;

namespace ConsolePacMan.GameClasses
{
    class PacMan
    {

        private Position pacManPos;
        private int score;
        private int lives;
        private int level;

        private string symbol = ((char)9786).ToString();
        private ConsoleColor color = ConsoleColor.Yellow;
        public string Direction = "right";
        public string NextDirection = "right";


        public int GetScore()
        {
            return this.score;
        }

        public int Lives()
        {
            return this.lives;
        }

        public int GetLevel()
        {
            return this.level;
        }

        public PacMan()
        {
            // Create new PacMan with default values
            this.pacManPos = new Position(17, 20);
            this.score = 0; // 684 = WIN
            this.lives = 0; 
            this.level = 0;
        }

        public void ResetPacMan()
        {
            this.pacManPos.X = 17;
            this.pacManPos.Y = 20;
            this.Direction = "right";
            this.NextDirection = "right";
        }

        public void LoseLife()
        {
            this.lives--;
            DeathPlayerMusic();
            Thread.Sleep(1200);
        }

        public void EarnPoint()
        {
            this.score++;
        }

        public void EarnStar()
        {
            this.score += 100;
        }

        public void LevelUp()
        {
            this.level++;
            this.score = 0;
        }

        public string GetSymbol()
        {
            return this.symbol;
        }

        public int GetPosX()
        {
            return this.pacManPos.X;
        }

        public int GetPosY()
        {
            return this.pacManPos.Y;
        }

        public ConsoleColor GetColor()
        {
            return this.color;
        }

        public BoardElements CheckCell(string[,] border, string direction, Ghost[] ghostList)
        {
            switch (direction)
            {
                case "up":
                    switch (border[this.pacManPos.Y - 1, this.pacManPos.X])
                    {
                        case "#":
                            return BoardElements.Wall;
                        case ".":
                            return BoardElements.Dot;
                        case "*":
                            return BoardElements.Star;
                        //case "☻":
                        //  return BoardElements.Monster;
                        default:
                            if (checkIfGhostAppears(ghostList, this.pacManPos.Y - 1, this.pacManPos.X))
                            {
                                return BoardElements.Ghost;
                            }
                            else
                            {
                                return BoardElements.Empty;
                            }
                    }

                //return BoardElements.Empty;
                case "right":
                    switch (border[this.pacManPos.Y, this.pacManPos.X + 1])
                    {
                        case "#":
                            return BoardElements.Wall;
                        case ".":
                            return BoardElements.Dot;
                        case "*":
                            return BoardElements.Star;
                        default:
                            if (checkIfGhostAppears(ghostList, this.pacManPos.Y, this.pacManPos.X + 1))
                            {
                                return BoardElements.Ghost;
                            }
                            else
                            {
                                return BoardElements.Empty;
                            }
                    }

                //return BoardElements.Empty;
                case "down":
                    switch (border[this.pacManPos.Y + 1, this.pacManPos.X])
                    {
                        case "#":
                            return BoardElements.Wall;
                        case ".":
                            return BoardElements.Dot;
                        case "*":
                            return BoardElements.Star;
                        default:
                            if (checkIfGhostAppears(ghostList, this.pacManPos.Y + 1, this.pacManPos.X))
                            {
                                return BoardElements.Ghost;
                            }
                            else
                            {
                                return BoardElements.Empty;
                            }
                    }

                //return BoardElements.Empty;
                case "left":
                    switch (border[this.pacManPos.Y, this.pacManPos.X - 1])
                    {
                        case "#":
                            return BoardElements.Wall;
                        case ".":
                            return BoardElements.Dot;
                        case "*":
                            return BoardElements.Star;
                        default:
                            if (checkIfGhostAppears(ghostList, this.pacManPos.Y, this.pacManPos.X - 1))
                            {
                                return BoardElements.Ghost;
                            }
                            else
                            {
                                return BoardElements.Empty;
                            }
                    }

                //return BoardElements.Empty;
                default:
                    if (checkIfGhostAppears(ghostList, pacManPos.Y, pacManPos.X))
                    {
                        return BoardElements.Ghost;
                    }
                    else
                    {
                        return BoardElements.Empty;
                    }
            }
            //return BoardElements.Empty;
        }
        public void MoveUp()
        {
            this.pacManPos.Y -= 1;
        }
        public void MoveDown()
        {
            this.pacManPos.Y += 1;
        }
        public void MoveLeft()
        {
            this.pacManPos.X -= 1;
        }
        public void MoveRight()
        {
            this.pacManPos.X += 1;
        }

        public bool checkIfGhostAppears(Ghost[] ghostList, int pacManPosY, int pacManPosX)
        {
            foreach (var ghost in ghostList)
            {
                if (ghost.GetPosX() == pacManPosX && ghost.GetPosY() == pacManPosY)
                {
                    return true;
                }

            }

            return false;
        }


        public void DeathPlayerMusic()
        {
            SoundPlayer death = new SoundPlayer(ConsolePacMan.PacManMusic.pacman_death);
            death.Play();
        }

    }
}


