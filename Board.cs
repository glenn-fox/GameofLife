class Board
{
    int board_size;
    bool wrap_around = true;

    bool[,] frame;
    (int X, int Y) cursor = (0, 0);

    public Board(int size)
    {
        board_size = size;
        frame = new bool[board_size, board_size];
    }

    public void ClearBoard()
    {
        frame = new bool[board_size, board_size];
    }

    public int GetCursorX()
    {
        return cursor.X;
    }

    public int GetCursorY()
    {
        return cursor.Y;
    }

    public void ToggleWrap()
    {
        if (wrap_around)
        {
            wrap_around = false;
        }
        else
        {
            wrap_around = true;
        }
    }

    public void PrintFrame()
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < board_size; y++)
        {
            for (int x = 0; x < board_size; x++)
            {
                Console.Write(frame[x, y] ? "██" : "  ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Use arrow keys and Spacebar to select cells and ESC to quit");
        Console.WriteLine("Wrap aroung is currently " + wrap_around + ". Use T to toggle    ");
    }

    public void MoveCursor(string direction)
    {
        switch (direction)
        {
            case "up":
                cursor.Y = (cursor.Y - 1 + board_size) % board_size;
                break;
            case "down":
                cursor.Y = (cursor.Y + 1 + board_size) % board_size;
                break;
            case "left":
                cursor.X = (cursor.X - 1 + board_size) % board_size;
                break;
            case "right":
                cursor.X = (cursor.X + 1 + board_size) % board_size;
                break;
        }
    }

    public void ToggleCell()
    {
        if (frame[cursor.X, cursor.Y])
        {
            frame[cursor.X, cursor.Y] = false;
        }
        else
        {
            frame[cursor.X, cursor.Y] = true;
        }
    }

    public bool IsAlive(int x, int y)
    {
        return frame[x, y];
    }

    public int GetNumNeighbors(int x, int y)
    {
        int liveNeighbors = 0;
        foreach (var offset in GameRules.Neighbors)
        {
            int nx = x + offset.row;
            int ny = y + offset.col;
            if (wrap_around)
            {
                nx = (nx + board_size) % board_size;
                ny = (ny + board_size) % board_size;
            }
            if ((nx >= 0 && nx < board_size) && (ny >= 0 && ny < board_size) && IsAlive(nx, ny))
                liveNeighbors++;
        }
        return liveNeighbors;
    }

    public void GetNextGeneration()
    {
        bool[,] new_frame = new bool[board_size, board_size];

        int liveNeighbors = 0;
        for (int i = 0; i < board_size * board_size; i++)
        {
            int x = i % board_size;
            int y = i / board_size;

            liveNeighbors = GetNumNeighbors(x, y);

            if (frame[x, y])
            {
                if (liveNeighbors < 2 || liveNeighbors > 3)
                {
                    new_frame[x, y] = false;
                }
                else
                {
                    new_frame[x, y] = true;
                }
            }
            else if (liveNeighbors == 3)
            {
                new_frame[x, y] = true;
            }
        }
        frame = new_frame;
    }
}
