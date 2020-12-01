
using System;

namespace _2048
{
    public class Game
    {
        private bool done;
        private bool win;
        private bool moved;
        private int score;
        private Tile[,] board = new Tile[4, 4];
        private Random random = new Random();

        public Game()
        {
            done = false;
            win = false;
            moved = true;
            score = 0;
            
            for(int x = 0; x < 4; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    board[x, y] = new Tile();
                }
            }
        }

        public void Begin()
        {
            AddTile();
            while(true)
            {
                if(moved)
                {
                    AddTile();
                }

                DrawBoard();

                if(done)
                {
                    break;
                }

                WaitInput();
            }

            string s = win ? "You Won!" : "Game Over!";
            Console.WriteLine(s);
        }

        private void AddTile()
        {
            for(int x = 0; x < 4; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(board[x, y].Value != 0)
                    {
                        continue;
                    }

                    int a = random.Next(0, 4);
                    int b = random.Next(0, 4);

                    while(board[a, b].Value != 0)
                    {
                        a = random.Next(0, 4);
                        b = random.Next(0, 4);
                    }

                    board[a, b].Value = random.Next(0, 100) < 90 ? 2 : 4;

                    if(CanMove())
                    {
                        return;
                    }

                    if(board[x, y].Value == 2048)
                    {
                        win = true;
                    }
                }
            }
            done = true;
        }

        private bool CanMove()
        {
            for(int x = 0; x < 4; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(board[x, y].Value == 0
                        || TestMove(x + 1, y, board[x, y].Value)
                        || TestMove(x - 1, y, board[x, y].Value)
                        || TestMove(x, y + 1, board[x, y].Value)
                        || TestMove(x, y - 1, board[x, y].Value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TestMove(int x, int y, int value)
        {
            if(x < 0 || x > 3 || y < 0 || y > 3)
            {
                return false;
            }

            return board[x, y].Value == value;
        }

        private void DrawBoard()
        {
            Console.Clear();

            for(int x = 0; x < 4; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    Console.ForegroundColor = GetNumberColor(board[x, y].Value);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" " + board[x, y].Value.ToString().PadRight(4));
                }
                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine("Score: " + score + "\n");
        }

        private static ConsoleColor GetNumberColor(int num)
        {
            switch(num)
            {
                case 0:
                    return ConsoleColor.DarkGray;
                case 2:
                    return ConsoleColor.Cyan;
                case 4:
                    return ConsoleColor.Magenta;
                case 8:
                    return ConsoleColor.Red;
                case 16:
                    return ConsoleColor.Green;
                case 32:
                    return ConsoleColor.Yellow;
                case 64:
                    return ConsoleColor.Yellow;
                case 128:
                    return ConsoleColor.DarkCyan;
                case 256:
                    return ConsoleColor.Cyan;
                case 512:
                    return ConsoleColor.DarkMagenta;
                case 1024:
                    return ConsoleColor.Magenta;
                default:
                    return ConsoleColor.Red;
            }
        }

        private void WaitInput()
        {
            moved = false;
            Console.WriteLine("Use arrow keys to move. Press Ctrl-C to exit.");
            ConsoleKeyInfo input = Console.ReadKey(true);

            switch(input.Key)
            {
                case ConsoleKey.UpArrow:

                case ConsoleKey.W:
                Move(Direction.Up);
                break;

                case ConsoleKey.LeftArrow:

                case ConsoleKey.A:
                Move(Direction.Left);
                break;

                case ConsoleKey.DownArrow:

                case ConsoleKey.S:
                Move(Direction.Down);
                break;

                case ConsoleKey.RightArrow:

                case ConsoleKey.D:
                Move(Direction.Right);
                break;
            }

            for(int x = 0; x < 4; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    board[x, y].Blocked = false;
                }
            }
        }

        private void Move(Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                for(int x = 1; x < 4; x++)
                {
                    for(int y = 0; y < 4; y++)
                    {
                        MoveVertically(x, y, 1);
                    }
                }
                break;

                case Direction.Down:
                for(int x = 2; x >= 0; x--)
                {
                    for(int y = 0; y < 4; y++)
                    {
                        MoveVertically(x, y, -1);
                    }
                }
                break;

                case Direction.Left:
                for(int x = 0; x < 4; x++)
                {
                    for(int y = 1; y < 4; y++)
                    {
                        MoveHorizontally(x, y, -1);
                    }
                }
                break;

                case Direction.Right:
                for(int x = 0; x < 4; x++)
                {
                    for(int y = 2; y >= 0; y--)
                    {
                        MoveHorizontally(x, y, 1);
                    }
                }
                break;
            }
        }

        private void MoveVertically(int x, int y, int d)
        {
            if(board[x - d, y].Value != 0
                && board[x - d, y].Value == board[x, y].Value
                && !board[x - d, y].Blocked
                && !board[x, y].Blocked)
            {
                board[x, y].Value = 0;
                board[x - d, y].Value *= 2;
                score += board[x - d, y].Value;
                board[x - d, y].Blocked = true;
                moved = true;
            }
            else
            {
                if(board[x - d, y].Value == 0 && board[x, y].Value != 0)
                {
                    board[x - d, y].Value = board[x, y].Value;
                    board[x, y].Value = 0;
                    moved = true;
                }
            }

            if(d > 0)
            {
                if(x - d > 0)
                {
                    MoveVertically(x - d, y, 1);
                }
            }
            else
            {
                if(x - d < 3)
                {
                    MoveVertically(x - d, y, -1);
                }
            }
        }

        private void MoveHorizontally(int x, int y, int d)
        {
            if(board[x, y + d].Value != 0
                && board[x, y + d].Value == board[x, y].Value
                && !board[x, y + d].Blocked
                && !board[x, y].Blocked)
            {
                board[x, y].Value = 0;
                board[x, y + d].Value *= 2;
                score += board[x, y + d].Value;
                board[x, y + d].Blocked = true;
                moved = true;
            }
            else
            {
                if(board[x, y + d].Value == 0 && board[x, y].Value != 0)
                {
                    board[x, y + d].Value = board[x, y].Value;
                    board[x, y].Value = 0;
                    moved = true;
                }
            }

            if(d > 0)
            {
                if(y + d < 3)
                {
                    MoveHorizontally(x, y + d, 1);
                }
            }
            else
            {
                if(y + d > 0)
                {
                    MoveHorizontally(x, y + d, -1);
                }
            }
        }
    }
}
