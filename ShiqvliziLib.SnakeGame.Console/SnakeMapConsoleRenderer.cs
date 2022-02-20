using System;
using System.Collections.Generic;
using System.Text;

namespace ShiqvliziLib.SnakeGame.Console
{
    public class SnakeMapConsoleRenderer : ISnakeMapRenderer
    {
        private string boundStr = "VV";
        private string emptyStr = "  ";
        private string foodStr = "FF";
        private string snakeHeadStr = "SS";
        private string snakeBodyStr = "BB";
        private bool boundEnabled = true;

        public string EmptyStr { get => emptyStr; set => emptyStr = value; }
        public string FoodStr { get => foodStr; set => foodStr = value; }
        public string SnakeHeadStr { get => snakeHeadStr; set => snakeHeadStr = value; }
        public string SnakeBodyStr { get => snakeBodyStr; set => snakeBodyStr = value; }
        public string BoundStr { get => boundStr; set => boundStr = value; }

        public bool BoundEnabled { get => boundEnabled; set => boundEnabled = value; }
        public IEnumerable<string> GetMapStrings(SnakeMap map, string emptyStr, string foodStr, string snakeHeadStr, string snakeBodyStr)
        {
            int
                width = map.Width,
                height = map.Height;
            (int x, int y) food = map.Food;

            StringBuilder sb = new StringBuilder();
            string[,] mapStr = new string[width, height];

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    mapStr[i, j] = emptyStr;

            mapStr[food.x, food.y] = foodStr;

            foreach (Snake snake in map)
            {
                if (snake.IsDied)
                    continue;      // 蛇死了就不画了

                (int X, int Y) head = snake.Head;
                mapStr[head.X, head.Y] = snakeHeadStr;

                foreach ((int x, int y) body in snake.Body)
                    mapStr[body.x, body.y] = snakeBodyStr;
            }

            for (int i = 0; i < height; i++)
            {
                sb.Clear();
                for (int j = 0; j < width; j++)
                    sb.Append(mapStr[j, i]);
                yield return sb.ToString();
            }
        }

        public void DrawMap(SnakeMap map)
        {
            int width = map.Width;

            StringBuilder sb = new StringBuilder();
            if (boundEnabled)
            {
                for (int i = 0, iend = width + 2; i < iend; i++)
                {
                    sb.Append(boundStr);
                }
                sb.AppendLine();

                foreach (string line in GetMapStrings(map, emptyStr, foodStr, snakeHeadStr, snakeBodyStr))
                {
                    sb.Append(boundStr);
                    sb.Append(line);
                    sb.Append(boundStr);
                    sb.AppendLine();
                }

                for (int i = 0, iend = width + 2; i < iend; i++)
                {
                    sb.Append(boundStr);
                }
            }
            else
            {
                foreach (string line in GetMapStrings(map, emptyStr, foodStr, snakeHeadStr, snakeBodyStr))
                {
                    sb.AppendLine(line);
                }
            }

            System.Console.SetWindowSize(
                map.Width * emptyStr.Length + (boundEnabled ? boundStr.Length * 2 : 0 + 2),
                map.Height + (boundEnabled ? 2 : 0) + 2);
            System.Console.SetCursorPosition(0, 0);
            System.Console.WriteLine(sb.ToString());
        }
    }
}
