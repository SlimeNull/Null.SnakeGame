using System;
using System.Threading;
using System.Threading.Tasks;
using ShiqvliziLib.SnakeGame;
using ShiqvliziLib.SnakeGame.Console;

namespace Shiqvlizi.SnakeGame
{
    internal partial class Program
    {
        static void Main()
        {
            SnakeGameCore game = new SnakeGameCore(new SnakeMap(30, 30)
            {
                new Snake(SnakeMoveDirection.Right, 5, 5, 3),
                new Snake(SnakeMoveDirection.Left, 24, 24, 3)
            });
            game.SnakeMapRenderer = new SnakeMapConsoleRenderer();

            SnakeGameConsoleController gameController = new SnakeGameConsoleController();
            gameController
                .SetStartKey(ConsoleKey.Spacebar)
                .SetPauseKey(ConsoleKey.P)
                .SetResumeKey(ConsoleKey.P)
                .SetStopKey(ConsoleKey.Escape)
                .SetSnakeKeys(game.Map[0],
                    ConsoleKey.A, ConsoleKey.W, ConsoleKey.D, ConsoleKey.S)
                .SetSnakeKeys(game.Map[1],
                    ConsoleKey.LeftArrow, ConsoleKey.UpArrow, ConsoleKey.RightArrow, ConsoleKey.DownArrow);

            Console.WriteLine("按 空格 开始游戏 (30x30, x2)");

            gameController.HandleController(game);

            Console.WriteLine("游戏结束");
            Console.ReadKey();

            return;
            int[,] mapp = new int[20, 20];
            string[,] map = new string[20, 40];
            for (int n = 0; n < 40; n++)
            {
                map[0, n] = "=";
            }
            for (int j = 0; j < 40; j++)
            {
                for (int i = 1; i < 20; i++)
                {
                    map[i, j] = "█";
                }
            }
            map[10, 20] = " ";
            while (true)
            {
                for (int k = 0; k < 20; k++)
                {
                    Console.WriteLine();
                    for (int m = 0; m < 40; m++)
                    {

                        Console.Write(map[k, m]);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        static void MyMain()
        {

        }
    }
}
