using NullLib.SnakeGame;
using NullLib.SnakeGame.Console;
using System;
using System.Threading;
using System.Threading.Tasks;


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
        }
    }
}
