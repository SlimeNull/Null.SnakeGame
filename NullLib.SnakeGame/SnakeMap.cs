using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NullLib.SnakeGame
{
    public partial class SnakeMap
    {
        int width, height;
        (int x, int y) food;

        private List<Snake> snakes;

        public int Width => width;
        public int Height => height;
        public (int X, int Y) Food => food;

        public SnakeMap(int width, int height)
        {
            snakes = new List<Snake>();                   // 新建一个列表用来保存蛇
            (this.width, this.height) = (width, height);  // 保存当前地图的宽度和高度
            RefreshFood(1);                               // 此时地图里面没有蛇, 一定能够成功生成食物
        }

        /// <summary>
        /// 更新食物坐标
        /// </summary>
        /// <param name="randomTimes">最多使用多少次随机数生成坐标 (超过这个次数, 则使用地图摒除的方法, 判断是否可以生成或直接生成随即食物坐标)</param>
        /// <returns></returns>
        public bool RefreshFood(int randomTimes)
        {
            bool regen = false;             // 重新生成食物
            Random random = new Random();   // 随机数生成器
            (int x, int y) newFood;         // 新食物的坐标
            int retryTimes = 0;             // 随机数生成食物的尝试次数

            do
            {
                regen = false;
                if (retryTimes < randomTimes)
                {
                    int
                        x = random.Next(0, width),
                        y = random.Next(0, height);   // 生成一对随机坐标
                    newFood = (x, y);                 // 转换为元组

                    foreach (Snake snake in snakes)   // 检查食物坐标是否与蛇坐标冲突
                    {
                        if (snake.Head == newFood)    // 如果食物坐标与蛇头坐标相同
                            regen = true;
                        foreach ((int, int) bodySeg in snake.Body)
                            if (bodySeg == newFood)                  // 如果蛇身体某个部分和食物坐标相同
                                regen = true;
                    }

                    retryTimes++;   // '尝试生成次数' 增加1
                }
                else
                {
                    IEnumerable<(int, int)> allMapBlocks = Enumerable.Range(0, width)      // 地图上所有的坐标
                        .Select(x => Enumerable.Range(0, height).Select(y => (x, y)))
                        .Aggregate((v1, v2) => v1.Concat(v2));

                    IEnumerable<(int, int)> allSnakePart;                                  // 所有蛇的部分 头和身体
                    IEnumerable<Snake> undiedSnakes = snakes.Where(s => !s.IsDied);        // 筛选没有死的蛇
                    if (undiedSnakes.Any())                                                // 如果有没有死的蛇
                        allSnakePart = undiedSnakes
                            .Select(s => s.Body.Append(s.Head))
                            .Aggregate((b1, b2) => b1.Concat(b2));                         // 将所有蛇的部分设定为没有死的蛇的身体和头部坐标
                    else
                        allSnakePart = Enumerable.Empty<(int, int)>();                     // 空集合, 没有蛇

                    List<(int, int)> allEmptyBlocks = allMapBlocks                         // 所有可用的能够生成食物的坐标
                        .Where(b => !allSnakePart.Contains(b))                             // 从所有坐标中筛选不是蛇的部分的坐标
                        .ToList();                                                         // 转换为列表
                    if (allEmptyBlocks.Count == 0)                                         // 如果列表为空, 也就是没办法生成食物
                        return false;                                                      // 返回 false, 即: 食物生成失败
                    newFood = allEmptyBlocks[random.Next(0, allEmptyBlocks.Count)];        // 返回一个随机的食物坐标
                }
            }
            while (regen);                                                                 // 当 regen 时, 始终尝试重新生成

            food = newFood;    // 将食物坐标设定为新坐标
            return true;
        }

        /// <summary>
        /// 刷新地图以及地图中所有蛇的状态
        /// </summary>
        /// <returns>是否刷新完毕, 如果返回 false, 那就意味着所有蛇都已经死亡, 继续刷新不会有任何改动</returns>
        public bool NextStep()
        {
            bool refreshedAny = false;                      // 是否刷新了任意一条蛇
            bool requireRefreshFood = false;                // 是否需要刷新食物

            IEnumerable<Snake> undiedSnakes = snakes
                .Where(s => !s.IsDied);                     // 筛选没有死的蛇

            if (!undiedSnakes.Any())
                return false;

            IEnumerable<(int x, int y)> allBodys = undiedSnakes
                .Select(v => v.Body)                        // 选择蛇的身体
                .Aggregate((v1, v2) => v1.Concat(v2));      // 把蛇的身体连接在一起

            foreach (Snake snake in snakes)
                snake.NextStep();                           // 使所有蛇更新坐标 (但是如果蛇已经死了, 这个逻辑不会有任何的作用, 在蛇的 NextStep 中判定了 isDied)

            foreach (Snake snake in snakes)
            {
                if (snake.IsDied)                 // 如果蛇已经嗝屁了
                    continue;                     // 跳过对这条蛇的处理, 处理下一条蛇

                refreshedAny |= true;             // 设定已经刷新了

                (int X, int Y) snakehead = snake.Head;
                if (snakehead == food)           // 如果蛇吃到了食物
                {
                    snake.NewBodySegment = true;  // 蛇的长度在下一次更新时加一
                    requireRefreshFood = true;
                }

                if (snakehead.X < 0 || snakehead.X >= width || snakehead.Y < 0 || snakehead.Y >= height)   // 如果蛇撞到边界
                {
                    snake.MakeDie();
                    continue;
                }

                foreach (Snake _snake in snakes)
                    if (snakehead == _snake.Head && snake != _snake)    // 如果俩蛇撞头了, 自己不能撞自己草 (((
                    {
                        snake.MakeDie();
                        _snake.MakeDie();             // 同归于尽
                    }

                foreach ((int x, int y) bodySeg in allBodys)
                    if (snakehead == bodySeg)        // 如果蛇撞上了谁的身体
                        snake.MakeDie();              // 让他嗝屁
            }

            if (requireRefreshFood)
                if (!RefreshFood(5))
                    return false;

            return refreshedAny;   // 如果这里返回了 false, 那就意味着所有的蛇都已经嗝屁了
        }
    }

    public partial class SnakeMap : IList<Snake>
    {
        public Snake this[int index] { get => snakes[index]; set => snakes[index] = value; }

        public int Count => snakes.Count;

        public bool IsReadOnly => false;

        public void Add(Snake item) => snakes.Add(item);

        public void Clear() => snakes.Clear();

        public bool Contains(Snake item) => snakes.Contains(item);

        public void CopyTo(Snake[] array, int arrayIndex) => snakes.CopyTo(array, arrayIndex);

        public IEnumerator<Snake> GetEnumerator() => snakes.GetEnumerator();

        public int IndexOf(Snake item) => snakes.IndexOf(item);

        public void Insert(int index, Snake item) => snakes.Insert(index, item);

        public bool Remove(Snake item) => snakes.Remove(item);

        public void RemoveAt(int index) => snakes.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => snakes.GetEnumerator();
    }
}
