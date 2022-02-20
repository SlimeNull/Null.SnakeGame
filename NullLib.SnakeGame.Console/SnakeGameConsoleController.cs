using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NullLib.SnakeGame.Console
{
    /// <summary>
    /// 贪吃蛇游戏控制台控制器
    /// </summary>
    public class SnakeGameConsoleController : ISnakeGameController
    {
        public SnakeGameConsoleController()
        {
            snakeKeys = new Dictionary<Snake, (ConsoleKey left, ConsoleKey up, ConsoleKey right, ConsoleKey down)>();
        }

        ConsoleKey startKey, pausekey, resumeKey, stopkey;
        Dictionary<Snake, (ConsoleKey left, ConsoleKey up, ConsoleKey right, ConsoleKey down)> snakeKeys;

        void GameControllerLoopAction(SnakeGameCore game)
        {
            if (game == null)
                return;

            while (!game.Stoped)
            {
                ConsoleKey key = System.Console.ReadKey().Key;

                if (key == startKey && !game.Started)
                    game.Start();

                if (key == pausekey && !game.Paused)
                    game.Pause();
                else if (key == resumeKey && game.Paused)
                    game.Resume();

                if (key == stopkey && !game.Stoped)
                    game.Stop();

                foreach (var snakeKV in snakeKeys)
                {
                    if (key == snakeKV.Value.left)
                        snakeKV.Key.MoveDirection = SnakeMoveDirection.Left;
                    else if (key == snakeKV.Value.up)
                        snakeKV.Key.MoveDirection = SnakeMoveDirection.Up;
                    else if (key == snakeKV.Value.right)
                        snakeKV.Key.MoveDirection = SnakeMoveDirection.Right;
                    else if (key == snakeKV.Value.down)
                        snakeKV.Key.MoveDirection = SnakeMoveDirection.Down;
                }
            }
        }

        public void HandleController(SnakeGameCore game)
        {
            GameControllerLoopAction(game);
        }

        public Task HandleControllerAsync(SnakeGameCore game)
        {
            return Task.Run(() => GameControllerLoopAction(game));
        }


        public SnakeGameConsoleController SetPauseKey(ConsoleKey key)
        {
            pausekey = key;
            return this;
        }

        public SnakeGameConsoleController SetResumeKey(ConsoleKey key)
        {
            resumeKey = key;
            return this;
        }

        public SnakeGameConsoleController SetSnakeKeys(Snake snake, ConsoleKey left, ConsoleKey up, ConsoleKey right, ConsoleKey down)
        {
            snakeKeys[snake] = (left, up, right, down);
            return this;
        }

        public SnakeGameConsoleController SetStartKey(ConsoleKey key)
        {
            startKey = key;
            return this;
        }

        public SnakeGameConsoleController SetStopKey(ConsoleKey key)
        {
            stopkey = key;
            return this;
        }
    }
}
