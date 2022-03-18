# Null.SnakeGame

一个面向对象的, 可拓展的, 简单易用的贪吃蛇小游戏.

## 简单使用
```csharp
SnakeGameCore game = new SnakeGameCore(new SnakeMap(30, 30)    // 实例化游戏对象
{
    new Snake(SnakeMoveDirection.Right, 5, 5, 3),              // 第一条蛇
    new Snake(SnakeMoveDirection.Left, 24, 24, 3)              // 第二条蛇
});
game.SnakeMapRenderer = new SnakeMapConsoleRenderer();         // 应用控制台游戏渲染器

SnakeGameConsoleController gameController = new SnakeGameConsoleController();   // 实例化控制台游戏控制器
gameController
    .SetStartKey(ConsoleKey.Spacebar)      // 设置各种用于游戏操作的按键
    .SetPauseKey(ConsoleKey.P)
    .SetResumeKey(ConsoleKey.P)
    .SetStopKey(ConsoleKey.Escape)
    .SetSnakeKeys(game.Map[0],
        ConsoleKey.A, ConsoleKey.W, ConsoleKey.D, ConsoleKey.S)
    .SetSnakeKeys(game.Map[1],
        ConsoleKey.LeftArrow, ConsoleKey.UpArrow, ConsoleKey.RightArrow, ConsoleKey.DownArrow);

Console.WriteLine("按 空格 开始游戏 (30x30, x2)");   // 打印一个提示信息

gameController.HandleController(game);               // 开启游戏主进程

Console.WriteLine("游戏结束");
Console.ReadKey();
```

##  项目列表

### Null.SnakeGame
一个控制台应用, 包含了贪吃蛇游戏的最基本功能调用, 可以用来玩

### NullLib.SnakeGame
项目包含了本游戏的基本实现, 例如 Snake 类, SnakeMap 类, 以及 SnakeGameCore 类等等.

1. Snake 类表示一条蛇
2. SnakeMap 是承载蛇的地图
3. SnakeGameCore 是游戏的核心, 用来控制游戏的进行
4. SnakeMoveDirection 蛇的移动方向
5. ISnakeGameController 贪吃蛇游戏控制器, 该接口应该实现对用户的响应
6. ISnakeMapRenderer 游戏地图渲染器, 用来将游戏地图表示出来

### NullLib.SnakeGame.Console
项目包含了游戏在控制台应用上的接口实现, 例如控制台按键控制响应以及控制台地图渲染

1. SnakeGameController 借助 Console.ReadKey 实现使用控制台按键控制游戏
2. SnakeMapRenderer 将 SnakeMap 绘制在控制台上.


## 功能拓展
如若要为游戏拓展功能, 你可以创建一个名字为 XXX.SnakeGame.YYY 的项目, 其中 XXX 是你的链接库前缀, YYY 是你要实现的功能平台, 例如 WinForm, WPF, 或其他

实现 NullLib.SnakeGame 的接口, 并供用户调用.