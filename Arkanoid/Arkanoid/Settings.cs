using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid
{
    public enum Direction {Left, Right, Stop};
    public enum BallDirection { RightUp, RightDown, LeftUp, LeftDown, Stop};
    class Settings{
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static bool GameOver { get; set; }
        public static Direction direction { get; set; }
        public static BallDirection ballDirection { get; set; }

        public Settings(){
            GameOver = false;
            Points = 10;
            Score = 0;
            Speed = 14;
            Width = 14;
            Height = 14;
            direction = Direction.Stop;
            ballDirection = BallDirection.Stop;
        }

    }
}
