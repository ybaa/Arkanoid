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
        Tile deck = new Tile();
        

        public Form1(){
            InitializeComponent();

            //int maxXPosition = pbCanvas.Size.Width / Settings.Width;
           // int maxIPosition = pbCanvas.Size.Height / Settings.Height;

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
            deck = new Tile();
            deck.X = 10;
            deck.Y = 34;
            
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
                

                MoveDeck();
            }
            pbCanvas.Invalidate();
        }

        private void MoveDeck() {
            switch (Settings.direction) {
                case Direction.Right:
                    deck.X++;
                    break;
                case Direction.Left:
                    deck.X--;
                    break;
            }

            int maxXPosition = pbCanvas.Size.Width / Settings.Width;

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

        private void FirstLevel() {
            
            for (int i = 0; i < 100; i += 10)
            {
                Tile element = new Tile();
                element.X = i;
                block.Add(element);
            }
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e) {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver) {
                Brush blockColor;

                for (int i = 2; i < block.Count + 2; i++) {
                    if ((i % 2) == 0)
                        blockColor = Brushes.Blue;
                    else {
                        blockColor = Brushes.Beige;
                    }

                    //canvas.FillRectangle(blockColor, new Rectangle(block[i].X * Settings.Width, block[i].Y * Settings.Height, Settings.Width, Settings.Height));
                    canvas.FillRectangle(Brushes.Yellow, new Rectangle(deck.X * Settings.Width, deck.Y * Settings.Height, Settings.Width, Settings.Height));
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
