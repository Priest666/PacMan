using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacMan
{
    public partial class Game : Form
    {
        bool moveLeft, moveRight, moveUp, moveDown;
        int speed = 6;
        int redSp = 6;
        int blueSp = 6;
        int pinkSp = 6;
        int score = 0;
        int health = 3;

        int pacTop;
        int pacLeft;

        // Sprites for animation
        List<Image> pacmanLeftSprites = new List<Image>();
        List<Image> pacmanRightSprites = new List<Image>();
        List<Image> pacmanUpSprites = new List<Image>();
        List<Image> pacmanDownSprites = new List<Image>();

        int currentSpriteIndex = 0;
        Timer animationTimer = new Timer();

        public Game()
        {
            InitializeComponent();

            pacmanLeftSprites.Add(Properties.Resources.pac_left1);
            pacmanLeftSprites.Add(Properties.Resources.pac_left2);
            pacmanLeftSprites.Add(Properties.Resources.pac_left3);
            pacmanLeftSprites.Add(Properties.Resources.pac_left4);

            pacmanRightSprites.Add(Properties.Resources.pac_right1);
            pacmanRightSprites.Add(Properties.Resources.pac_right2);
            pacmanRightSprites.Add(Properties.Resources.pac_right3);
            pacmanRightSprites.Add(Properties.Resources.pac_right4);

            pacmanUpSprites.Add(Properties.Resources.pac_up1);
            pacmanUpSprites.Add(Properties.Resources.pac_up2);
            pacmanUpSprites.Add(Properties.Resources.pac_up3);
            pacmanUpSprites.Add(Properties.Resources.pac_up4);

            pacmanDownSprites.Add(Properties.Resources.pac_down1);
            pacmanDownSprites.Add(Properties.Resources.pac_down2);
            pacmanDownSprites.Add(Properties.Resources.pac_down3);
            pacmanDownSprites.Add(Properties.Resources.pac_down4);

            // Timer for animation, controls how fast sprites are switched
            animationTimer.Interval = 50;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void Key_Down(object sender, KeyEventArgs e)
        {
            // Change direction immediately without delay
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = true;
                moveRight = false;
                moveUp = false;
                moveDown = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                moveRight = true;
                moveLeft = false;
                moveUp = false;
                moveDown = false;
            }
            else if (e.KeyCode == Keys.Up)
            {
                moveUp = true;
                moveLeft = false;
                moveRight = false;
                moveDown = false;
            }
            else if (e.KeyCode == Keys.Down)
            {
                moveDown = true;
                moveLeft = false;
                moveRight = false;
                moveUp = false;
            }
        }

        private void Key_Up(object sender, KeyEventArgs e)
        {
            // Release movement depending on which key is released
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                moveRight = false;
            }
            else if (e.KeyCode == Keys.Up)
            {
                moveUp = false;
            }
            else if (e.KeyCode == Keys.Down)
            {
                moveDown = false;
            }
        }

        private void Game_Load(object sender, EventArgs e)
        {
            pacLeft = pacman.Left;
            pacTop = pacman.Top;
        }


        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // Switch to the next sprite in the list based on the direction Pac-Man is moving
            if (moveLeft == true)
            {
                pacman.Image = pacmanLeftSprites[currentSpriteIndex];
            }
            else if (moveRight == true)
            {
                pacman.Image = pacmanRightSprites[currentSpriteIndex];
            }
            else if (moveUp == true)
            {
                pacman.Image = pacmanUpSprites[currentSpriteIndex];
            }
            else if (moveDown == true)
            {
                pacman.Image = pacmanDownSprites[currentSpriteIndex];
            }

            // Switch to the next sprite in the list
            currentSpriteIndex++;
            if (currentSpriteIndex >= pacmanLeftSprites.Count)
            {
                currentSpriteIndex = 0; // Loop through sprites
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            LbScore.Text = "Score: " + score;
            // Save previous positions
            int pacPreviousLeft = pacman.Left;
            int pacPreviousTop = pacman.Top;

            ghBlue.Left += blueSp;
            ghRed.Left += redSp;
            ghPink.Left -= pinkSp;


            if (ghPink.Bounds.IntersectsWith(pictureBox12.Bounds))
            {
                pinkSp = -pinkSp;
                ghPink.Image = Properties.Resources.pink_gh_right;
            }

            if (ghPink.Bounds.IntersectsWith(pictureBox21.Bounds))
            {
                pinkSp = -pinkSp;
                ghPink.Image = Properties.Resources.pink_gh_left;
            }


            if (ghBlue.Bounds.IntersectsWith(pictureBox19.Bounds))
            {
                blueSp = -blueSp;
                ghBlue.Image = Properties.Resources.blue_gh_left;
            }

            if (ghBlue.Bounds.IntersectsWith(pictureBox16.Bounds))
            {
                blueSp = -blueSp;
                ghBlue.Image = Properties.Resources.blue_gh_right;
            }

            if (ghRed.Bounds.IntersectsWith(pictureBox19.Bounds))
            {
                redSp = -redSp;
            }

            if (ghRed.Bounds.IntersectsWith(pictureBox16.Bounds))
            {
                redSp = -redSp;
            }

            if (moveLeft == true)
            {
                pacman.Left -= speed;
            }
            if (moveRight == true)
            {
                pacman.Left += speed;
            }
            if (moveUp == true)
            {
                pacman.Top -= speed;
            }
            if (moveDown == true)
            {
                pacman.Top += speed;
            }

            if (pacman.Left < 0)
            {
                pacman.Left = 1460;
            }

            if (pacman.Left > 1460)
            {
                pacman.Left = 0;
            }

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox)
                {
                    if ((string)c.Tag == "wall") // Collision with walls
                    {

                        if (pacman.Bounds.IntersectsWith(c.Bounds))
                        {
                            // If a collision occurs, restore position based on movement
                            if (moveLeft == true)
                            {
                                pacman.Left = pacPreviousLeft; // Restore position before the collision
                            }
                            else if (moveRight == true)
                            {
                                pacman.Left = pacPreviousLeft;
                            }
                            else if (moveUp == true)
                            {
                                pacman.Top = pacPreviousTop;
                            }
                            else if (moveDown == true)
                            {
                                pacman.Top = pacPreviousTop;
                            }
                        }
                    }

                    if ((string)c.Tag == "coin" && c.Visible == true) // Collision with coins
                    { 
                        if (pacman.Bounds.IntersectsWith(c.Bounds))
                        {
                            score += 1;
                            c.Visible = false;                            
                        }
                    }

                    if ((string)c.Tag == "ghost")
                    {
                        if (pacman.Bounds.IntersectsWith(c.Bounds))
                        {
                            health--;
                            pacman.Left = pacLeft;
                            pacman.Top = pacTop;
                            
                            if (health == 2)
                            {
                                Hp1.Visible = false;
                            }

                            if (health == 1)
                            {
                                Hp2.Visible = false;
                            }

                            if (health == 0)
                            {
                                Hp3.Visible = false;
                                GameTimer.Stop();
                                MessageBox.Show($"You died your score was {score}");
                                RestartGame();
                            }
                        }
                    }
                }
            }

            if (score == 87)
            {
                GameTimer.Stop();
                MessageBox.Show("Congrats you won!!");                
                RestartGame();
            }
        }

        private void RestartGame()
        {           
            LbScore.Text = "Score: 0";
            score = 0;
            health = 3;
            pacman.Left = pacLeft;
            pacman.Top = pacTop;

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox)
                {
                    if ((string)c.Tag == "coin")
                    {
                        c.Visible = true;
                    }

                    if ((string)c.Tag == "health")
                    {
                        c.Visible = true;
                    }
                }


            }
            GameTimer.Start();
        }
    }
}
