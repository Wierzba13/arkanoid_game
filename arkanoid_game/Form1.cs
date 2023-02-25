using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid_game
{
    public partial class Form1 : Form
    {
        bool goToLeft, gotoRight, isGameOver;
        int score, ballX, ballY, paletteSpeed;
        Random random = new Random();
        PictureBox[] gameBlocks = new PictureBox[15];

        public Form1()
        {
            InitializeComponent();

            setupBlocks();
        }

        private void setupGame()
        {
            score = 0;
            isGameOver = false;

            // Setting the initial direction for ball
            int flag = random.Next(0, 2);
            if(flag == 0)
            {
                ballX = -random.Next(0, 5);
            } else if(flag == 1)
            {
                ballX = random.Next(0, 5);
            }

            ballY = 5;
            paletteSpeed = 12;
            scoreLbl.Visible = false;

            timer.Start(); // Start interval

        }

        private void setupControls()
        {
            ball.Left = 380;
            ball.Top = 200;
            palette.Left = 345;
            palette.Top = 400;
        }
        private void setupBlocks()
        {
            setupControls();

            int x = 0;
            int top = 50;
            int left = 100;
            
            for(int i = 0; i < gameBlocks.Length; i++)
            {
                gameBlocks[i] = new PictureBox();
                gameBlocks[i].Width = 100;
                gameBlocks[i].Height = 30;
                gameBlocks[i].Tag = "block";
                gameBlocks[i].BackColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));   // Set random color to all blocks

                if(x == 5)
                {
                    top += 50;
                    left = 100;
                    x = 0;
                }

                if(x < 5)
                {
                    x++;
                    gameBlocks[i].Top = top;
                    gameBlocks[i].Left = left;
                    this.Controls.Add(gameBlocks[i]);

                    left += 120;
                }
            }

            setupGame();
        }

        private void gameOver(string msg)
        {
            isGameOver = true;
            timer.Stop();

            scoreLbl.Visible = true;
            scoreLbl.Text = "Twój wynik: " + score + " / " + msg + " / Kliknij SPACE aby zagrać ponownie";
        }

        private void paletteMovement(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                goToLeft = true;
            }

            if(e.KeyCode == Keys.Right)
            {
                gotoRight = true;
            }
        }

        private void platteMovementStop(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goToLeft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                gotoRight = false;
            }

            if(isGameOver && e.KeyCode == Keys.Space)
            {
                foreach(PictureBox pb in gameBlocks)
                {
                    this.Controls.Remove(pb);
                }
                setupBlocks();
            }
        }

        private void game_events(object sender, EventArgs e)
        {
            if(goToLeft && palette.Left > 0)
            {
                palette.Left -= paletteSpeed;
            }

            if (gotoRight && palette.Left < 680)
            {
                palette.Left += paletteSpeed;
            }

            ball.Left += ballX;
            ball.Top += ballY;

            if(ball.Left < 0 || ball.Left > 760)
            {
                ballX = -ballX;
            }
               
            if(ball.Top < 0)
            {
                ballY = -ballY;
            }

            // https://stackoverflow.com/questions/29456028/picturebox-bounds-intersectswith-doesnt-seem-to-work
            if (ball.Bounds.IntersectsWith(palette.Bounds))
            {

                ballY = random.Next(5, 12) * -1;

                if(ballX < 0) // going to the Left
                {
                    ballX = random.Next(5, 12) * -1;
                } else
                {
                    ballX = random.Next(5, 12);
                }
            }

            foreach (Control ctl in this.Controls)   // Iterate through all Controls in Form
            {
                if (ctl is PictureBox && (string)ctl.Tag == "block")
                {
                    if (ball.Bounds.IntersectsWith(ctl.Bounds)) // Check is ball intersects with blocks
                    {
                        score++;
                        ballY = -ballY;

                        this.Controls.Remove(ctl); // Remove block, which we destroy

                    }
                }
            }

            if(score == gameBlocks.Length)
            {
                gameOver("Brawo! Udało ci się wygrać");
            }

            if(ball.Top > 425)
            {
                gameOver("Niestety przegrałeś");
            }
        }
    }
}
