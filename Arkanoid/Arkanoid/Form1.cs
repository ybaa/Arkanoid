using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arkanoid
{
    public partial class Form1 : Form
    {
        private List<Tile> block = new List<Tile>();
        private Tile deck = new Tile();
        private Tile ball = new Tile(); //ball needs the same values as tile (X and Y)
        

        public Form1(){
            InitializeComponent();
            
            //set default settings
            new Settings();

           // set timer and game spped
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();
            StartGame();

        }

        private void StartGame(){
            labelGameOver.Visible = false;
            new Settings();
            block.Clear();

           // deck.X = 6;
            deck.Y = 34;
           // ball.X = 28;
            ball.Y = 33;
            deck.X = 26;
            ball.X = 30;
            
            FirstLevel();

            labelScore.Text = Settings.Score.ToString();
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver == true){
                if (Input.KeyPressed(Keys.Enter)) 
                    StartGame();
                
            }
            else {
                if (Input.KeyPressed(Keys.Right))
                    Settings.direction = Direction.Right;

                else if (Input.KeyPressed(Keys.Left))
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Space))
                    Settings.ballDirection = BallDirection.LeftUp;
                

                MoveDeck();
                MoveBall();
            }
            pbCanvas.Invalidate();
        }

        private void MoveDeck() {
            switch (Settings.direction) {
                case Direction.Right:
                    deck.X+=2;
                    break;
                case Direction.Left:
                    deck.X-=2;
                    break;
                case Direction.Stop:
                    break;
            }

            int maxXPosition = pbCanvas.Size.Width / Settings.Width - 10;

            if (deck.X < 0){
                int position = 0;
                StopMoving(position);
            }

            if (deck.X > maxXPosition) {
                int position = maxXPosition;
                StopMoving(position);
            }
            
        }

        private void StopMoving(int position) {
            deck.X = position;
        }

        private void MoveBall() {
            switch (Settings.ballDirection) {
                case BallDirection.RightDown:
                    ball.X++;
                    ball.Y++;
                    break;
                case BallDirection.LeftUp:
                    ball.X--;
                    ball.Y--;
                    break;
                case BallDirection.RightUp:
                    ball.X++;
                    ball.Y--;
                    break;
                case BallDirection.LeftDown:
                    ball.X--;
                    ball.Y++;
                    break;
                case BallDirection.Stop:
                    break;
            }

            int maxXPosition = pbCanvas.Size.Width / Settings.Width;
            int maxYPosition = pbCanvas.Size.Height / Settings.Height;

            if (ball.X < 0 && Settings.ballDirection == BallDirection.LeftUp)
                Settings.ballDirection = BallDirection.RightUp;
            if (ball.X < 0 && Settings.ballDirection == BallDirection.LeftDown)
                Settings.ballDirection = BallDirection.RightDown;
            if (ball.X > maxXPosition-1 && Settings.ballDirection == BallDirection.RightUp)
                Settings.ballDirection = BallDirection.LeftUp;
            if (ball.X > maxXPosition-1 && Settings.ballDirection == BallDirection.RightDown)
                Settings.ballDirection = BallDirection.LeftDown;
            if (ball.Y < 0 && Settings.ballDirection == BallDirection.LeftUp)
                Settings.ballDirection = BallDirection.LeftDown;
            if (ball.Y < 0 && Settings.ballDirection == BallDirection.RightUp)
                Settings.ballDirection = BallDirection.RightDown;
            if (ball.Y == deck.Y - 1 && Settings.ballDirection == BallDirection.RightDown && deck.X  <= ball.X && deck.X + 8 >= ball.X)
                Settings.ballDirection = BallDirection.RightUp;
            if (ball.Y == deck.Y - 1 && Settings.ballDirection == BallDirection.LeftDown && deck.X <= ball.X && deck.X+8 >= ball.X)
                Settings.ballDirection = BallDirection.LeftUp;
            if (ball.Y > maxYPosition)
                Settings.GameOver = true;

          
            for (int i = block.Count; i < 1; i--) {
                if (ball.Y == block[i].Y && block[i].X <= ball.X  && block[i].X >= ball.X-2 ) { //this is bad, remember to change it
                    if (Settings.ballDirection == BallDirection.RightUp) {
                        block.Remove(block[i]);
                        Settings.ballDirection = BallDirection.RightDown;
                    }
                    else if (Settings.ballDirection == BallDirection.RightDown) {
                        block.Remove(block[i]);
                        Settings.ballDirection = BallDirection.RightUp;
                    }
                    else if (Settings.ballDirection == BallDirection.LeftDown) {
                        block.Remove(block[i]);
                        Settings.ballDirection = BallDirection.LeftUp;
                    }
                    else if (Settings.ballDirection == BallDirection.LeftUp) { 
                        block.Remove(block[i]);
                        Settings.ballDirection = BallDirection.LeftDown;
                    }
                    
                    
                }
            }
            
        }

        private void FirstLevel() {

            int xx = 0;
            int yy = 2;
            for (int i = 0; i < 5*15+1; i++) {      //sth wrong with element 0 so i have to skip it
                Tile element = new Tile();
                element.X = xx;
                element.Y = yy;
                block.Add(element);
                xx++;

                if (i%15==0){
                    yy++;
                    xx = 0;
                }
            }
            
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e) {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver) {
                Brush blockColor;

               
               for (int i = 1; i < block.Count; i++) {      //don't show [0] element

                   
                    if ((i % 2) == 0)
                            blockColor = Brushes.DarkSalmon;

                    else 
                            blockColor = Brushes.OrangeRed;
                      
                    

                    canvas.FillRectangle(blockColor, new Rectangle(block[i].X * Settings.Width*5, block[i].Y * Settings.Height, Settings.Width*5, Settings.Height));
                    canvas.FillRectangle(Brushes.Yellow, new Rectangle(deck.X * Settings.Width, deck.Y * Settings.Height, Settings.Width*10, Settings.Height));
                    canvas.FillEllipse(Brushes.Black, new Rectangle(ball.X * Settings.Width, ball.Y * Settings.Height, Settings.Width, Settings.Height));
                }

                

            }
            else {
                string gameOver = "Game over, your score is " + Settings.Score + "\nPress Enter to play again";
                labelGameOver.Text = gameOver;
                labelGameOver.Visible = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            Input.ChangeState(e.KeyCode, false);
        }


        
    }
}
