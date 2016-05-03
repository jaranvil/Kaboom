using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Kaboom
{
    public partial class Form1 : Form
    {
        // colors for drawing
        private Color sky1color = System.Drawing.ColorTranslator.FromHtml("#8080ff");
        private Color sky2color = System.Drawing.ColorTranslator.FromHtml("#cccccc");
        private Color currentSky;
        private Color ground1Color = System.Drawing.ColorTranslator.FromHtml("#009900");
        private Color ground2Color = System.Drawing.ColorTranslator.FromHtml("#333333");
        private Color currentGround;

        Enemy enemy;
        Paddle paddle;

        bool RightKeyIsDown = false;
        bool LeftKeyIsDown = false;

        // all bombs currently dropping
        private List<Bomb> bombs = new List<Bomb>();

        private int bombCounter = 10;
        private int bombCounterTrigger = 10;        

        // game states
        private bool pressSpaceMessage = false;
        private bool enemyMoving = true;
        private bool droppingBombs = true;
        private bool bombsAreExploding = false;
        private bool gameOver = false;

        private int score = 0;

        // level information
        int level = 1;
        int bombsThisLevel = 10;
        int bombsSoFarThisLevel = 0;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Main Form load
        /// - set starting colors
        /// - current player objects
        /// - start main timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            currentSky = sky1color;
            currentGround = ground1Color;

            Cursor.Hide();

            paddle = new Paddle(this.Width, this.Height, this.Width / 2, this.Height - 250);
            enemy = new Enemy(this.DisplayRectangle);            

            timer1.Start();
        }

        /// <summary>
        /// Paint override for main form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint_1(object sender, PaintEventArgs e)
        {
            // draw background
            SolidBrush brush = new SolidBrush(currentGround);
            e.Graphics.FillRectangle(brush,
                                   0,
                                   0,
                                   this.Width,
                                   this.Height);

            // draw sky
            brush = new SolidBrush(currentSky);
            e.Graphics.FillRectangle(brush,
                                   0,
                                   0,
                                   this.Width,
                                   this.Height / 5);

            // draw score
            brush = new SolidBrush(Color.Yellow);
            Font font = new Font("Arial", 52);
            e.Graphics.DrawString(score+"", font, brush, 10, 10);

            // check if a new bomb is needed
            if (droppingBombs)
            {
                bombCounter++;
                if (bombCounter >= bombCounterTrigger)
                {
                    if (bombsSoFarThisLevel < bombsThisLevel)
                    {
                        bombs.Add(new Bomb(level, this.Width, this.Height, enemy.x, (this.Height / 5)));
                        bombCounter = 0;
                        bombsSoFarThisLevel++;
                        Random r = new Random();
                        bombCounterTrigger = 5 + r.Next(10);
                    }
                }
            }            

            // draw bombs           
            for (int i = 0;i < bombs.Count;i++)
            {                
                bombs[i].Draw(e.Graphics, bombsAreExploding);

                if (droppingBombs)
                {
                    if (bombs[i].CheckCollision(paddle))
                    {
                        if (!bombs[i].caught)
                        {
                            bombs[i].caught = true;
                            bombs[i].width = 0;
                            bombs[i].height = 0;
                            score++;
                            if (i == bombs.Count - 1)
                                NextLevel();
                        }
                    }
                    if (bombs[i].CheckFloor())
                    {
                        BombHitsFloor();
                    }
                }
                
            }

            // draw player and computer
            enemy.Draw(e.Graphics, enemyMoving);
            paddle.Draw(e.Graphics);  

            if (pressSpaceMessage)
            {
                brush = new SolidBrush(Color.White);
                font = new Font("Arial", 26);
                e.Graphics.DrawString("press space to continue", font, brush, 10, 100);
            }

            if (gameOver)
            {
                brush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
                e.Graphics.FillRectangle(brush, this.Width / 2 - 200, this.Height / 2 - 100, 400, 200);

                brush = new SolidBrush(Color.White);
                font = new Font("Arial", 32);
                e.Graphics.DrawString("Game Over", font, brush, this.Width/2 - 150, this.Height/2 - 50);
                font = new Font("Arial", 22);
                e.Graphics.DrawString("Score: " + score, font, brush, this.Width / 2 - 150, this.Height / 2);
            }
        }

        /// <summary>
        /// All bombs have been caught, move to next level
        /// </summary>
        public void NextLevel()
        {
            level++;
            bombsSoFarThisLevel = 0;
            bombsThisLevel += 5;
            enemy.IncreaseSpeed();
            paddle.speed += 5;
            droppingBombs = true;
            enemyMoving = true;
        }        
                       
        public void BombHitsFloor()
        {
            timer1.Stop();
            
            enemyMoving = false;
            droppingBombs = false;
            bombsAreExploding = true;
            enemy.ResetSpeed();                         

            timer2.Start(); 

            if (paddle.life == 1)
            {
                gameOver = true;
            }
        }

        /// <summary>
        /// Main timmer to run gameplay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (RightKeyIsDown)
            {
                paddle.UpdatePosition(1, enemyMoving);
            }
            if (LeftKeyIsDown)
            {
                paddle.UpdatePosition(-1, enemyMoving);
            }

            Invalidate();
        }

        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                RightKeyIsDown = true;
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Left)
            {
                LeftKeyIsDown = true;
                e.Handled = true;
            }

            if (bombsAreExploding)
            {
                if (e.KeyCode == Keys.Space)
                {
                    foreach (Bomb bomb in bombs)
                    {
                        bomb.caught = true;
                    }
                    bombsAreExploding = false;
                    paddle.life--;
                    if (paddle.life == 0)
                        GameOver();
                    else
                        NextLevel();

                    pressSpaceMessage = false;

                    timer2.Stop();
                    timer1.Start();
                }
            }
        }

        /// <summary>
        /// after space bar has been hit to reset game
        /// </summary>
        public void GameOver()
        {
            // reset game
            gameOver = false;
            paddle.life = 3;
            level = 0;
            enemy.ResetSpeed();
            NextLevel();
            score = 0;
        }

        /// <summary>
        /// reset paddle movement booleans
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                RightKeyIsDown = false;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                LeftKeyIsDown = false;
                e.Handled = true;
            }
        }

        /// <summary>
        /// secondary timer for explosion animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0;i < bombs.Count;i++)
            {
                if (!bombs[i].exploded && !bombs[i].caught)
                {
                    bombs[i].exploded = true;
                    bombs[i].ChangeImage(1);
                    switchSkyColor(false);
                    break;
                }
                if (i == bombs.Count-1)
                {
                    switchSkyColor(true);
                    pressSpaceMessage = true;
                }
            }
            Invalidate();
        }

        public void switchSkyColor(bool normal)
        {
            if (normal)
            {
                currentSky = sky1color;
                currentGround = ground1Color;
            }
            else
            {
                if (currentSky == sky1color)
                {
                    currentSky = sky2color;
                    currentGround = ground1Color;
                }                    
                else
                {
                    currentSky = sky1color;
                    currentGround = ground2Color;
                }                                    
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            paddle.SetXPosition(Cursor.Position.X);
        }

    }
}
