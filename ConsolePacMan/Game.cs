using ConsolePacMan.GameClasses;
using System;
using System.Threading.Tasks;
using System.Media;
using System.Threading;

namespace ConsolePacMan
{
    class Game
    {
        // Global Declarations

        static Random random = new Random();
        static bool gamePaused = false;
        static bool pausedTextIsShown = false;
        static bool continueLoop = true;

        // Player
        static PacMan pacman = new PacMan();

        // Ghosts
        static Ghost[] ghostList =
        {
            new Ghost(ConsoleColor.DarkRed,15,8),
            new Ghost(ConsoleColor.DarkYellow,16,12),
            new Ghost(ConsoleColor.White,17,12),
            new Ghost(ConsoleColor.Blue,18,12),

        };

        // Game Board
        static GameBoard board = new GameBoard();
        static string[,] border = board.GetBoard;

        // Console Settings
        const int GameWidth = 70;
        const int GameHeight = 30;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "Console PacMan";
            Console.WindowWidth = GameWidth;
            Console.BufferWidth = GameWidth;
            Console.WindowHeight = GameHeight;
            Console.BufferHeight = GameHeight;
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            ShowWelcomeMenu();

            RedrawBoard();
            LoadGUI();

            LoadPlayer();

            LoadGhosts();

            while (continueLoop)
            {

                ReadUserKey();

                // Check if paused
                if (gamePaused)
                {
                    BlinkPausedText();
                    continue;
                }

                GhostAi();

                PlayerMovement();

                CheckIfNoLives();

                CheckScore();

                Thread.Sleep(200);
            }
        }

        static void LoadGUI()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(40, 2);
            Console.Write("Level: {0}", pacman.GetLevel());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(40, 4);
            Console.Write("Score: {0}", pacman.GetScore());
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(40, 6);
            Console.Write("Lives: {0}", pacman.Lives());
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.SetCursorPosition(40, GameHeight - 8);
            Console.Write("{0}", new string('-', 29));
            Console.SetCursorPosition(40, GameHeight - 7);
            Console.Write("|  WCIŚNIJ P ABY ZAPUZOWAĆ  |");
            Console.SetCursorPosition(40, GameHeight - 6);
            Console.Write("| WCIŚNIJ ESC ABY ZAKOŃCZYĆ |");
            Console.SetCursorPosition(40, GameHeight - 5);
            Console.Write("{0}", new string('-', 29));
        }

        static void LoadPlayer()
        {
            Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
            Console.ForegroundColor = pacman.GetColor();
            Console.Write(pacman.GetSymbol());
        }

        static void LoadGhosts()
        {
            foreach (var ghost in ghostList)
            {
                Console.ForegroundColor = ghost.GetColor();
                Console.SetCursorPosition(ghost.GetPosX(), ghost.GetPosY());
                Console.Write(ghost.GetSymbol());
            }

        }
        static void MoveGhost()
        {
            foreach (var ghost in ghostList)
            {

                Console.ForegroundColor = ghost.GetColor();
                Console.SetCursorPosition(ghost.GetPosX(), ghost.GetPosY());
                Console.Write(ghost.GetSymbol());
                Console.ForegroundColor = ConsoleColor.White;
                if (ghost.GetPosX() != ghost.prevPosX || ghost.GetPosY() != ghost.prevPosY)
                {
                    if (border[ghost.prevPosY, ghost.prevPosX] == " ")
                    {
                        Console.SetCursorPosition(ghost.prevPosX, ghost.prevPosY);
                        Console.Write(' ');
                    }
                    else if (border[ghost.prevPosY, ghost.prevPosX] == ".")
                    {
                        Console.SetCursorPosition(ghost.prevPosX, ghost.prevPosY);
                        Console.Write('.');
                    }
                    else if (border[ghost.prevPosY, ghost.prevPosX] == "*")
                    {
                        Console.SetCursorPosition(ghost.prevPosX, ghost.prevPosY);
                        Console.Write('*');
                    }
                }
            }

        }

        static void ReadUserKey()
        {
            if (Console.KeyAvailable)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Escape:
                        continueLoop = false; // Loop break
                        GameOver();
                        break;
                    case ConsoleKey.P:
                        SetGamePaused();
                        break;
                    case ConsoleKey.UpArrow:
                        pacman.NextDirection = "up";
                        break;
                    case ConsoleKey.DownArrow:
                        pacman.NextDirection = "down";
                        break;
                    case ConsoleKey.LeftArrow:
                        pacman.NextDirection = "left";
                        break;
                    case ConsoleKey.RightArrow:
                        pacman.NextDirection = "right";
                        break;
                }
            }
        }

        static void SetGamePaused()
        {
            switch (gamePaused)
            {
                case false:
                    ShowPausedText(true);
                    break;
                case true:
                    ShowPausedText(false);
                    break;
            }

            gamePaused = gamePaused ? false : true;
        }

        static void BlinkPausedText()
        {
            switch (pausedTextIsShown)
            {
                case true:
                    Thread.Sleep(800);
                    ShowPausedText(false);
                    break;
                case false:
                    Thread.Sleep(800);
                    ShowPausedText(true);
                    break;
            }
        }

        static void ShowPausedText(bool showText)
        {
            switch (showText)
            {
                case true:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(47, GameHeight - 2);
                    Console.Write("PAUZA");
                    pausedTextIsShown = true;
                    break;
                case false:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(47, GameHeight - 2);
                    Console.Write("      ");
                    pausedTextIsShown = false;
                    break;
            }
        }

        static void PlayerMovement()
        {
            switch (pacman.CheckCell(border, pacman.NextDirection, ghostList))
            {
                case BoardElements.Dot:
                    MovePlayer(pacman.NextDirection);
                    pacman.EarnPoint();
                    pacman.Direction = pacman.NextDirection;
                    LoadGUI();
                    break;
                case BoardElements.Star:
                    MovePlayer(pacman.NextDirection);
                    pacman.EarnStar();
                    pacman.Direction = pacman.NextDirection;
                    LoadGUI();
                    break;
                case BoardElements.Empty:
                    MovePlayer(pacman.NextDirection);
                    pacman.Direction = pacman.NextDirection;
                    break;
                case BoardElements.Ghost:
                    pacman.LoseLife();
                    MovePlayer("reset");
                    LoadGUI();
                    break;
                case BoardElements.Wall:
                    switch (pacman.CheckCell(border, pacman.Direction, ghostList))
                    {
                        case BoardElements.Dot:
                            MovePlayer(pacman.Direction);
                            pacman.EarnPoint();
                            LoadGUI();
                            break;
                        case BoardElements.Star:
                            MovePlayer(pacman.Direction);
                            pacman.EarnStar();
                            LoadGUI();
                            break;
                        case BoardElements.Empty:
                            MovePlayer(pacman.Direction);
                            break;
                        case BoardElements.Ghost:
                            pacman.LoseLife();
                            MovePlayer("reset");
                            LoadGUI();
                            break;
                        case BoardElements.Wall:
                            break;
                    }
                    break;
            }
        }


        static void MovePlayer(string direction)
        {
            switch (direction)
            {
                case "up":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY() - 1);
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    pacman.MoveUp();
                    break;
                case "right":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    Console.SetCursorPosition(pacman.GetPosX() + 1, pacman.GetPosY());
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    pacman.MoveRight();
                    break;
                case "down":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY() + 1);
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    pacman.MoveDown();
                    break;
                case "left":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    Console.SetCursorPosition(pacman.GetPosX() - 1, pacman.GetPosY());
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    pacman.MoveLeft();
                    break;
                case "reset":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    pacman.ResetPacMan();
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    break;
            }
        }

        static void GhostAi()
        {
            for (int i = 0; i < ghostList.Length; i++)
            {
                if (random.Next(0, 2) != 0)
                {
                    ghostList[i].Direction = Ghost.possibleDirections[random.Next(0, Ghost.possibleDirections.Length)];
                }
                switch (ghostList[i].Direction)
                {
                    case "left":
                        if (ghostList[i].CheckLeftCell(ghostList, ghostList[i].GetPosX(), ghostList[i].GetPosY(), border))
                        {
                            ghostList[i].MoveLeft();
                            //MoveGhost();
                            if (ghostList[i].GetPosX() == pacman.GetPosX() && ghostList[i].GetPosY() == pacman.GetPosY())
                            {
                                pacman.LoseLife();
                                MovePlayer("reset");
                                LoadGUI();
                            }
                        }
                        break;
                    case "right":
                        if (ghostList[i].CheckRightCell(ghostList, ghostList[i].GetPosX(), ghostList[i].GetPosY(), border))
                        {
                            ghostList[i].MoveRight();
                            //MoveGhost();
                            if (ghostList[i].GetPosX() == pacman.GetPosX() && ghostList[i].GetPosY() == pacman.GetPosY())
                            {
                                pacman.LoseLife();
                                MovePlayer("reset");
                                LoadGUI();
                            }
                        }
                        break;
                    case "up":
                        if (ghostList[i].CheckUpCell(ghostList, ghostList[i].GetPosX(), ghostList[i].GetPosY(), border))
                        {
                            ghostList[i].MoveUp();
                            //MoveGhost();
                            if (ghostList[i].GetPosX() == pacman.GetPosX() && ghostList[i].GetPosY() == pacman.GetPosY())
                            {
                                pacman.LoseLife();
                                MovePlayer("reset");
                                LoadGUI();
                            }
                        }
                        break;
                    case "down":
                        if (ghostList[i].CheckDownCell(ghostList, ghostList[i].GetPosX(), ghostList[i].GetPosY(), border))
                        {
                            ghostList[i].MoveDown();
                            //MoveGhost();
                            if (ghostList[i].GetPosX() == pacman.GetPosX() && ghostList[i].GetPosY() == pacman.GetPosY())
                            {
                                pacman.LoseLife();
                                MovePlayer("reset");
                                LoadGUI();
                            }
                        }
                        break;

                }
            }

            MoveGhost();
        }

        static void CheckScore()
        {
            if (pacman.GetScore() == 684)
            {
                continueLoop = false;
                WinGame();
            }
        }

        static void CheckIfNoLives()
        {
            if (pacman.Lives() < 0)
            {
                continueLoop = false;
                GameOver();
            }

        }

        static void RedrawBoard()
        {
            for (int i = 0; i < board.GetBoard.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetBoard.GetLength(1); j++)
                {
                    Console.Write("{0}", board.GetBoard[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void ChangeBoard()
        {
            border[pacman.GetPosY(), pacman.GetPosX()] = " ";
        }

        static void ShowWelcomeMenu()
        {
            PlayMusic();
            RedrawBoard();

            int horizontalPos = GameHeight / 2 - 2;
            int verticalPos = GameWidth / 2 - 15;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(verticalPos, horizontalPos);
            Console.Write("|{0}|", new string('-', 31));
            Console.SetCursorPosition(verticalPos, horizontalPos + 1);
            Console.Write("|    WCIŚNIJ X ABY ROZPOCZĄĆ    |");
            Console.SetCursorPosition(verticalPos, horizontalPos + 2);
            Console.Write("|   WCIŚNIJ ESC ABY ZAKOŃCZYĆ   |");
            Console.SetCursorPosition(verticalPos, horizontalPos + 3);
            Console.Write("|{0}|", new string('-', 31));
            Console.ForegroundColor = ConsoleColor.White;

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);
            while (true)
            {
                if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                else if (keyPressed.Key == ConsoleKey.X)
                {
                    Console.Clear();
                    break;
                }

                keyPressed = Console.ReadKey(true);
            }
        }

        static void GameOver()
        {
            Console.Clear();
            RedrawBoard();

            int horizontalPos = GameHeight / 2 - 2;
            int verticalPos = GameWidth / 2 - 15;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(verticalPos, horizontalPos);
            Console.Write("|{0}|", new string('-', 27));
            Console.SetCursorPosition(verticalPos, horizontalPos + 1);
            Console.Write("||        KONIEC GRY       ||");
            Console.SetCursorPosition(verticalPos, horizontalPos + 2);
            Console.Write("||                         ||");
            Console.SetCursorPosition(verticalPos, horizontalPos + 3);
            int score = pacman.GetScore();
            Console.Write("||        WYNIK: {0}{1}  ||", score, new string(' ', 9 - score.ToString().Length));
            Console.SetCursorPosition(verticalPos, horizontalPos + 4);
            Console.Write("|{0}|", new string('-', 27));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, GameHeight - 1);

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);
            while (true)
            {
                if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }

                keyPressed = Console.ReadKey(true);
            }
        }

        static void WinGame()
        {
            Console.Clear();
            RedrawBoard();

            int horizontalPos = GameHeight / 2 - 2;
            int verticalPos = GameWidth / 2 - 15;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(verticalPos, horizontalPos);
            Console.Write("|{0}|", new string('-', 27));
            Console.SetCursorPosition(verticalPos, horizontalPos + 1);
            Console.Write("|        WYGRAŁEŚ!          |");
            Console.SetCursorPosition(verticalPos, horizontalPos + 2);
            Console.Write("|                           |");
            Console.SetCursorPosition(verticalPos, horizontalPos + 3);
            int score = pacman.GetScore();
            Console.Write("|       WYNIK: {0}{1}    |", score, new string(' ', 9 - score.ToString().Length));
            Console.SetCursorPosition(verticalPos, horizontalPos + 4);
            Console.Write("|                           |");
            Console.SetCursorPosition(verticalPos, horizontalPos + 5);
            Console.Write("| WCIŚNIJ ESC ABY ZAKOŃCZYĆ |");
            Console.SetCursorPosition(verticalPos, horizontalPos + 6);
            Console.Write("|{0}|", new string('-', 27));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, GameHeight - 1);

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);
            while (true)
            {
                if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }

                keyPressed = Console.ReadKey(true);
            }
        }

        public static void PlayMusic()
        {
            Task.Factory.StartNew(() => Music());
        }

        public static void Music()
        {

            SoundPlayer PacManMusic = new SoundPlayer(ConsolePacMan.PacManMusic.pacman_beginning);
            PacManMusic.Play();

        }
    }
}
