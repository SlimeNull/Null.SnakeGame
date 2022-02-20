using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NullLib.SnakeGame
{
    public class SnakeGameCore
    {
        public SnakeMap Map { get; }

        public bool Started { get; private set; }
        public bool Paused { get; private set; }
        public bool Stoped { get; private set; }
        public bool Running { get; private set; }

        public int TimespanPerStep { get; set; } = 150;       // 每一次刷新的间隔

        public ISnakeMapRenderer? SnakeMapRenderer { get; set; }  // 用来绘制贪吃蛇地图的绘制器(Drawer)

        public SnakeGameCore(SnakeMap map)
        {
            Map = map ?? throw new ArgumentNullException(nameof(map));
        }

        Task? gameLoopTask;
        public Task? GameLoopTask => gameLoopTask;

        bool requireStopGame = false;
        void GameLoopAction()
        {
            if (Map == null)
                return;

            Started = true;
            Stoped = false;

            Running = true;
            while (Running && !requireStopGame)
            {
                if (!Paused)
                {
                    Running = Map.NextStep();
                    SnakeMapRenderer?.DrawMap(Map);
                }

                Thread.Sleep(TimespanPerStep);
            }

            Started = false;
            Stoped = true;
        }

        public void Start()
        {
            requireStopGame = false;
            gameLoopTask = Task.Run(GameLoopAction);
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Resume()
        {
            Paused = false;
        }

        public void Stop()
        {
            requireStopGame = true;
        }

        public void ResetState()
        {
            if (Running)
                throw new Exception("Game is still running now!");
            Started = Paused = Stoped = false;
        }
    }
}
