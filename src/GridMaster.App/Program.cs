using System;
using GridMaster.App.Models;

namespace GridMaster.App
{
    class Program
    {
        static Player playerOne = new Player();
        static Player playerTwo = new Player();
        private static int gridSize = 10;

        static Box[,] grid = new Box[gridSize, gridSize];

        static void Main(string[] args)
        {
            bool gameOver = false;
            bool playerOneTurn = true;

            InitializeGrid();

            //ask player names
            Console.WriteLine("GridMaster!");
            Console.WriteLine("Created by Zachary and Daddy");

            Console.Write("What is player one's name? ");
            playerOne.Name = Console.ReadLine();

            Console.Write("What is player two's name? ");
            playerTwo.Name = Console.ReadLine();

            DrawGrid();

            // game loop
            while (!gameOver)
            {
                Console.WriteLine(playerOneTurn
                    ? $"{playerOne.Name}, it's your turn."
                    : $"{playerTwo.Name}, it's your turn.");

                Console.Write("Enter move (X, Y, Side) or Q to quit: ");
                var move = Console.ReadLine();

                if (move.Equals("Q", StringComparison.InvariantCultureIgnoreCase))
                {
                    gameOver = true;
                    continue;
                }

                var split = move.Split(',');
                var moveX = int.Parse(split[0]);
                var moveY = int.Parse(split[1]);
                var moveSide = split[2];

                if (moveX < 0 || moveX > gridSize)
                {
                    Console.WriteLine("Invalid X coordinate.");
                    continue;
                }

                if (moveY < 0 || moveY > gridSize)
                {
                    Console.WriteLine("Invalid Y coordinate.");
                    continue;
                }

                if (!IsValidSide(moveSide))
                {
                    Console.WriteLine("Invalid Side.  Please enter T, B, L, R");
                    continue;
                }

                // check grid
                if (grid[moveX, moveY].IsSideMarked(moveSide))
                {
                    Console.WriteLine("This side is already marked.  Please enter a new move.");
                    continue;
                }

                //mark side
                MarkSide(moveX, moveY, moveSide);

                //check for box
                if (grid[moveX, moveY].Enclosed())
                {
                    //mark for player
                    grid[moveX, moveY].WonBy = playerOneTurn ? playerOne : playerTwo;
                }

                playerOneTurn = !playerOneTurn;

                DrawGrid();
            }
        }

        static bool IsValidSide(string side)
        {
            return (side.Equals("T", StringComparison.InvariantCultureIgnoreCase))
                   || (side.Equals("B", StringComparison.InvariantCultureIgnoreCase))
                   || (side.Equals("L", StringComparison.InvariantCultureIgnoreCase))
                   || (side.Equals("R", StringComparison.InvariantCultureIgnoreCase))
                ;
        }

        static void MarkSide(int x, int y, string moveSide)
        {
            var currentBox = grid[x, y];

            switch (moveSide.ToUpper())
            {
                case "T":
                    currentBox.Top.IsMarked = true;
                    if (y != 0 && y - 1 >= 0)
                    {
                        grid[x, y - 1].Bottom.IsMarked = true;
                    }
                    return;
                case "B":
                    currentBox.Bottom.IsMarked = true;
                    if (y != gridSize - 1 && y + 1 < gridSize)
                    {
                        grid[x, y + 1].Top.IsMarked = true;
                    }
                    return;
                case "L":
                    currentBox.Left.IsMarked = true;
                    if (x != 0 && x - 1 >= 0)
                    {
                        grid[x - 1, y].Right.IsMarked = true;
                    }
                    return;
                case "R":
                    currentBox.Right.IsMarked = true;
                    if (x != gridSize - 1 && x + 1 < gridSize)
                    {
                        grid[x + 1, y].Left.IsMarked = true;
                    }
                    return;
            }
        }
        static void InitializeGrid()
        {
            for (var y = 0; y <= gridSize - 1; y++)
            {
                for (var x = 0; x <= gridSize - 1; x++)
                {
                    grid[x, y] = new Box { X = x, Y = y };
                }
            }
        }

        static void DrawGrid()
        {
            Console.Clear();

            var currentRowY = 0;
            var rowIndex = 0;

            //loop through Grid
            for (var y = 0; y <= 2 * gridSize + 1; y++)
            {
                if (y == 0)
                {
                    Console.Write("   ");
                }
                else if (rowIndex == 1)
                {
                    Console.Write($" {currentRowY} ");
                    rowIndex++;
                }
                else
                {
                    Console.Write("   ");
                    rowIndex++;
                }

                for (var x = 0; x < gridSize; x++)
                {
                    if (y == 0)
                    {
                        // top line 
                        Console.Write(x == 0 ? $"  {x}  " : $" {x}  ");
                        continue;
                    }

                    var currentBox = grid[x, currentRowY];

                    if (y % 2 == 0)
                    {
                        //write left, won by, right
                        Console.Write(currentBox.Left.IsMarked ? "|" : " ");
                        Console.Write(currentBox.WonBy == null ? "   " : $" {currentBox.WonBy.Name.Substring(0, 1)} ");

                        if (x == gridSize - 1)
                        {
                            Console.Write(currentBox.Right.IsMarked ? "|" : "");
                        }
                    }
                    else
                    {
                        //write top or bottom
                        if (y == 2 * gridSize + 1)
                        {
                            Console.Write(currentBox.Bottom.IsMarked
                                ? x == 0 ? "-----" : "----"
                                : "    ");
                        }
                        else
                        {
                            Console.Write(currentBox.Top.IsMarked
                          ? x == 0 ? "-----" : "----"
                          : "    ");
                        }


                    }
                }

                if (rowIndex == 2)
                {
                    rowIndex = 0;

                    if (currentRowY != gridSize - 1)
                    {
                        currentRowY++;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
