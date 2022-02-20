using System;
using System.Collections.Generic;
using System.Linq;

namespace NullLib.SnakeGame
{
    public class Snake
    {
        private (int x, int y) head;        // 头的坐标  坐标系 x 正方向为 →, y 正方向为 ↓
        private List<(int x, int y)> body;  // 身体每一个部位的坐标
        private SnakeMoveDirection moveDirection;
        private SnakeMoveDirection newDirection;
        private bool isDied = false;

        public (int X, int Y) Head => head;                            // 向外暴露蛇的头部坐标
        public IEnumerable<(int X, int Y)> Body => body.AsReadOnly();  // 将蛇的身体坐标作为只读的列表向外暴露

        /// <summary>
        /// 蛇是否已死亡 (如果已死亡, SnakeMap 将不会绘制该蛇
        /// </summary>
        public bool IsDied { get => isDied; }

        /// <summary>
        /// 在更新蛇状态时, 是否添加一个身体部分
        /// </summary>
        public bool NewBodySegment { get; set; }  // 指定蛇在更新状态时 是否追加一个身体部分 (当蛇吃到食物时, 请将这个设定为 true

        /// <summary>
        /// 蛇的移动方向
        /// </summary>
        public SnakeMoveDirection MoveDirection
        {
            get => moveDirection;
            set
            {
                if (!Enum.IsDefined(typeof(SnakeMoveDirection), value))
                    throw new ArgumentOutOfRangeException(nameof(value), "不是有效的方向值");
                if ((moveDirection == SnakeMoveDirection.Up && value == SnakeMoveDirection.Down) ||
                    (moveDirection == SnakeMoveDirection.Left && value == SnakeMoveDirection.Right) ||
                    (moveDirection == SnakeMoveDirection.Down && value == SnakeMoveDirection.Up) ||
                    (moveDirection == SnakeMoveDirection.Right && value == SnakeMoveDirection.Left))
                    return;
                newDirection = value;
            }
        }

        /// <summary>
        /// 创建一个新蛇
        /// </summary>
        /// <param name="direction">蛇的初始方向</param>
        /// <param name="headX">蛇头部 x 坐标</param>
        /// <param name="headY">蛇头部 y 坐标</param>
        /// <param name="length">蛇的初始长度 (蛇的身体将会根据初始长度和初始方向进行创建)</param>
        /// <exception cref="ArgumentOutOfRangeException">蛇的初始方向设定错误</exception>
        public Snake(SnakeMoveDirection direction, int headX, int headY, int length)  // 蛇的构造方法
        {
            moveDirection = direction;
            head = (headX, headY);
            body = direction switch
            {
                SnakeMoveDirection.Up => Enumerable.Range(1, length).Select(v => (headX, headY + v * 1)).ToList(),
                SnakeMoveDirection.Down => Enumerable.Range(1, length).Select(v => (headX, headY + v * -1)).ToList(),
                SnakeMoveDirection.Left => Enumerable.Range(1, length).Select(v => (headX + v * 1, headY)).ToList(),
                SnakeMoveDirection.Right => Enumerable.Range(1, length).Select(v => (headX + v * -1, headY)).ToList(),
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };
        }

        /// <summary>
        /// 更新蛇的状态
        /// </summary>
        /// <exception cref="Exception">内部错误 (一半不会出现)</exception>
        /// <returns>是否更新成功, 如果false, 表示蛇已死亡</returns>
        public bool NextStep()   // 更新蛇的状态 (游戏进行了一步, 或者刷新了一次
        {
            if (isDied)
                return false;

            body.Insert(0, head);         // 把蛇身体的最前端插入一个头部的坐标
            head = newDirection switch   // 根据移动方向, 更新蛇头部的坐标
            {
                SnakeMoveDirection.Up => (head.x, head.y - 1),       // 向上移动 y - 1
                SnakeMoveDirection.Down => (head.x, head.y + 1),     // 向下移动 y + 1
                SnakeMoveDirection.Left => (head.x - 1, head.y),     // 向左移动 x - 1
                SnakeMoveDirection.Right => (head.x + 1, head.y),    // 向右移动 x + 1
                _ => throw new Exception("Inner exception"),
            };

            if (!NewBodySegment)
                body.RemoveAt(body.Count - 1);      // 如果不添加一个身体部分, 则移除蛇身体的最末尾

            NewBodySegment = false;
            moveDirection = newDirection;

            isDied = body.Contains(head);
            return !isDied;
        }

        /// <summary>
        /// 让蛇死
        /// </summary>
        public void MakeDie()
        {
            isDied = true;
        }
    }
}
