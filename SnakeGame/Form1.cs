// 1/23/2017 - Snake Game by Microsoft Developer:
// https://code.msdn.microsoft.com/windowsapps/MySnakegame-0ced8739
// This is exactly the same code as the sample code from Microsoft website shown above.
// I use this codes to study C#
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
//using System.Windows.Shapes; // Need to add PresentationFramework in the Assemblies/Framework
//using System.Windows.Input;  // Need to add PresentationCore in the Assemblies/Framework. For keyboard events


namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private Circle food = new Circle();
        private List<Circle> Snake = new List<Circle>();
                
        public Form1()
        {
            InitializeComponent();

            // Set setting to default - What is Settings, i guess it is the properties' settings
            new Settings();

            // Set game speed and start timer
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen; // Update screen
            gameTimer.Start();

            // Start New game
            StartGame();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            // Set settings to default
            new Settings();

            // Create new player object
            Snake.Clear();

            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        // Place random food object
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            // Check for Game Over
            if (Settings.GameOver)
            {
                if(Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }
            pbCanvas.Invalidate();
        }

        // This function is for practice purposes, not related to this game at all.
        private void DrawCircle()
        {
            Graphics graphics = this.CreateGraphics();
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(50, 50, 150, 150);
            graphics.DrawEllipse(Pens.Black, rectangle);
            graphics.DrawRectangle(Pens.Red, rectangle);
        }

        // This function is for practice purposes, not related to this game at all.
        private void DrawFilledEllipse()
        {
            SolidBrush brush1 = new SolidBrush(Color.Red);
            Graphics formGraphics = this.CreateGraphics();
            formGraphics.FillEllipse(brush1, new System.Drawing.Rectangle(0, 0, 200, 300));
            brush1.Dispose();
            formGraphics.Dispose();
        }
       
        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if(!Settings.GameOver)
            {
                // Set color of snake
                Brush snakeColor;

                // Draw snake
                for(int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                        snakeColor = Brushes.Purple; // Draw head
                    else
                        snakeColor = Brushes.Purple;  // Draw rest of body
                    
                    // Draw snake
                    canvas.FillEllipse(snakeColor, new Rectangle(Snake[i].X * Settings.Width,
                                                                 Snake[i].Y * Settings.Height,
                                                                 Settings.Width, Settings.Height));

                    // Draw Food
                    canvas.FillEllipse(Brushes.Red, new Rectangle(food.X * Settings.Width,
                                        food.Y * Settings.Height, Settings.Width, Settings.Height));
                }
            }
           else
            {
                string gameOver = "Game Over!!!\nFinal Score: " + Settings.Score + "\nPress Enter for New Game";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        private void MovePlayer()
        {
            for(int i = Snake.Count-1; i >= 0; i--)
            {
                // Move head
                if(i == 0)
                {
                    switch(Settings.direction)
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
                    // Get maximum X and Y position
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;
                    // Detect collission with game borders.
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }
                    // Detect collission with body
                    for(int j = 1; j < Snake.Count; j++)
                    {
                        if(Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }
                    // Detect collision with food piece
                    if(Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    // Move body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {
            // Add circle to body
            Circle circle = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            // Update Score
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("My Name is Terri Chong");
        }
    }
}
