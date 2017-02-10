using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake2
{
    public enum Direction
    {
        Up, Down, Right, Left
    };
    class Settings
    {
        // Use static keyword so i can access the member without creating an object of the class
        public static int Speed { get; set; } 
        public static Direction direction { get; set; }
        public static int CircleWidth { get; set; }
        public static int CircleHeight { get; set; }
        public static int Points { get; set; }
        public static int Score { get; set; }
        public static bool GameOver { get; set; }

        public Settings()
        {
            Speed = 5; // the higher the number, the faster the snake moves
            direction = Direction.Right;
            CircleWidth = 15;
            CircleHeight = 15;
            Points = 100;
            Score = 0;
            GameOver = false;
        }
        
    }
}
