using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;



internal class Program
{
    static Random random = new Random();
    static void PrintBoard(char[,] board)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.SetCursorPosition(j, i);
                if (board[i, j] == 'P')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("P");
                    Console.ResetColor();
                }

                else if (board[i, j] == 'C')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("C");
                    Console.ResetColor();
                }else if (board[i,j] == ':')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(":");
                    Console.ResetColor();
                }

                else
                    Console.Write(board[i, j]);

                if (colors[i, j] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(j, i);
                    Console.Write(board[i, j]);
                    Console.ResetColor();
                }
                if (colors[i, j] == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(j, i);
                    Console.Write(board[i, j]);
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
        }
    }
    static int[,] colors = new int[19, 33]; // color array for random piece and squares
    static char[,] GenerateRandomPiece3() //3 line random piece
    {
        char[,] randompiece = new char[5, 5];
        for (int i = 0; i < randompiece.GetLength(0); i++)
        {
            for (int j = 0; j < randompiece.GetLength(1); j++)
            {
                if (i % 2 == 0 && j % 2 == 0)
                    randompiece[i, j] = '+';
                else
                    randompiece[i, j] = ' ';
            }
        }
        // if random piece is connected
        bool connected = false;
        while (!connected)
        {
            char[,] temp = (char[,])randompiece.Clone();
            int stars = 0;
            int lines = 0;
            while (lines < 3)
            {
                int xx, yy;
                while (true)
                {
                    xx = random.Next(0, 5);
                    yy = random.Next(0, 5);
                    if (temp[yy, xx] == ' ') break;
                }
                if (yy % 2 == 0 && xx % 2 == 1)
                {
                    temp[yy, xx] = '-';
                    lines++;
                    if (xx + 1 < 5 && temp[yy, xx + 1] == '+') //Right star control
                    {
                        temp[yy, xx + 1] = '*';
                        stars++;
                    }
                    if (xx - 1 >= 0 && temp[yy, xx - 1] == '+') //Left star control
                    {
                        temp[yy, xx - 1] = '*';
                        stars++;
                    }
                }
                else if (yy % 2 == 1 && xx % 2 == 0)
                {
                    temp[yy, xx] = '|';
                    lines++;
                    if (yy + 1 < 5 && temp[yy + 1, xx] == '+') //Upward star control
                    {
                        temp[yy + 1, xx] = '*';
                        stars++;
                    }
                    if (yy - 1 >= 0 && temp[yy - 1, xx] == '+') //Downward star control
                    {
                        temp[yy - 1, xx] = '*';
                        stars++;
                    }
                }
            }
            if (stars == 4 && lines == 3) //connected condition with temp array
            {
                connected = true;
                randompiece = temp;
            }
        }
        return randompiece;
    }
    static char[,] GenerateRandomPiece2() // 2 line random piece
    {
        char[,] randompiece = new char[5, 5];
        for (int i = 0; i < randompiece.GetLength(0); i++)
        {
            for (int j = 0; j < randompiece.GetLength(1); j++)
            {
                if (i % 2 == 0 && j % 2 == 0)
                    randompiece[i, j] = '+';
                else
                    randompiece[i, j] = ' ';
            }
        }
        // if random piece is connected
        bool connected = false;
        while (!connected)
        {
            char[,] temp = (char[,])randompiece.Clone();
            int stars = 0;
            int lines = 0;
            while (lines < 2)
            {
                int xx, yy;
                while (true)
                {
                    xx = random.Next(0, 5);
                    yy = random.Next(0, 5);
                    if (temp[yy, xx] == ' ') break;
                }
                if (yy % 2 == 0 && xx % 2 == 1)
                {
                    temp[yy, xx] = '-';
                    lines++;
                    if (xx + 1 < 5 && temp[yy, xx + 1] == '+') //Right star control
                    {
                        temp[yy, xx + 1] = '*';
                        stars++;
                    }
                    if (xx - 1 >= 0 && temp[yy, xx - 1] == '+') //Left star control
                    {
                        temp[yy, xx - 1] = '*';
                        stars++;
                    }
                }
                else if (yy % 2 == 1 && xx % 2 == 0)
                {
                    temp[yy, xx] = '|';
                    lines++;
                    if (yy + 1 < 5 && temp[yy + 1, xx] == '+') //Upward star control
                    {
                        temp[yy + 1, xx] = '*';
                        stars++;
                    }
                    if (yy - 1 >= 0 && temp[yy - 1, xx] == '+') //Downward star control
                    {
                        temp[yy - 1, xx] = '*';
                        stars++;
                    }
                }
            }
            if (stars == 3 && lines == 2) // connected condition with temp array
            {
                connected = true;
                randompiece = temp;
            }
        }
        return randompiece;
    }
    static char[,] GenerateRandomPiece1() // 1 line random piece
    {
        char[,] randompiece = new char[5, 5];
        for (int i = 0; i < randompiece.GetLength(0); i++)
        {
            for (int j = 0; j < randompiece.GetLength(1); j++)
            {
                if (i % 2 == 0 && j % 2 == 0)
                    randompiece[i, j] = '+';
                else
                    randompiece[i, j] = ' ';
            }
        }
        char[,] temp = (char[,])randompiece.Clone();
        int lines = 0;
        while (lines < 1)
        {
            int xx, yy;
            while (true)
            {
                xx = random.Next(0, 5);
                yy = random.Next(0, 5);
                if (temp[yy, xx] == ' ') break;
            }
            if (yy % 2 == 0 && xx % 2 == 1)
            {
                temp[yy, xx] = '-';
                lines++;
            }
            else if (yy % 2 == 1 && xx % 2 == 0)
            {
                temp[yy, xx] = '|';
                lines++;
            }
        }
        randompiece = temp;
        return randompiece;
    }
    static char[,] GenerateShiftPiece(char[,] randompiece)
    {
        while (randompiece[0, 1] == ' ' && randompiece[0, 3] == ' ' && randompiece[1, 0] == ' ' && randompiece[1, 2] == ' ' && randompiece[1, 4] == ' ')
        {
            // Checking First Two Rows for shifting
            randompiece[0, 1] = randompiece[2, 1];
            randompiece[0, 3] = randompiece[2, 3];
            randompiece[1, 0] = randompiece[3, 0];
            randompiece[1, 2] = randompiece[3, 2];
            randompiece[1, 4] = randompiece[3, 4];
            randompiece[2, 1] = randompiece[4, 1];
            randompiece[2, 3] = randompiece[4, 3];
            // Clearing last two rows
            randompiece[3, 0] = ' ';
            randompiece[3, 2] = ' ';
            randompiece[3, 4] = ' ';
            randompiece[4, 1] = ' ';
            randompiece[4, 3] = ' ';
        }
        while (randompiece[1, 0] == ' ' && randompiece[3, 0] == ' ' && randompiece[0, 1] == ' ' && randompiece[2, 1] == ' ' && randompiece[4, 1] == ' ')
        {
            // Checking First Two Columns for shifting
            randompiece[1, 0] = randompiece[1, 2];
            randompiece[3, 0] = randompiece[3, 2];
            randompiece[0, 1] = randompiece[0, 3];
            randompiece[2, 1] = randompiece[2, 3];
            randompiece[4, 1] = randompiece[4, 3];
            randompiece[1, 2] = randompiece[1, 4];
            randompiece[3, 2] = randompiece[3, 4];
            // Clearing last two columns
            randompiece[0, 3] = ' ';
            randompiece[2, 3] = ' ';
            randompiece[4, 3] = ' ';
            randompiece[1, 4] = ' ';
            randompiece[3, 4] = ' ';
        }
        return randompiece;
    }
    static char[,] PlacePieceOnBoard(char[,] board, char[,] piece)
    {
        int maxTries = 100;
        int maxRows = board.GetLength(0);
        int maxCols = board.GetLength(1);
        int pieceRows = piece.GetLength(0);
        int pieceCols = piece.GetLength(1);

        int[] rowOffsets = new int[25];  //Controlling random pieces area for every coordiantes
        int[] colOffsets = new int[25];
        int offsetCount = 0; // Checking occupied areas in array

        for (int i = 0; i < pieceRows; i++)
        {
            for (int j = 0; j < pieceCols; j++)
            {
                if (piece[i, j] == '-' || piece[i, j] == '|')
                {
                    //Taking occupied coordinates in areas. 
                    rowOffsets[offsetCount] = i;
                    colOffsets[offsetCount] = j;
                    offsetCount++;
                }
            }
        }
        for (int attempt = 0; attempt < maxTries; attempt++)
        {
            //For choosing even Coordinates (+)
            int startRow = 2 * random.Next(0, 9);
            int startCol = 2 * random.Next(0, 16);

            bool canPlace = true;
            for (int i = 0; i < offsetCount; i++)
            {
                int targetRow = startRow + rowOffsets[i];
                int targetCol = startCol + colOffsets[i];
                if (targetRow < 0 || targetRow >= maxRows || targetCol < 0 || targetCol >= maxCols) // Checking board for owerflow
                {
                    canPlace = false;
                    break;
                }
                if (targetRow < maxRows && targetCol < maxCols)
                {
                    if (board[targetRow, targetCol] != ' ') //Checking board for empty areas to placing random pieces
                    {
                        canPlace = false;
                        break;
                    }
                }
            }
            if (canPlace)
            {
                for (int i = 0; i < offsetCount; i++)
                {
                    int targetRow = startRow + rowOffsets[i];
                    int targetCol = startCol + colOffsets[i];
                    int a = rowOffsets[i];
                    int b = colOffsets[i];
                    char c = piece[a, b]; //taking lines on random pieces
                    board[targetRow, targetCol] = c;
                    colors[targetRow, targetCol] = 2;
                }
                return board;
            }
        }
        return board;
    }
    static void printt(char[,] board)
    {
        int c = 0, p = 0, o = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == 'C')
                {
                    c++;
                }
                if (board[i, j] == 'P')
                {
                    p++;
                }
                if (board[i, j] == ':')
                {
                    o++;
                }

            }
        }

        Console.SetCursorPosition(40, 7);
        Console.WriteLine("H.Squares : " + p);
        Console.SetCursorPosition(40, 8);
        Console.WriteLine("C.Squares : " + c);
        Console.SetCursorPosition(40, 9);
        Console.WriteLine("Ownerless : " + o);
    }
    static void printLine(int line)
    {
        Console.SetCursorPosition(40, 11); Console.WriteLine("Random Piece : " + line + " lines");

    }

    static void printScore(int computerScore, int humanScore)
    {
        Console.SetCursorPosition(40, 4); Console.WriteLine("H.Score : " + humanScore);
        Console.SetCursorPosition(40, 5); Console.WriteLine("C.Score : " + computerScore);

    }

    static void PrintShape(char[,] rpiece, int stagenum) // print random piece 
    {
        if (stagenum == 3 || stagenum == 6)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (rpiece[i, j] == '*')
                    {
                        rpiece[i, j] = '+';
                    }
                    Console.SetCursorPosition(40 + j, 13 + i);
                    Console.Write(rpiece[i, j]);
                }
            }
        }
    }
    static void ComputerPrint(int bestX, int bestY, int bestPoint, int[] Direction, int l)
    {
        Console.SetCursorPosition(40, 11);
        Console.WriteLine("--- Computer AI ---");
        Console.SetCursorPosition(40, 12);
        Console.WriteLine("BestPoint : " + bestPoint);
        Console.SetCursorPosition(40, 13);
        Console.WriteLine("BestX : " + bestX);
        Console.SetCursorPosition(40, 14);
        Console.WriteLine("BestY : " + bestY);
        Console.SetCursorPosition(40, 15);
        Console.WriteLine("BestDirection : ");
        Console.SetCursorPosition(40, 16);

        if (l!=1 || l!=0 )
        {
            for (int i = 0; i < l-1; i++)
            {
                if (i == l - 2)
                {
                    Console.Write(Direction[l - 2]);
                    break;
                }
                Console.Write(Direction[i]);
                Console.Write(",");
            }

        }
        else
        {
            Console.Write(' '); 
        }

    }

    static void PrintState(int round, int stage, String turn)//right of board informations
    {
        Console.SetCursorPosition(40, 0);
        Console.WriteLine("Round:" + " " + round);
        Console.SetCursorPosition(40, 1);
        Console.WriteLine("Turn:" + " " + turn); 
        Console.SetCursorPosition(40, 2);
        Console.WriteLine("Stage:" + " " + stage);
    }
    static char[,] kareYapma(char[,] board, int x, int y)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        if (board[x, y] == ' ')
        {
            if (x + 1 < rows && board[x + 1, y] == ' ')
                board[x + 1, y] = '-';

            if (x - 1 >= 0 && board[x - 1, y] == ' ')
                board[x - 1, y] = '-';

            if (y + 1 < cols && board[x, y + 1] == ' ')
                board[x, y + 1] = '|';

            if (y - 1 >= 0 && board[x, y - 1] == ' ')
                board[x, y - 1] = '|';

            board[x, y] = 'C';
        }
        return board;
    }
    static char[,] ownerlesscheck(char[,] board)
    {
        for (int i = 0; i < 18; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                if (board[i, j + 1] == ' ' && board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' && board[i, j + 2] == '|')
                {
                    board[i, j + 1] = ':';
                }
            }
        }
        return board;
    }
    static char[,] ownerlesscheck2(char[,] board)
    {
        for (int i = 0; i < 18; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                if (board[i, j + 1] == ' ' && board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' && board[i, j + 2] == '|')
                {
                    board[i, j + 1] = ':';
                    colors[i, j + 1] = 2;
                }
            }
        }
        return board;
    }

    static char[,] çizgikoyma(char[,] board, int x, int y)
    {
        int m = 0;
        int[] compstagelinerow = new int[1000];
        int[] compStagelinecol = new int[1000];
        char[] compstageline = new char[1000];

        if (board[x, y] == ' ')
        {
            if (x > 0 && board[x - 1, y] == ' ')
            {
                m++;
                compstagelinerow[m] = x - 1;
                compStagelinecol[m] = y;
                compstageline[m] = '-';
            }
            if (x < board.GetLength(0) - 1 && board[x + 1, y] == ' ')
            {
                m++;
                compstagelinerow[m] = x + 1;
                compStagelinecol[m] = y;
                compstageline[m] = '-';
            }
            if (y > 0 && board[x, y - 1] == ' ')
            {
                m++;
                compstagelinerow[m] = x;
                compStagelinecol[m] = y - 1;
                compstageline[m] = '|';
            }
            if (y < board.GetLength(1) - 1 && board[x, y + 1] == ' ')
            {
                m++;
                compstagelinerow[m] = x;
                compStagelinecol[m] = y + 1;
                compstageline[m] = '|';
            }
        }
        int randomM = random.Next(m) + 1;

        board[compstagelinerow[randomM], compStagelinecol[randomM]] = compstageline[randomM];
        colors[compstagelinerow[randomM], compStagelinecol[randomM]] = 1;

        return board;
    }
    static bool AreBoardsEqual(char[,] board1, char[,] board2)
    {
        for (int i = 0; i < board1.GetLength(0); i++)
        {
            for (int j = 0; j < board1.GetLength(1); j++)
            {
                if (board1[i, j] != board2[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }
    static bool gameOver(char[,] board)
    {
        int counter = 0;
        bool gameOver = true;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (i % 2 == 1 && j % 2 == 1)
                {
                    if (board[i, j] != ' ')
                    {
                        counter++;
                    }
                }
            }
        }
        if (counter == 144)
        {
            gameOver = false;
        }
        return gameOver;
    }
    static int[,] compboard(char[,] board)
    {
        int[,] comp = new int[19, 33];

        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 31; j++)
            {
                if (board[i, j] == '+')
                {
                    int counter = 0;
                    if (j + 1 < 33 && board[i, j + 1] == '-') // Top side
                    {
                        counter++;
                    }

                    if (i + 2 < 19 && j + 1 < 33 && board[i + 2, j + 1] == '-') // Bottom side
                    {
                        counter++;
                    }

                    if (i + 1 < 19 && board[i + 1, j] == '|') // Left side
                    {
                        counter++;
                    }

                    if (j + 2 < 33 && i + 1 < 19 && board[i + 1, j + 2] == '|') // Right side
                    {
                        counter++;
                    }

                    if (counter == 4)
                    {
                        comp[i + 1, j + 1] = 4;
                    }

                    if (counter == 3)
                    {
                        comp[i + 1, j + 1] = 3;
                       
                    }

                    if (counter == 2)
                    {
                        comp[i + 1, j + 1] = 2;
                    }

                    if (counter == 1)
                    {
                        comp[i + 1, j + 1] = 1;
                    }
                    if (counter == 0)
                    {
                        comp[i + 1, j + 1] = 0;
                    }
                }
            }
        }
        return comp;
    }
    static void Main(string[] args)
    {
        int[,] comp = new int[19, 33];
        int tryPoint = 0;
        int humanScore = 0;
        int computerScore = 0;
        String gamemode;
        int gameMode;
        do
        {
            Console.WriteLine("Choose Game Mode");
            Console.WriteLine("1-)Easy");
            Console.WriteLine("2-)Moderate");
            Console.WriteLine("3-)Hard");
            gamemode = Console.ReadLine();

            if (int.TryParse(gamemode, out gameMode))  // Safely parse input
            {
                switch (gameMode)
                {
                    case 1:
                        tryPoint = 5;
                        break;
                    case 2:
                        tryPoint = 50;
                        break;
                    case 3:
                        tryPoint = 500;
                        break;
                }
            }
            Console.Clear();
        } while (gamemode != "1" && gamemode != "2" && gamemode != "3");

        string[] lines = File.ReadAllLines("highscore.txt"); // Dosyayı satır satır oku
        int[] scores = new int[lines.Length];
        string[] names = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            names[i] = string.Join(" ", parts.Take(parts.Length - 1));  // Adı ve soyadı birleştir
            scores[i] = int.Parse(parts.Last());  // Son eleman skordur
        }

        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();


       
        Console.Clear();

        int extraline = 0;
        int previousX = 0, previousY = 0;
        int stagenum = 1, round = 1;
        // board
        char[,] board = new char[19, 33];
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 33; j++)
            {
                if (i % 2 == 0 && j % 2 == 0)
                    board[i, j] = '+';
                else if ((i == 0 || i == 18) && j % 2 == 1)
                    board[i, j] = '-';
                else if (j == 0 || j == 32 && i % 2 == 1)
                    board[i, j] = '|';
                else
                    board[i, j] = ' ';
            }
        }

        // placing random 90 lines
        int a = 0;
        int xx, yy; //Coordinates
        do
        {
            while (true)
            {
                xx = random.Next(1, 33);
                yy = random.Next(1, 19);
                if (board[yy, xx] == ' ') break;
            }
            switch (yy % 2)
            {
                case 0 when xx % 2 == 1:
                    board[yy, xx] = '-';
                    a++;
                    break;
                case 1 when xx % 2 == 0:
                    board[yy, xx] = '|';
                    a++;
                    break;
            }
        } while (a != 90);
        // Ownerless Checking for initial board
        board = ownerlesscheck(board);
        PrintBoard(board);
        int cursorx = 1, cursory = 1;
        bool flag = true;
        bool flagger = true;
        bool gameO = true;
        bool flag4 =false;
        bool flag5 = false;
        bool flag6 = true;

        while (gameO)
        {
            ConsoleKeyInfo cki;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true); // true: do not write character 
                    switch (cki.Key)
                    {
                        case ConsoleKey.UpArrow when cursory > 1:
                            cursory--;
                            break;
                        case ConsoleKey.DownArrow when cursory < 17:
                            cursory++;
                            break;
                        case ConsoleKey.RightArrow when cursorx < 31:
                            cursorx++;
                            break;
                        case ConsoleKey.LeftArrow when cursorx > 1:
                            cursorx--;
                            break;
                    }

                    Console.SetCursorPosition(cursorx, cursory);
                    if(cki.Key == ConsoleKey.Spacebar && flag4 ==true && stagenum==1 )
                    {

                        flag5 = true;
                              flag6 = false;
                            break;
                    }
                    if (((cki.Key == ConsoleKey.Spacebar && cursorx % 2 == 0 && cursory % 2 == 1) || (cki.Key == ConsoleKey.Spacebar && cursorx % 2 == 1 && cursory % 2 == 0) ) && flag6==true)
                    {

                        if (board[cursory, cursorx] == ' ' && (stagenum == 1 || stagenum == 2))
                        {
                            flagger = true; //error handling for stage 2
                            break;
                        }

                    }
                    if (cki.Key == ConsoleKey.Enter && stagenum < 7 && stagenum != 2) //move to next stage
                    {
                        Console.Clear();
                        PrintBoard(board);
                        printScore(computerScore, humanScore);
                        printt(board);
                        flagger = false;
                        stagenum++;
                        for (int i = 0; i < colors.GetLength(0); i++)
                        {
                            for (int j = 0; j < colors.GetLength(1); j++)
                            {
                                colors[i, j] = 0;
                            }
                        }
                        if (stagenum == 7)
                        {
                            stagenum = 1;
                            PrintState(round, stagenum, "Human");
                            continue;
                        }

                        break;
                    }

                }
                Console.SetCursorPosition(cursorx, cursory);
                Thread.Sleep(0);
            }



            if (stagenum == 1)
            {
                PrintState(round, stagenum, "Human");
                PrintBoard(board);

                // clearing random piece information when we switch back to stage 1
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Console.SetCursorPosition(40 + j, 12 + i);
                        Console.Write(' ');
                    }
                }
                switch (cursory % 2)
                {
                    // Checking For forming Squares which is made by stage 1  
                    case 0:
                        if (((board[cursory + 1, cursorx - 1] == '|' && board[cursory + 1, cursorx + 1] == '|' && board[cursory + 2, cursorx] == '-') ||
                             (board[cursory - 1, cursorx - 1] == '|' && board[cursory - 1, cursorx + 1] == '|' && board[cursory - 2, cursorx] == '-')))
                        {
                            board[cursory, cursorx] = '-';
                        }
                        break;
                    case 1:
                        if (((board[cursory - 1, cursorx + 1] == '-' && board[cursory + 1, cursorx + 1] == '-' && board[cursory, cursorx + 2] == '|') ||
                             (board[cursory - 1, cursorx - 1] == '-' && board[cursory + 1, cursorx - 1] == '-' && board[cursory, cursorx - 2] == '|')))
                        {
                            board[cursory, cursorx] = '|';
                        }
                        break;
                }
                PrintBoard(board);
                printScore(computerScore, humanScore);
                printt(board);
            }

            if (stagenum == 2)
            {
                PrintState(round, stagenum, "Human");

            }

            while (stagenum == 2 && extraline == 0 && flagger)
            {

                Console.Clear();
                PrintState(round, stagenum, "Human");
                switch (cursory % 2)
                {
                    case 0:
                        {
                            board[cursory, cursorx] = '-';
                            extraline++;
                        }
                        break;
                    case 1:
                        {
                            board[cursory, cursorx] = '|';
                            extraline++;
                        }
                        break;
                }

                ownerlesscheck(board);
                PrintBoard(board);
                PrintState(round, stagenum, "Human");
                printScore(computerScore, humanScore);
                printt(board);
                if (extraline == 1)
                {
                    stagenum++;
                    break;
                }

            }
            // Stage 3 random piece 
            bool piece = false;

            while (stagenum == 3 && piece == false)
            {
                PrintState(round, stagenum, "Human");
                char[,] randompiece3 = GenerateRandomPiece3(); // Creating random piece
                GenerateShiftPiece(randompiece3); //Shifting Random piece
                PrintShape(randompiece3, stagenum); //Printing random piece
                board = PlacePieceOnBoard(board, randompiece3); //Placing random piece on temp board
                board = ownerlesscheck2(board);
                printLine(3);
                PrintBoard(board);
                printt(board);
             //   Thread.Sleep(1500);
                char[,] randompiece2 = GenerateRandomPiece2();
                GenerateShiftPiece(randompiece2);
                PrintShape(randompiece2, stagenum);
                board = PlacePieceOnBoard(board, randompiece2);
                board = ownerlesscheck2(board);
                printLine(2);
                PrintBoard(board);
                printt(board);
             //   Thread.Sleep(1500);
                char[,] randompiece1 = GenerateRandomPiece1();
                GenerateShiftPiece(randompiece1);
                printt(board);
                PrintShape(randompiece1, stagenum);
                board = PlacePieceOnBoard(board, randompiece1);
                board = ownerlesscheck2(board);
                printLine(1);
                PrintBoard(board);
                printt(board);
                printScore(computerScore, humanScore);
                piece = true;
            
            }

            if (gameOver(board)==false) {
            break;
            }

            if (stagenum == 4)
            {

               /* string filePath = "randomboard.txt"; // txt dosyanın doğru yolunu kontrol et

                // Dosyadaki satırları oku
                string[] lines2 = File.ReadAllLines(filePath);

                // Satır ve sütun sayısını belirle
                int rowCount = lines2.Length;
                int colCount = lines2[0].Length; // Tüm satırların eşit uzunlukta olduğunu varsayıyoruz

                // 2D char array tanımla
                char[,] board2 = new char[rowCount, colCount];

                // Her satırı ve karakteri diziye aktar
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        board2[i, j] = lines2[i][j];
                    }
                }
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        board[i, j] = board2[i,j];
                    }
                }

                PrintBoard(board);
                Thread.Sleep(1000);*/
                board = ownerlesscheck(board);
                PrintState(round, 1, "Computer");
                for (int i = 0; i < colors.GetLength(0); i++)
                {
                    for (int j = 0; j < colors.GetLength(1); j++)
                    {
                        colors[i, j] = 0;
                    }
                }
                int f = 0;
                int bestX = 0, bestY = 0;
                int[] bestDirectionRow = new int[100];
                int[] bestDirectionCol = new int[100];
                int[] rowSquareSet = new int[100];
                int[] colSquareSet = new int[100];
                int[] bestDirection = new int[100];

                PrintBoard(board);
                Thread.Sleep(1000);
                // Kare sayma ve puanlama
                for (int i = 0; i < 17; i++)
                {
                    for (int j = 0; j < 31; j++)
                    {
                        if (board[i, j] == '+')
                        {
                            int counter = 0;
                            if (j + 1 < 33 && board[i, j + 1] == '-') // Top side
                            {
                                counter++;
                            }

                            if (i + 2 < 19 && j + 1 < 33 && board[i + 2, j + 1] == '-') // Bottom side
                            {
                                counter++;
                            }

                            if (i + 1 < 19 && board[i + 1, j] == '|') // Left side
                            {
                                counter++;
                            }

                            if (j + 2 < 33 && i + 1 < 19 && board[i + 1, j + 2] == '|') // Right side
                            {
                                counter++;
                            }

                            if (counter == 4)
                            {
                                comp[i + 1, j + 1] = 4;
                            }

                            if (counter == 3)
                            {
                                comp[i + 1, j + 1] = 3;
                                rowSquareSet[f] = i + 1;
                                colSquareSet[f] = j + 1;
                                f++;
                            }

                            if (counter == 2)
                            {
                                comp[i + 1, j + 1] = 2;
                            }

                            if (counter == 1)
                            {
                                comp[i + 1, j + 1] = 1;
                            }
                            if (counter == 0)
                            {
                                comp[i + 1, j + 1] = 0;
                            }
                        }
                    }
                }

                int highestScore = 0;
                int l = 0;
                // En iyi kare oluşturma
                for (int i = 0; i < f; i++)  
                {
                    for (int j = 0; j < tryPoint; j++)
                    {
                        int[,] temp = (int[,])comp.Clone();
                        char[,] tempboard = (char[,])board.Clone();
                        int x = rowSquareSet[i];
                        int y = colSquareSet[i];
                        int z = 0;
                        int[] directionrow = new int[50];
                        int[] directioncol = new int[50];
                        int[] Direction = new int[50];


                        int currentScore = 1;
                        while (x >= 0 && x < 19 && y >= 0 && y < 33 && temp[x, y] == 3)
                        {

                            tempboard = kareYapma(tempboard, x, y);
                            temp = compboard(tempboard);

                            int[ ] directy = new int[4];
                            int directcount = 0;
                            if ( y + 2 < 33 && temp[x, y + 2] == 3)
                            {
                                directy[directcount] = 1;
                                directcount++;
                            }
                            if ( x - 2 >= 0 && temp[x - 2, y] == 3)
                            {
                                directy[directcount] = 2;
                                directcount++;
                            }
                             if ( y - 2 >= 0 && temp[x, y - 2] == 3)
                            {
                                directy[directcount] = 3;
                                directcount++;
                            }
                            if ( x + 2 < 19 && temp[x + 2, y] == 3)
                            {

                                directy[directcount] = 4;
                                directcount++;
                            }


                            int directyön = random.Next(directcount);
                            int direct = directy[directyön];

                            directionrow[z] = x;
                            directioncol[z] = y;
                            Direction[z] = direct;
                            if (direct == 1 && y + 2 < 33 && temp[x, y + 2] == 3)
                            {
                                z++;
                                y = y + 2;
                                currentScore++;
                            }
                            else if (direct == 2 && x - 2 >= 0 && temp[x - 2, y] == 3)
                            {
                                z++;
                                x = x - 2; 
                                currentScore++;
                            }
                            else if (direct == 3 && y - 2 >= 0 && temp[x, y - 2] == 3)
                            {
                                z++;
                                y = y - 2; 
                                currentScore++;
                            }
                            else if (direct == 4 && x + 2 < 19 && temp[x + 2, y] == 3)
                            {
                                z++;
                                x = x + 2; 
                                currentScore++;
                            }
                        }


                        if (currentScore > highestScore)
                        {
                            highestScore = currentScore;
                            bestX = x;
                            bestY = y;
                            l = z + 1;
                            for (int p = 0; p < l+1; p++) 
                            {
                                bestDirectionRow[p] = directionrow[p];
                                bestDirectionCol[p] = directioncol[p];
                                bestDirection[p] = Direction[p];
                            }
                        }
                    }
                }

                if (gameOver(board) == false)
                {
                    break;
                }

                for (int i = 0; i < l+1; i++) 
                {
                    if (bestDirectionRow[i] > 0 && bestDirectionRow[i] < 19 &&
                        bestDirectionCol[i] > 0 && bestDirectionCol[i] < 33)
                    {
                        int u = bestDirectionRow[i];
                        int ı = bestDirectionCol[i];
                        
                        board = kareYapma(board, u, ı);
                        ownerlesscheck(board);
                        comp = compboard(board);
                        ComputerPrint(bestDirectionCol[0], bestDirectionRow[0], i+1, bestDirection, i);
                        Thread.Sleep(500);
                        PrintBoard(board);

                        computerScore++;
                    }
                }

                ownerlesscheck(board);
                PrintBoard(board);
                if (gameOver(board) == false)
                {
                    break;
                }
                ComputerPrint(bestDirectionCol[0], bestDirectionRow[0], highestScore, bestDirection, l);

                printt(board);
                printScore(computerScore, humanScore);

            }
            if (gameOver(board) == false)
            {
                break;
            }

            if (stagenum == 5)
            {
                board = ownerlesscheck(board);
                PrintState(round, 2, "Computer");
                int[] compStage2row = new int[1000];
                int[] compStage2col = new int[1000];
                int z = 0;
                char[,] temp = (char[,])board.Clone();
                for (int i = 0; i < comp.GetLength(0); i++)
                {
                    for (int j = 0; j < comp.GetLength(1); j++)
                    {
                        if (j % 2 == 1 && i % 2 == 1)
                        {
                            if (comp[i, j] == 1 || comp[i, j] == 0)
                            {
                                z++;
                                compStage2row[z] = i;
                                compStage2col[z] = j;
                            }
                        }
                    }
                }
                int z2 = 0;
                for (int i = 0; i < comp.GetLength(0); i++)
                {
                    for (int j = 0; j < comp.GetLength(1); j++)
                    {
                        if (j % 2 == 1 && i % 2 == 1)
                        {
                            bool top = (i - 2 >= 0) && (comp[i - 2, j] == 3 || comp[i - 2, j] == 2 || comp[i - 2, j] == 4);
                            bool bottom = (i + 2 < comp.GetLength(0)) && (comp[i + 2, j] == 3 || comp[i + 2, j] == 2 || comp[i + 2, j] == 4);
                            bool left = (j - 2 >= 0) && (comp[i, j - 2] == 3 || comp[i, j - 2] == 2 || comp[i, j - 2] == 4);
                            bool right = (j + 2 < comp.GetLength(1)) && (comp[i, j + 2] == 3 || comp[i, j + 2] == 2 || comp[i, j + 2] == 4);
                            if (comp[i, j] == 0 || comp[i, j] == 1)
                            {

                                if (top && bottom && left && right && i != 1 && i != 17 && j != 1 && j != 31)
                                {
                                    z2++;
                                }
                                if (i != 1 && i != 17 && j == 1 && top && bottom && right)
                                {
                                    z2++;
                                }
                                if (i != 1 && i != 17 && j == 31 && top && bottom && left)
                                {
                                    z2++;
                                }

                                if (j != 1 && j != 31 && i == 1 && right && bottom && left)
                                {
                                    z2++;
                                }

                                if (j != 1 && j != 31 && i == 17 && (comp[i, j] == 1 || comp[i, j] == 0) && right && top && left)
                                {
                                    z2++;
                                }

                                if (i == 1 && j == 1 && right && bottom)
                                {
                                    z2++;
                                }
                                if (i == 1 && j == 31 && bottom && left)
                                {
                                    z2++;
                                }
                                if (i == 17 && j == 1 && top && right)
                                {
                                    z2++;
                                }
                                if (i == 17 && j == 31 && top && left)
                                {
                                    z2++;
                                }
                            }
                        }
                    }
                }
                bool flag2 = false;
                if (z2 >= z)
                {
                    flag2 = true; // artık insana hamle vermekten başka şans olmadığında bura çalışıyor ve flag 2 yi true yapıyoruz

                    int[] compStage2row2 = new int[1000];
                    int[] compStage2col2 = new int[1000];
                    z = 0;

                    for (int i = 0; i < comp.GetLength(0); i++)
                    {
                        for (int j = 0; j < comp.GetLength(1); j++)
                        {
                            if (j % 2 == 1 && i % 2 == 1)
                            {
                                if (comp[i, j] == 3)
                                {
                                    z++;
                                    compStage2row2[z] = i;
                                    compStage2col2[z] = j;
                                }
                            }
                        }
                    }


                    int randomz2 = random.Next(z) + 1;
                    int x2 = compStage2row2[randomz2];
                    int y2 = compStage2col2[randomz2];
                    temp = çizgikoyma(temp, x2, y2);
                    if (AreBoardsEqual(board, temp))
                    {
                        int[] compStage2row3 = new int[1000];
                        int[] compStage2col3 = new int[1000];
                        z = 0;
                        for (int i = 0; i < comp.GetLength(0); i++)
                        {
                            for (int j = 0; j < comp.GetLength(1); j++)
                            {
                                if (j % 2 == 1 && i % 2 == 1)
                                {
                                    if (comp[i, j] == 2)
                                    {
                                        z++;
                                        compStage2row3[z] = i;
                                        compStage2col3[z] = j;
                                    }
                                }
                            }
                        }
                        randomz2 = random.Next(z) + 1;//0 dahil olmasın z dahil olsun
                        x2 = compStage2row3[randomz2];
                        y2 = compStage2col3[randomz2];

                        temp = çizgikoyma(temp, x2, y2);

                        if (!AreBoardsEqual(board, temp))
                        {
                            for (int i = 0; i < 18; i++)
                            {
                                for (int j = 0; j < 32; j++)
                                {
                                    if (board[i, j + 1] == ' ' && board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' && board[i, j + 2] == '|')
                                    {
                                        board[i, j + 1] = ':';
                                    }
                                }
                            }
                            board = temp;
                            PrintBoard(board);
                            printt(board);
                            printScore(computerScore, humanScore);
                        }
                    }
   
                    else
                    {
                        board = temp;
                        for (int i = 0; i < 18; i++)
                        {
                            for (int j = 0; j < 32; j++)
                            {
                                if (board[i, j + 1] == ' ' && board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' && board[i, j + 2] == '|')
                                {
                                    board[i, j + 1] = ':';
                                }
                            }
                        }
                        PrintBoard(board);
                        printt(board);
                    }
                    printt(board);
                }

                if (flag2 == false)// insana hamle vermiyoruz burayı çalıştırıyoruz.
                {
                    int randomz = random.Next(z) + 1;
                    int x = compStage2row[randomz];
                    int y = compStage2col[randomz];
                    int m = 0;
                    int[] compstagelinerow = new int[1000];
                    int[] compStagelinecol = new int[1000];
                    char[] compstageline = new char[1000];
                    bool koy = true;



                    while (koy && z != 0)
                    {

                        if (temp[x, y] == ' ')
                        {

                            if (x > 0 && temp[x - 1, y] == ' ' && !(comp[x - 2, y] == 3 || comp[x - 2, y] == 2))
                            {
                                m++;
                                compstagelinerow[m] = x - 1;
                                compStagelinecol[m] = y;
                                compstageline[m] = '-';
                                koy = false;
                            }
                            if (x < board.GetLength(0) - 1 && temp[x + 1, y] == ' ' && !(comp[x + 2, y] == 3 || comp[x + 2, y] == 2))
                            {
                                m++;
                                compstagelinerow[m] = x + 1;
                                compStagelinecol[m] = y;
                                compstageline[m] = '-';
                                koy = false;
                            }
                            if (y > 0 && temp[x, y - 1] == ' ' && !(comp[x, y - 2] == 3 || comp[x, y - 2] == 2))
                            {
                                m++;
                                compstagelinerow[m] = x;
                                compStagelinecol[m] = y - 1;
                                compstageline[m] = '|';
                                koy = false;
                            }
                            if (y < board.GetLength(1) - 1 && temp[x, y + 1] == ' ' && !(comp[x, y + 2] == 3 || comp[x, y + 2] == 2))
                            {
                                m++;
                                compstagelinerow[m] = x;
                                compStagelinecol[m] = y + 1;
                                compstageline[m] = '|';
                                koy = false;
                            }
                            randomz = random.Next(z) + 1;
                            x = compStage2row[randomz];
                            y = compStage2col[randomz];

                        }
                    }
                    if (m != 0)
                    {
                        int randomM = random.Next(m) + 1;

                        temp[compstagelinerow[randomM], compStagelinecol[randomM]] = compstageline[randomM];
                        colors[compstagelinerow[randomM], compStagelinecol[randomM]] = 1;
                    }
                    board = temp;
                    PrintBoard(board);
                    printt(board);
                }
                printScore(computerScore, humanScore);
            }
            piece = false;
                if (stagenum == 6)
                {
                    PrintState(round, 3, "Computer");
                    for (int i = 0; i < colors.GetLength(0); i++)
                    {
                        for (int j = 0; j < colors.GetLength(1); j++)
                        {
                            colors[i, j] = 0;
                        }
                    }
                    while (piece == false)
                    {

                        char[,] randompiece3 = GenerateRandomPiece3(); // Creating random piece
                        GenerateShiftPiece(randompiece3); //Shifting Random piece
                        PrintShape(randompiece3, stagenum); //Printing random piece
                        board = PlacePieceOnBoard(board, randompiece3); //Placing random piece on temp board
                        board = ownerlesscheck2(board);
                        printLine(3);
                        PrintBoard(board);
                        printt(board);
                      //  Thread.Sleep(1500);
                        char[,] randompiece2 = GenerateRandomPiece2();
                        GenerateShiftPiece(randompiece2);
                        PrintShape(randompiece2, stagenum);
                        board = PlacePieceOnBoard(board, randompiece2);
                        board = ownerlesscheck2(board);
                        printLine(2);
                        PrintBoard(board);
                        printt(board);
                       // Thread.Sleep(1500);
                        char[,] randompiece1 = GenerateRandomPiece1();
                        GenerateShiftPiece(randompiece1);
                        printt(board);
                        PrintShape(randompiece1, stagenum);
                        board = PlacePieceOnBoard(board, randompiece1);
                        board = ownerlesscheck2(board);
                        printLine(1);
                        PrintBoard(board);
                        printt(board);
                        piece = true;

                    }
                    printScore(computerScore, humanScore);
                }

                int squaresP = 0;
                if (flag && stagenum == 1)
                {
                    for (int i = 0; i < 19; i++)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                            bool score = false;
                            if (board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' &&
                              board[i, j + 2] == '|' && board[i, j + 1] == ' ' )
                            {
                                
                                squaresP++;
                            }
                           
                        }
                    }
                for (int i = 0; i < 19; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        bool score = false;
                        if (board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' &&
                          board[i, j + 2] == '|' && board[i, j + 1] == ' ' && squaresP == 1)
                        {
                            board[i, j + 1] = 'P';
                            previousX = j + 1;
                            previousY = i;
                            squaresP++;
                            score = true;
                            flag = false;
                            humanScore++;
                        }
                        

                        if (board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' &&
                          board[i, j + 2] == '|' && board[i, j + 1] == ' ' && squaresP != 1)
                        {
                            board[i, j + 1] = '?';
                            flag4 = true;
                            flag = true;
                        }
                       

                    }
                }
                if (flag5 = true)
                {
                    if (board[cursory, cursorx] == '?' )
                    {
                        board[cursory, cursorx] = 'P';
                        previousX = cursorx;
                        previousY = cursory;
                        flag = false;
                        flag4 = false;
                        flag6 = true;
                        ownerlesscheck(board);
                        humanScore++;
                       

                    }
                }

              
                    PrintBoard(board);
                    printt(board);
                    printScore(computerScore, humanScore);
                }
                int squaresPp = 0;
                if (!flag && stagenum == 1)
                {
                for (int i = 0; i < 18; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (board[i, j + 1] == '?' && board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' && board[i, j + 2] == '|')
                        {
                            board[i, j + 1] = ':';
                        }
                    }
                }
                PrintBoard(board);
                Console.Clear();
                    PrintState(round, stagenum, "Human");
                    for (int i = 0; i < 19; i++)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                     
                            if (board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' &&
                                board[i, j + 2] == '|' && board[i, j + 1] == ' ')
                            {
                           
                                if (i == previousY && j + 3 == previousX || i == previousY && j - 1 == previousX ||
                                    i - 2 == previousY && j + 1 == previousX || i + 2 == previousY && j + 1 == previousX)
                                {
                                    if (squaresPp == 0)
                                    {
                                        board[i, j + 1] = 'P';
                                        previousX = j + 1;
                                        previousY = i;
                                        humanScore++;
                                        squaresPp++;
                                    }
                                }
                            }
                        }
                    }
                    bool flag3 = false;
                    for (int i = 0; i < 19; i++)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                            if (board[i, j] == '|' && board[i - 1, j + 1] == '-' && board[i + 1, j + 1] == '-' &&
                                board[i, j + 2] == '|' && board[i, j + 1] == ' ')
                            {
                                if (squaresPp == 0)
                                {
                                    board[i, j + 1] = ':';
                                    humanScore = humanScore - 5;
                                    flag3 = true;
                                    PrintState(round, stagenum, "Human");
                                }
                                else
                                {
                                    board[i, j + 1] = ':';
                                }
                            }
                        }
                    }
                    if (flag3 == true)
                    {
                        stagenum++;
                        PrintState(round, stagenum, "Human");
                    }
                    PrintBoard(board);
                    printt(board);
                    printScore(computerScore, humanScore);
                }

                if (stagenum == 6)
                {
                    previousY = 0;
                    previousX = 0;
                    flag = true;
                    extraline = 0;
                    printt(board);
                    round++;
                    PrintBoard(board);
                }
                gameO = gameOver(board);

            }
            Console.Clear();
        Console.WriteLine("---GAME OVER----");
        Console.WriteLine("Human Score : " + humanScore);
        Console.WriteLine("Computer Score : " + computerScore);
        if (computerScore > humanScore)
        {
            Console.WriteLine();
            Console.WriteLine("Computer wins");
        }
        else if (computerScore < humanScore)
        {
            Console.WriteLine();
            Console.WriteLine("Human wins");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Draw");
        }
        Thread.Sleep(3000);

         Console.WriteLine() ;
        int newLength = scores.Length + 1;
        int[] updatedScores = new int[newLength];
        string[] updatedNames = new string[newLength];
        bool inserted = false;

        for (int i = 0, j = 0; i < newLength; i++)
        {
            if (!inserted && (j == scores.Length || humanScore > scores[j]))
            {
                updatedScores[i] = humanScore;
                updatedNames[i] = playerName;
                inserted = true;
            }
            else
            {
                updatedScores[i] = scores[j];
                updatedNames[i] = names[j];
                j++;
            }
        }

        for (int i = 0; i < updatedScores.Length - 1; i++)
        {
            for (int j = 0; j < updatedScores.Length - 1 - i; j++)
            {
                if (updatedScores[j] < updatedScores[j + 1])
                {
                    int tempScore = updatedScores[j];
                    updatedScores[j] = updatedScores[j + 1];
                    updatedScores[j + 1] = tempScore;

                    string tempName = updatedNames[j];
                    updatedNames[j] = updatedNames[j + 1];
                    updatedNames[j + 1] = tempName;
                }
            }
        }

        using (StreamWriter writer = new StreamWriter("highscore.txt"))
        {
            for (int i = 0; i < updatedScores.Length; i++)
            {
                writer.WriteLine(updatedNames[i] + " " + updatedScores[i]);
            }
        }

        Console.WriteLine("Updated High Scores:");
        for (int i = 0; i < updatedScores.Length; i++)
        {
            Console.WriteLine(updatedNames[i] + " " + updatedScores[i]);
        }

    }
}