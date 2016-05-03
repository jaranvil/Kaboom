using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Kaboom.Properties;

namespace Kaboom
{
    class Bomb
    {
        private Bitmap bombImage;
        private Bitmap explosionImage;
        private Bitmap currentImage;

        private int screenWidth;
        private int screenHeight;
        private int x;
        private int y;
        private int startingX;
        public int width = 40;
        public int height = 50;
        private int dropSpeed = 10;

        public bool caught = false;
        public bool exploded = false;
        public Color color = Color.Black;

        /// <summary>
        /// Bomb constructor
        /// - setup new bomb
        /// </summary>
        /// <param name="level"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Bomb(int level, int width, int height, int x, int y)
        {
            screenWidth = width;
            screenHeight = height;
            this.x = x;
            this.startingX = x;
            this.y = y;

            bombImage = Resources.bomb;
            explosionImage = Resources.boom;
            
            currentImage = bombImage;
            dropSpeed += level;
        }

        /// <summary>
        /// Draw method for bomb
        /// update psotion and drawimage
        /// </summary>
        /// <param name="g"></param>
        /// <param name="exploding"></param>
        public void Draw(Graphics g, bool exploding)
        {
            if (!exploding)
                UpdatePosition();

            //SolidBrush brush = new SolidBrush(color);
            //g.FillRectangle(brush,
            //                x,
            //                y,
            //                width,
            //                height);
            if (!caught)
                g.DrawImage(currentImage, x, y);
        }

        /// <summary>
        /// Update position 
        /// // TODO reandom dropspeed based on level
        /// </summary>
        public void UpdatePosition()
        {
            y += dropSpeed;       
        }

        /// <summary>
        /// switch between standard sprite and bomb
        /// </summary>
        /// <param name="type"></param>
        public void ChangeImage(int type)
        {
            if (type == 0)
                currentImage = bombImage;
            else if (type == 1)
                currentImage = explosionImage;
        }

        /// <summary>
        /// Collision detection with paddle
        /// </summary>
        /// <param name="paddle"></param>
        /// <returns></returns>
        public Boolean CheckCollision(Paddle paddle)
        {
            int paddleHeight = 0;

            if (paddle.life == 3)
                paddleHeight = paddle.height;
            else if (paddle.life == 2)
                paddleHeight = paddle.height - (paddle.height/3);
            else if (paddle.life == 1)
                paddleHeight = paddle.height/3;

            int paddleBtm = paddle.y + paddleHeight;

            // TODO - remove these variables
            //        they are IReadOnlyCollection for human readability
            int paddleTop = paddle.y;           
            int paddleLeft = paddle.x;
            int paddleRight = paddle.x + paddle.width;
            int bottomOfBomb = y + height;
            int topOfBomb = y;
            int leftOfBomb = x;
            int rightOfBomb = x + width;            

            // if bottom of bomb is within paddle verticaly             OR      if top of bomb is within paddle verticaly
            if ((bottomOfBomb >= paddleTop && bottomOfBomb <= paddleBtm) || topOfBomb >= paddleTop && topOfBomb <= paddleBtm)
            {
                // bomb could be hit by paddel
                //if right of bomb is within paddle horizontaly             OR      if left of bomb is within paddle horizontaly
                if ((rightOfBomb >= paddleLeft && rightOfBomb <= paddleRight) || leftOfBomb >= paddleLeft && leftOfBomb <= paddleRight)
                {
                    // collision = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// collision detection with floor
        /// </summary>
        /// <returns></returns>
        public Boolean CheckFloor()
        {
            if (y + height >= screenHeight && !caught)
                return true;
            else
                return false;
        }

      
    }
}
