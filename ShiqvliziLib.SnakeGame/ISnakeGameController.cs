using System.Threading.Tasks;

namespace ShiqvliziLib.SnakeGame
{
    /// <summary>
    /// 贪吃蛇游戏控制器
    /// </summary>
    public interface ISnakeGameController
    {
        void HandleController(SnakeGameCore game);
        Task HandleControllerAsync(SnakeGameCore game);
    }
}
