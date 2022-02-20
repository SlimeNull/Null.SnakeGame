using System.Threading.Tasks;

namespace NullLib.SnakeGame
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
