// 1/26/2017 - Snake Game Terri Version
// I am rewriting the code from scratch by myself see if i can do it.
// Game Logic:
// Update: Check Input, Update Player, Check Collision
// Render: Draw Player, Draw Fruit, Draw Score
// Concept:
// Snake is an easy game to understand. You start off as a "head" or a really, really small snake, roam around the playing field to collect food and you grow.
// You try to eat yourself, you die, if you run out of bounds, you die. Not much to it. So let's dig a bit deeper and understand how it works.
// Each part of the snake can be considered an instance of an object, we'll call it "SnakePart". Each part follows the part in front of it.
// The snake and it's body, usually follow along a grid-based path.Each part is assigned an X and a Y coordinate, then gets rendered to that coordinate on screen.
// The food can be an instance of "SnakePart" rather then creating a new class. Set it's X and Y randomly to fit into the screen and then render it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Snake2
{
    public partial class Form1 : Form
    {
        // Circle is a class with property x and y, the location coordinate
        // Create a location point for food and the head of snake.
        // Create a list of circle to add to the body of snake.
        private Circle food = new Circle();
        private Circle head = new Circle();
        private List<Circle> Snake = new List<Circle>();

        
        public Form1()
        {
            InitializeComponent();

            // Initialize the Settings class
            new Settings();

            // Set the timer
            gameTimer.Interval = 1000 / Settings.Speed; // in ms
            gameTimer.Tick += GameTimer_Tick; // raise timer event
            gameTimer.Start();
            // Start the Game
            StartGame();
        }
        
        // Update the picture box every tick.
        private void GameTimer_Tick(object sender, EventArgs e)
        {   
            if (Input.KeyPressed(Keys.Right))
            {
                //Debug.WriteLine("Right pressed");
                Settings.direction = Direction.Right;
            }
            else if (Input.KeyPressed(Keys.Left))
            {
                //Debug.WriteLine("Left pressed");
                Settings.direction = Direction.Left;
            }
            else if (Input.KeyPressed(Keys.Up))
            {
                //Debug.WriteLine("Up pressed");
                Settings.direction = Direction.Up;
            }
            else if (Input.KeyPressed(Keys.Down))
            {
                //Debug.WriteLine("Down pressed");
                Settings.direction = Direction.Down;
            }

            MoveSnake();
            
            pbCanvas.Invalidate();
        }

        private void StartGame()
        {
            head.X = 5;
            head.Y = 5;
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void MoveSnake()
        {
            for (int i = Snake.Count-1; i >= 0; i--)
            {
                if (i == 0) // if it is the head
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    // Detect if the head meet the food
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat(); // eat food
                    }
                }
                else // else, it is the body
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.CircleWidth;
            int maxYPos = pbCanvas.Size.Height / Settings.CircleHeight;

            Random rPos = new Random();
            food.X = rPos.Next(0, maxXPos);
            food.Y = rPos.Next(0, maxYPos);
            //Debug.WriteLine("X: {0}, Y: {1}", food.X, food.Y);
        }

        private void Eat()
        {
            Circle body = new Circle();
            body.X = Snake[Snake.Count - 1].X;
            body.Y = Snake[Snake.Count - 1].Y;
            Snake.Add(body);

            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush SnakeColor = Brushes.White;
            Brush foodColor = Brushes.Red;

            for (int i = 0; i < Snake.Count; i++)
            {
                // draw the snake on picture box
                // Position = head.X * Settings.CircleWidth, so the snake move a multiple of the snake width
                canvas.FillEllipse(SnakeColor, new Rectangle(Snake[i].X * Settings.CircleWidth, Snake[i].Y * Settings.CircleHeight, Settings.CircleWidth, Settings.CircleHeight));
            }
            
            // draw food on picture box
            canvas.FillEllipse(foodColor, new Rectangle(food.X * Settings.CircleWidth, food.Y * Settings.CircleHeight, Settings.CircleWidth, Settings.CircleHeight));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
    }
}
