using System.IO;

class GameOfLife
{
    static void GenerateNextFrame(bool[,] frame)
    {
        int size = frame.GetLength(0);
        bool[,] new_frame = new bool[size, size];
    }

    static (int X, int Y) MoveCursor(string direction, int size, (int X, int Y) cursor)
    {
        switch (direction)
        {
            case "up":
                cursor.Y = (cursor.Y + size - 1) % size;
                break;
            case "down":
                cursor.Y = (cursor.Y + 1) % size;
                break;
            case "left":
                cursor.X = (cursor.X - 1 + size) % size;
                break;
            case "right":
                cursor.X = (cursor.X + 1 + size) % size;
                break;
        }
        return cursor;
    }

    public enum GameAction
    {
        None,
        ToggleCell,
        ToggleWrap,
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
        Start,
        Clear,
        Quit,
    }

    static void DisplayRules()
    {
        String? line;
        try
        {
            StreamReader sr = new StreamReader("rules.txt");
            line = sr.ReadLine();
            while (line != null)
            {
                Console.WriteLine(line);
                line = sr.ReadLine();
            }
            sr.Close();
            Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    static int Menu()
    {
        Console.WriteLine("Welcome to the Game of Life!");
        Console.WriteLine("(1) Start Playing");
        Console.WriteLine("(2) Read Rules");
        Console.WriteLine("Enter a choice: ");
        String? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                return 1;
            case "2":
                return 2;
            case "9":
                return 9;
            default:
                return 0;
        }
    }

    static GameAction GetKeyPress()
    {
        ConsoleKeyInfo cki;

        cki = Console.ReadKey(true);

        switch (cki.Key)
        {
            case ConsoleKey.Escape:
                return GameAction.Quit;
            case ConsoleKey.UpArrow:
            case ConsoleKey.K:
                return GameAction.MoveUp;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.H:
                return GameAction.MoveLeft;
            case ConsoleKey.DownArrow:
            case ConsoleKey.J:
                return GameAction.MoveDown;
            case ConsoleKey.RightArrow:
            case ConsoleKey.L:
                return GameAction.MoveRight;
            case ConsoleKey.Spacebar:
                return GameAction.ToggleCell;
            case ConsoleKey.C:
                return GameAction.Clear;
            case ConsoleKey.T:
                return GameAction.ToggleWrap;
            case ConsoleKey.Enter:
                return GameAction.Start;
            default:
                return GameAction.None;
        }
    }

    static void Main()
    {
        int board_size = 40;
        Board board = new Board(board_size);

        // clear screen before first run
        Console.Clear();
        GameAction action = GameAction.None;

        do
        {
            board.PrintFrame();
            //PrintFrame(frame);
            // double x since the x characters are doubled
            Console.SetCursorPosition(board.GetCursorX() * 2, board.GetCursorY());
            Console.CursorVisible = true;

            //action = GetKeyPress();

            // hides characters from input
            //cki = Console.ReadKey(true);
            while (Console.KeyAvailable)
            {
                action = GetKeyPress();
                switch (action)
                {
                    case GameAction.MoveUp:
                        board.MoveCursor("up");
                        break;
                    case GameAction.MoveDown:
                        board.MoveCursor("down");
                        break;
                    case GameAction.MoveLeft:
                        board.MoveCursor("left");
                        break;
                    case GameAction.MoveRight:
                        board.MoveCursor("right");
                        break;
                    case GameAction.Clear:
                        board.ClearBoard();
                        break;
                    case GameAction.ToggleCell:
                        board.ToggleCell();
                        break;
                    case GameAction.ToggleWrap:
                        board.ToggleWrap();
                        break;
                    case GameAction.Start:
                        do
                        {
                            while (!Console.KeyAvailable)
                            {
                                board.GetNextGeneration();
                                board.PrintFrame();
                                Thread.Sleep(100);
                            }
                        } while (Console.ReadKey(true).Key != ConsoleKey.Enter);
                        break;
                }
            }
            Thread.Sleep(30);
        } while (action != GameAction.Quit);
        Console.Clear();
    }
}
